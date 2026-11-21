---
title: "Tabel app_users"
aliases:
  - public.app_users
  - app_users
  - Tabel Pengguna
tags:
  - database
  - tabel
  - auth
  - supabase
layer: database
type: reference
updated: 2026-11-21
---

# 👤 Tabel `public.app_users`

⬅️ Kembali ke [[MOC - Database Supabase]] | [[Skema Database & ERD Lengkap]] | Lihat juga: [[AuthController]] · [[UserSession & OperationResult]]

> [!info] Deskripsi Tabel
> Tabel `public.app_users` menyimpan kredensial akun pengguna aplikasi (**Admin IT** dan **Staff IT**) beserta **Nama Pengguna (`nama_lengkap`)** yang dikaitkan dengan `username` login. Tabel ini mendukung **`username` yang *case-sensitive*** (mempertahankan huruf besar dan kecil apa adanya) serta dilindungi secara ketat oleh **4 Check Constraints**, **3 Kebijakan RLS terpisah** (tanpa izin `DELETE` dari client), dan **Trigger `trg_enforce_app_users_security`** untuk mencegah *privilege escalation*.

---

## 1. Struktur Kolom & Tipe Data Presisi

| Posisi | Nama Kolom | Tipe Data SQL (`udt_name`) | Panjang Maks | Nullable? | Default / Identity | Peran & Keterangan |
| :-: | :--- | :--- | :---: | :---: | :--- | :--- |
| 1 | `id` | `bigint` (`int8`) | Precision 64, Scale 0 | `NO` | `GENERATED ALWAYS AS IDENTITY` | Nomor urut baris internal database. |
| 2 | **`username`** | `character varying` (`varchar`) | `50` | `NO` | **PRIMARY KEY** | Username unik untuk login (**case-sensitive**, minimal 3 karakter, tanpa spasi). |
| 3 | `password` | `character varying` (`varchar`) | `100` | `NO` | - | Hash **SHA-256** dari password pengguna (wajib 64 karakter hex huruf kecil `[0-9a-f]{64}`). |
| 4 | `role` | `character varying` (`varchar`) | `10` | `NO` | - | Hak akses pengguna: `'Admin'` atau `'Staff'`. |
| 5 | **`nama_lengkap`** | `character varying` (`varchar`) | `100` | `NO` | `DEFAULT ''` | Nama pengguna / nama lengkap yang ditampilkan di antarmuka aplikasi (`UserSession.DisplayName`). Boleh sama antar akun karena relasi `riwayat` merujuk ke Primary Key `username`. |

---

## 2. Daftar Constraints pada `public.app_users`

| Nama Constraint | Tipe | Definisi SQL (`pg_get_constraintdef`) | Tujuan Validasi |
| :--- | :---: | :--- | :--- |
| `app_users_pkey` | `PRIMARY KEY` | `PRIMARY KEY (username)` | Menjamin keunikan `username` secara *case-sensitive* sekaligus membuat B-Tree Unique Index otomatis pada `username` (dan menjadi target Foreign Key `riwayat_username_pelaku_fkey`). |
| `app_users_username_check` | `CHECK` | `CHECK (char_length(btrim(username::text)) >= 3 AND POSITION((' '::text) IN (username)) = 0)` | Menolak username yang panjangnya kurang dari 3 karakter setelah di-trim atau mengandung karakter spasi. |
| `app_users_nama_lengkap_check` | `CHECK` | `CHECK (char_length(btrim(nama_lengkap::text)) >= 2)` | Menjamin setiap akun memiliki nama pengguna tampilan minimal 2 karakter. |
| `app_users_password_hash_check` | `CHECK` | `CHECK (password::text ~ '^[0-9a-f]{64}$'::text)` | Memastikan database **tidak pernah menyimpan password plaintext**; hanya menerima string hash SHA-256 hex lowercase 64 karakter. |
| `app_users_role_check` | `CHECK` | `CHECK (role::text = ANY (ARRAY['Admin'::text, 'Staff'::text]))` | Membatasi nilai kolom `role` hanya boleh `'Admin'` atau `'Staff'`. |

---

## 3. Index, Trigger, & Kebijakan Keamanan (RLS)

### A. Index (`pg_indexes`)
- **`app_users_pkey`**: `CREATE UNIQUE INDEX app_users_pkey ON public.app_users USING btree (username)`
  Digunakan secara langsung oleh query `DataStore.FindUser()` (`username=eq.<cleanUsername>`), sehingga pencarian saat Login berlangsung dalam kompleksitas waktu $\mathcal{O}(\log N)$ (*Index Scan*) dan membandingkan string secara *case-sensitive*.

