---
title: "Keamanan RLS, Trigger, dan Fungsi Database"
aliases:
  - Keamanan Database
  - RLS Policies
  - Database Triggers
  - enforce_app_users_security
tags:
  - database
  - security
  - rls
  - trigger
  - supabase
layer: database
type: explanation
updated: 2026-11-21
---

# 🔐 Keamanan RLS, Trigger, dan Fungsi Database

⬅️ Kembali ke [[MOC - Database Supabase]] | Lihat juga: [[Tabel app_users]] · [[ADR-004 - Arsitektur Autentikasi Kustom SHA-256 dan Proteksi Trigger Privilege Escalation]]

> [!info] Gambaran Umum Keamanan Database
> Karena aplikasi desktop **IT Asset Management** berkomunikasi langsung ke Supabase PostgREST menggunakan **Publishable Key** (`anon` role), keamanan database tidak hanya mengandalkan validasi di kode C# (`AuthController`), melainkan diperkuat di level mesin PostgreSQL 17 melalui **Row-Level Security (RLS)**, **Table Privileges (`GRANT`)**, **Check Constraints**, dan **PL/pgSQL Trigger**.

---

## 1. Hak Akses Role (`information_schema.role_table_grants`)

Hasil inspeksi tabel `information_schema.role_table_grants` pada schema `public` menunjukkan matriks hak akses berikut untuk role `anon` dan `authenticated`:

| Nama Tabel | `SELECT` | `INSERT` | `UPDATE` | `DELETE` | Catatan Keamanan |
| :--- | :---: | :---: | :---: | :---: | :--- |
| [[Tabel app_users\|public.app_users]] | ✅ | ✅ | ✅ | ❌ **Ditolak** | Role `anon` & `authenticated` **tidak diberi hak `DELETE`** pada `app_users`, sehingga akun pengguna tidak dapat dihapus melalui REST API client. |
| [[Tabel assets\|public.assets]] | ✅ | ✅ | ✅ | ✅ | Akses penuh CRUD untuk operasional inventaris. |
| [[Tabel karyawan\|public.karyawan]] | ✅ | ✅ | ✅ | ✅ | Akses penuh CRUD untuk manajemen karyawan. |
| [[Tabel alokasi\|public.alokasi]] | ✅ | ✅ | ✅ | ✅ | Akses penuh CRUD untuk sirkulasi peminjaman. |
| [[Tabel riwayat\|public.riwayat]] | ✅ | ✅ | ✅ | ✅ | Akses CRUD (termasuk fitur Bersihkan Riwayat oleh Admin). |

---

## 2. Daftar Lengkap 7 Kebijakan Row-Level Security (`pg_policies`)

Seluruh 5 tabel di schema `public` memiliki status `rls_enabled = true`. Berikut adalah ketujuh kebijakan RLS yang aktif di server:

| Tabel | Nama Policy | Command (`cmd`) | Target Roles | Ekspresi `USING` (`qual`) | Ekspresi `WITH CHECK` |
| :--- | :--- | :---: | :--- | :--- | :--- |
| `app_users` | `app_users_client_select` | `SELECT` | `{anon, authenticated}` | `true` | `null` |
| `app_users` | `app_users_client_insert` | `INSERT` | `{anon, authenticated}` | `null` | `char_length(btrim(username)) >= 3 AND POSITION(' ' IN username) = 0 AND password ~ '^[0-9a-f]{64}$' AND role = ANY (ARRAY['Admin', 'Staff'])` |
| `app_users` | `app_users_client_update` | `UPDATE` | `{anon, authenticated}` | `true` | `char_length(btrim(username)) >= 3 AND POSITION(' ' IN username) = 0 AND password ~ '^[0-9a-f]{64}$' AND role = ANY (ARRAY['Admin', 'Staff'])` |
| `assets` | `assets_client_all` | `ALL` | `{anon, authenticated}` | `true` | `true` |
| `karyawan` | `karyawan_client_all` | `ALL` | `{anon, authenticated}` | `true` | `true` |
| `alokasi` | `alokasi_client_all` | `ALL` | `{anon, authenticated}` | `true` | `true` |
| `riwayat` | `riwayat_client_all` | `ALL` | `{anon, authenticated}` | `true` | `true` |

---

## 3. Bedah Fungsi PL/pgSQL & Trigger di Schema `public`

Berdasarkan hasil query ke `pg_proc` dan `pg_trigger` (diperbarui 2026-11-21), terdapat **5 fungsi** dan **4 table trigger** di schema `public`, ditambah 1 event trigger:

### A00. Fungsi `public.sync_riwayat_pelaku()` & `public.sync_app_users_nama_to_riwayat()` *(baru, 2026-11-21)*
- **`trg_sync_riwayat_pelaku`** (`BEFORE INSERT OR UPDATE ON public.riwayat`): otomatis mengisi `NEW.username_pelaku` dari `public.app_users` berdasarkan `NEW.oleh_user` (atau sebaliknya), sehingga Foreign Key `riwayat_username_pelaku_fkey` selalu merujuk ke Primary Key `app_users(username)` tanpa mewajibkan `app_users.nama_lengkap` menjadi `UNIQUE`.
- **`trg_sync_app_users_nama_to_riwayat`** (`AFTER UPDATE OF nama_lengkap ON public.app_users`): otomatis menyinkronkan `riwayat.oleh_user = NEW.nama_lengkap` untuk seluruh baris riwayat dengan `username_pelaku = NEW.username`.

### A0. Fungsi `public.enforce_karyawan_kontak_unique()` & Trigger `trg_karyawan_kontak_unique` *(baru, 2026-11-21)*

- **Trigger**: `CREATE TRIGGER trg_karyawan_kontak_unique BEFORE INSERT OR UPDATE OF kontak ON karyawan FOR EACH ROW EXECUTE FUNCTION enforce_karyawan_kontak_unique()`
- **Tipe**: `SECURITY INVOKER` (`prosecdef = false`), `SET search_path TO 'public'`. Tidak memakai `SECURITY DEFINER`, sehingga tidak melewati RLS dan tidak menjadi endpoint RPC publik.
- **Definisi Lengkap di Server**:

```sql
CREATE OR REPLACE FUNCTION public.enforce_karyawan_kontak_unique() RETURNS trigger
 LANGUAGE plpgsql
 SET search_path TO 'public'
AS $f$
DECLARE v_baru text;
BEGIN
  v_baru := lower(regexp_replace(coalesce(new.kontak, ''), '[\s().\-]', '', 'g'));
  IF v_baru = '' THEN RETURN new; END IF;
  IF tg_op = 'UPDATE' AND v_baru = lower(regexp_replace(coalesce(old.kontak, ''), '[\s().\-]', '', 'g')) THEN RETURN new; END IF;
  PERFORM pg_advisory_xact_lock(hashtext('karyawan_kontak_unique'));
  IF EXISTS (SELECT 1 FROM public.karyawan k WHERE k.nik <> new.nik AND lower(regexp_replace(k.kontak, '[\s().\-]', '', 'g')) = v_baru) THEN
    RAISE EXCEPTION 'Kontak % sudah digunakan karyawan lain', new.kontak USING errcode = '23505', constraint = 'karyawan_kontak_unique';
  END IF;
  RETURN new;
END;
$f$;
```

> [!note] Penjagaan Ganda bersama `karyawan_kontak_unique_idx`
> Selain Partial Functional `UNIQUE INDEX` `karyawan_kontak_unique_idx`, trigger ini memberikan pesan penolakan yang jelas (`Kontak % sudah digunakan karyawan lain`, SQLSTATE `23505`) dan `pg_advisory_xact_lock` untuk mencegah dua transaksi bersamaan lolos. Detail: [[Tabel karyawan]] dan [[ADR-007 - Relasi Riwayat ke Pengguna dan Kontak Karyawan Unik]].

### A. Fungsi `public.enforce_app_users_security()` & Trigger `trg_enforce_app_users_security`

- **Trigger**: `CREATE TRIGGER trg_enforce_app_users_security BEFORE INSERT OR UPDATE ON app_users FOR EACH ROW EXECUTE FUNCTION enforce_app_users_security()`
- **Tipe**: `SECURITY INVOKER` (`prosecdef = false`), `SET search_path TO 'public'` (aman dari serangan *search_path hijacking* sesuai **Supabase Security Checklist**).
- **Definisi Lengkap di Server**:

```sql
CREATE OR REPLACE FUNCTION public.enforce_app_users_security()
 RETURNS trigger
 LANGUAGE plpgsql
 SET search_path TO 'public'
AS $function$
BEGIN
  -- Pertahankan huruf besar/kecil (case-sensitive) pada username, hanya hapus spasi di ujung
  NEW.username := btrim(NEW.username);
  NEW.nama_lengkap := btrim(COALESCE(NEW.nama_lengkap, ''));

  IF NEW.nama_lengkap = '' THEN
    NEW.nama_lengkap := NEW.username;
  END IF;

  IF TG_OP = 'INSERT' THEN
    -- Jika sudah ada minimal 1 Admin di database, paksa semua pendaftaran baru menjadi Staff
    IF EXISTS (SELECT 1 FROM public.app_users WHERE role = 'Admin') THEN
      NEW.role := 'Staff';
    ELSE
      NEW.role := 'Admin';
    END IF;
  ELSIF TG_OP = 'UPDATE' THEN
    -- Kunci kolom role agar pengguna tidak bisa menaikkan hak aksesnya sendiri (Privilege Escalation)
    NEW.role := OLD.role;
  END IF;

  RETURN NEW;
END;
$function$;
```