### B. Relasi Masuk (*Incoming Foreign Key*) *(baru, 2026-11-21)*
- **`riwayat_username_pelaku_fkey`** dari [[Tabel riwayat|public.riwayat]]:
  ```sql
  FOREIGN KEY (username_pelaku) REFERENCES public.app_users(username)
  ON UPDATE CASCADE ON DELETE RESTRICT
  ```
  - **Kardinalitas**: `1` akun (`username` PK) dapat memiliki `0..N` baris riwayat.
  - `ON UPDATE CASCADE`: mengubah `username` di [[UC_Profil]] otomatis memperbarui `riwayat.username_pelaku`.
  - Trigger `trg_sync_app_users_nama_to_riwayat` (`AFTER UPDATE OF nama_lengkap ON public.app_users`) otomatis memperbarui `riwayat.oleh_user` untuk semua baris milik `username` tersebut.
  - `ON DELETE RESTRICT`: akun yang sudah punya riwayat tidak dapat dihapus (client memang tidak punya hak `DELETE` pada tabel ini).
  - Detail alasan: [[ADR-007 - Relasi Riwayat ke Pengguna dan Kontak Karyawan Unik]].

### C. Trigger Keamanan (`trg_enforce_app_users_security`)
```sql
CREATE TRIGGER trg_enforce_app_users_security
BEFORE INSERT OR UPDATE ON public.app_users
FOR EACH ROW EXECUTE FUNCTION public.enforce_app_users_security();
```
Fungsi trigger `enforce_app_users_security()` melakukan tiga proteksi otomatis di sisi server:
1. Menghapus spasi di ujung `NEW.username := btrim(NEW.username)` dan `NEW.nama_lengkap := btrim(COALESCE(NEW.nama_lengkap, ''))` **tanpa mengubah huruf besar/kecil** (`username` tetap *case-sensitive*). Jika `nama_lengkap` kosong, otomatis diisi dengan `NEW.username`.
2. Saat `INSERT`: Jika sudah ada minimal 1 baris dengan `role = 'Admin'` di `public.app_users`, maka semua pendaftaran baru **dipaksa** menjadi `NEW.role := 'Staff'` (walaupun client memanipulasi payload JSON dan mengirim `"role": "Admin"`). Jika tabel belum memiliki Admin sama sekali (*bootstrap* awal), akun pertama otomatis menjadi `'Admin'`.
3. Saat `UPDATE`: Mengunci `NEW.role := OLD.role` sehingga tidak ada pengguna yang dapat mengubah rolenya sendiri dari `Staff` menjadi `Admin` (*Anti-Privilege Escalation*).
👉 Lihat kode lengkap fungsi PL/pgSQL-nya di: [[Keamanan RLS, Trigger, dan Fungsi Database]].

### D. Kebijakan Row-Level Security (RLS) & Grants
Berbeda dengan 4 tabel operasional lainnya, tabel `app_users` **tidak mengizinkan operasi `DELETE`** pada role `anon` dan `authenticated`:
- **Role Grants**: `INSERT, REFERENCES, SELECT, TRIGGER, TRUNCATE, UPDATE` (tanpa `DELETE`).
- **Policies**:
  1. `app_users_client_select` (`FOR SELECT TO anon, authenticated USING (true)`)
  2. `app_users_client_insert` (`FOR INSERT TO anon, authenticated WITH CHECK (...)`)
  3. `app_users_client_update` (`FOR UPDATE TO anon, authenticated USING (true) WITH CHECK (...)`)

---

## 4. Pemetaan ke Kode C# & Endpoint PostgREST

| Operasi di Aplikasi | Fungsi Pemanggil di C# | HTTP Method & Endpoint PostgREST |
| :--- | :--- | :--- |
| Cari akun secara *case-sensitive* saat Login / Cek duplikasi saat Register & UpdateProfile | [[DataStore.cs (REST Client & In-Memory Cache)\|DataStore.FindUser(username)]] ([L188-L200](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L188-L200)) | `GET /rest/v1/app_users?select=username,nama_lengkap,password,role&username=eq.{username}&limit=1` |
| Registrasi akun baru (`username` + `nama_lengkap`) | [[AuthController\|AuthController.Register]] ([L99-L135](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AuthController.cs#L99-L135)) | `POST /rest/v1/app_users` dengan body `{ username, nama_lengkap, password, role = "Staff" }` |
| Ubah username (*case-sensitive*), nama pengguna, dan/atau password | [[AuthController\|AuthController.UpdateProfile]] ([L138-L227](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AuthController.cs#L138-L227)) | `PATCH /rest/v1/app_users?username=eq.{savedCurrent}` dengan body `{ username, nama_lengkap, password }` |