> [!important] Mengapa Trigger Ini Sangat Penting?
> Karena `SupabaseUrl` dan `SupabaseKey` berada di dalam file `App.config` pada komputer client, pengguna teknis yang mencoba mengirim request HTTP `POST` atau `PATCH` langsung ke `/rest/v1/app_users` menggunakan `curl` atau Postman untuk membuat akun ber-role `"Admin"` atau mengubah role akun `"Staff"` menjadi `"Admin"` **akan selalu digagalkan secara transparan oleh trigger ini**!
> - Pada `INSERT`: Selama sudah ada 1 Admin di tabel `app_users`, `NEW.role` akan selalu ditimpa menjadi `'Staff'`.
> - Pada `UPDATE`: `NEW.role` selalu dikembalikan ke `OLD.role`.

---

### B. Event Trigger Function `public.rls_auto_enable()`

Fungsi kedua di schema `public` adalah `public.rls_auto_enable()` (`RETURNS event_trigger`, `SET search_path TO 'pg_catalog'`):

```sql
CREATE OR REPLACE FUNCTION public.rls_auto_enable()
 RETURNS event_trigger
 LANGUAGE plpgsql
 SET search_path TO 'pg_catalog'
AS $function$
DECLARE
  cmd record;
  tbl name;
BEGIN
  FOR cmd IN
    SELECT *
    FROM pg_event_trigger_ddl_commands()
    WHERE command_tag IN ('CREATE TABLE', 'CREATE TABLE AS', 'SELECT INTO')
      AND object_type IN ('table','partitioned table')
  LOOP
     IF cmd.schema_name IS NOT NULL AND cmd.schema_name IN ('public') AND cmd.schema_name NOT IN ('pg_catalog','information_schema') AND cmd.schema_name NOT LIKE 'pg_toast%' AND cmd.schema_name NOT LIKE 'pg_temp%' THEN
      BEGIN
        SELECT c.relname INTO tbl FROM pg_catalog.pg_class c WHERE c.oid = cmd.objid;
        EXECUTE format('alter table if exists %I.%I enable row level security', cmd.schema_name, tbl);
        RAISE LOG 'rls_auto_enable: enabled RLS on %', cmd.object_identity;
      EXCEPTION
        WHEN OTHERS THEN
          RAISE LOG 'rls_auto_enable: failed to enable RLS on %', cmd.object_identity;
      END;
     ELSE
        RAISE LOG 'rls_auto_enable: skip % (either system schema or not in enforced list: %.)', cmd.object_identity, cmd.schema_name;
     END IF;
  END LOOP;
END;
$function$;
```
Fungsi ini memastikan bahwa setiap kali ada tabel baru yang dibuat di schema `public` (`CREATE TABLE`), PostgreSQL secara otomatis menjalankan `ALTER TABLE ... ENABLE ROW LEVEL SECURITY`, sehingga tidak ada tabel baru yang tidak sengaja terekspos ke Data API tanpa proteksi RLS.

---

## 4. Risiko yang Diketahui (Hasil Audit 2026-11-21, Tidak Diubah)

> [!warning] Semua Policy RLS Bernilai `true` untuk Role `anon`
> Semua kebijakan di atas mengizinkan `anon` dan `authenticated` membaca (dan pada 4 tabel operasional juga mengubah/menghapus) seluruh baris. Siapa pun yang memegang `SupabaseKey` dari `App.config` dapat memanggil REST API secara langsung, termasuk membaca kolom `password` (hash SHA-256 tanpa *salt*) di `app_users`. Ini konsekuensi desain aplikasi desktop dengan login buatan sendiri dan tanpa Supabase Auth ([[ADR-004 - Arsitektur Autentikasi Kustom SHA-256 dan Proteksi Trigger Privilege Escalation]]). Memperbaikinya butuh perombakan arsitektur (Supabase Auth atau Edge Function dengan `service_role` di server), bukan perubahan skema kecil, sehingga sengaja **tidak diubah** dalam perbaikan relasi ini. Untuk penggunaan produksi sungguhan, jangan membagikan `App.config` yang berisi key tersebut.
