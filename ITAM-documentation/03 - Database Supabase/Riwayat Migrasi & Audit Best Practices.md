---
title: "Riwayat Migrasi & Audit Best Practices"
aliases:
  - Riwayat Migrasi
  - Database Advisors
  - Audit Postgres Best Practices
tags:
  - database
  - migrations
  - best-practices
  - audit
  - supabase
layer: database
type: explanation
updated: 2026-11-21
---

# 📋 Riwayat Migrasi & Audit Best Practices (Supabase Advisors)

⬅️ Kembali ke [[MOC - Database Supabase]] | Lihat juga: [[Skema Database & ERD Lengkap]] · [[Keamanan RLS, Trigger, dan Fungsi Database]]

> [!info] Tentang Dokumen Ini
> Dokumen ini mencatat kronologi lengkap migrasi skema di tabel `supabase_migrations.schema_migrations`, mendokumentasikan perubahan skema langsung (*post-migration evolution*), serta menyajikan hasil audit resmi dari **Supabase Security & Performance Advisors** beserta analisis **Supabase Postgres Best Practices**.

---

## 1. Kronologi Migrasi Tercatat (`supabase_migrations.schema_migrations`)

Terdapat **14 versi migrasi** yang tercatat di tabel `supabase_migrations.schema_migrations`:

### Migrasi 1: `20261029030600` — `init_it_asset_management_schema`
- Membuat 5 tabel awal di schema `public`: `app_users`, `assets`, `karyawan`, `alokasi`, dan `riwayat` dengan tipe data teks awalnya menggunakan `text` dan `id bigint generated always as identity primary key`.
- Membuat 4 B-Tree index awal:
  - `alokasi_kode_aset_idx` pada `alokasi(kode_aset)`
  - `alokasi_nik_karyawan_idx` pada `alokasi(nik_karyawan)`
  - `alokasi_status_alokasi_idx` pada `alokasi(status_alokasi)`
  - `riwayat_waktu_idx` pada `riwayat(waktu desc)`
- Mengaktifkan Row-Level Security (`ENABLE ROW LEVEL SECURITY`) pada kelima tabel dan membuat policy awal (`app_users_client_select`, `assets_client_all`, `karyawan_client_all`, `alokasi_client_all`, `riwayat_client_all`).

### Migrasi 2: `20261029030746` — `update_alokasi_fk_cascade`
- Mengubah aturan Foreign Key `alokasi_kode_aset_fkey` dan `alokasi_nik_karyawan_fkey` pada tabel `public.alokasi` dari `ON DELETE RESTRICT` menjadi `ON UPDATE CASCADE ON DELETE CASCADE` agar penghapusan master aset (yang tidak sedang dipinjam) atau master karyawan (yang tidak memiliki pinjaman aktif) tidak terblokir oleh histori alokasi lama yang sudah berstatus `'Dikembalikan'`.

### Migrasi 3: `20261029031901` — `grant_table_privileges_to_anon_and_authenticated`
- Memberikan `GRANT USAGE ON SCHEMA public TO anon, authenticated`.
- Memberikan `GRANT SELECT ON TABLE public.app_users TO anon, authenticated`.
- Memberikan `GRANT SELECT, INSERT, UPDATE, DELETE` pada tabel `assets`, `karyawan`, `alokasi`, dan `riwayat` kepada `anon, authenticated`.
- Memberikan `GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO anon, authenticated`.

### Migrasi 4: `20261115042450` — `ubah_tipe_data_varchar_dan_date`
- Mengubah seluruh kolom bertipe `text` menjadi `varchar(n)` dengan batas panjang eksplisit yang sesuai dengan `MaxLength` pada `TextBox` WinForms:
  - `app_users`: `username varchar(50)`, `password varchar(100)`, `role varchar(10)`
  - `assets`: `kode_aset varchar(20)`, `nama_aset varchar(100)`, `kategori varchar(50)`, `kondisi varchar(20)`, `status varchar(20)`
  - `karyawan`: `nik varchar(20)`, `nama varchar(100)`, `divisi varchar(50)`, `jabatan varchar(100)`, `kontak varchar(50)`
  - `alokasi`: `id_alokasi varchar(20)`, `kode_aset varchar(20)`, `nama_aset varchar(100)`, `nik_karyawan varchar(20)`, `nama_karyawan varchar(100)`, `divisi varchar(50)`, `tanggal_pinjam date DEFAULT current_date`, `keterangan varchar(255)`, `status_alokasi varchar(20)`
  - `riwayat`: `jenis_aksi varchar(20)`, `kode_aset varchar(20)`, `nama_aset varchar(100)`, `nama_karyawan varchar(100)`, `catatan varchar(255)`, `oleh_user varchar(50)`

### Migrasi 5: `20261115074313` — `tambah_kolom_jumlah_dan_tersedia_pada_assets`
- Menambahkan dukungan stok aset multi-unit (*real-world multi-unit asset inventory*) pada tabel `public.assets`:
  - `ALTER TABLE public.assets ADD COLUMN IF NOT EXISTS jumlah integer NOT NULL DEFAULT 1;`
  - `ALTER TABLE public.assets ADD COLUMN IF NOT EXISTS tersedia integer NOT NULL DEFAULT 1;`
- Menambahkan 2 *Check Constraints* penjaga integritas stok di level PostgreSQL:
  - `assets_jumlah_check`: `CHECK (jumlah >= 1)`
  - `assets_tersedia_check`: `CHECK (tersedia >= 0 AND tersedia <= jumlah)`
- Menyinkronkan nilai `tersedia` dan `status` terhadap jumlah pinjaman yang berstatus `'Aktif'` pada tabel `public.alokasi`.

### Migrasi 6: `20261115082628` — `tambah_nama_lengkap_dan_username_case_sensitive`
- Menambahkan kolom `nama_lengkap character varying(100) NOT NULL DEFAULT ''` pada tabel `public.app_users` beserta constraint `app_users_nama_lengkap_check CHECK (char_length(btrim(nama_lengkap)) >= 2)` untuk memisahkan antara `username` login dan **Nama Pengguna** yang tampil di aplikasi.
- Memperbarui fungsi trigger `public.enforce_app_users_security()` agar **tidak lagi memaksa `username` menjadi huruf kecil** (`NEW.username := btrim(NEW.username)` alih-alih `lower(btrim(NEW.username))`), sehingga `username` bersifat **case-sensitive** penuh.

### Migrasi 7: `20261115084721` — `tambah_kolom_jumlah_pada_alokasi`
- Menambahkan kolom `jumlah integer NOT NULL DEFAULT 1` pada tabel `public.alokasi` beserta constraint `alokasi_jumlah_check CHECK (jumlah >= 1)` untuk mendukung transaksi **peminjaman beberapa unit sekaligus (*bulk / multi-quantity borrowing*)** dalam 1 transaksi alokasi.

### Migrasi 8: `20261115091157` — `tambah_kolom_jumlah_pada_riwayat`
- Menambahkan kolom `jumlah integer NOT NULL DEFAULT 1` pada tabel `public.riwayat` beserta constraint `riwayat_jumlah_check CHECK (jumlah >= 1)` agar jumlah unit barang yang dipinjam/dikembalikan/ditambahkan tampil sebagai kolom tersendiri di tabel Riwayat (tidak lagi disambung ke kolom `catatan`).

### Migrasi 9: `20261115101840` — `tambah_stok_perbaikan_dan_rusak_dinamis_pada_assets`
- Menambahkan kolom `perbaikan integer NOT NULL DEFAULT 0` dan `rusak integer NOT NULL DEFAULT 0` pada tabel `public.assets`, memperluas `kondisi` menjadi `varchar(60)`, serta memperbarui constraint `assets_tersedia_check CHECK (tersedia >= 0 AND perbaikan >= 0 AND rusak >= 0 AND (tersedia + perbaikan + rusak) <= jumlah)`.
- Memungkinkan pelacakan kondisi stok per unit secara dinamis (misalnya ketika 2 dari 4 unit dikembalikan dalam kondisi Rusak, hanya 2 unit tersebut yang tercatat di `rusak = 2` dengan `kondisi = 'Baik (2), Rusak (2)'`, sementara 2 unit sisanya tetap `tersedia = 2` dan siap dipinjam).

### Migrasi 10: `20261115103110` — `perlebar_oleh_user_riwayat`
- Memperlebar `riwayat.oleh_user` dari `varchar(50)` menjadi `varchar(100)` agar sama dengan `app_users.nama_lengkap` (`varchar(100)`). Sebelumnya Nama Pengguna lebih dari 50 karakter membuat `INSERT` riwayat ditolak server, sementara aplikasi menelan error tersebut sehingga baris riwayat hilang tanpa pesan.

### Migrasi 11: `20261115113401` — `izinkan_hapus_aset_pada_riwayat`
- Memperbarui *Check Constraint* `riwayat_jenis_aksi_check` pada tabel `public.riwayat` agar mengizinkan nilai `'Hapus Aset'` di samping `'Peminjaman'`, `'Pengembalian'`, dan `'Tambah Aset'`, sehingga jejak audit penghapusan aset oleh Admin selalu tersimpan di PostgreSQL.

### Migrasi 12–14: Relasi `riwayat` ke `app_users` & Keunikan Kontak Karyawan (2026-11-21)
- Menambahkan kolom `username_pelaku varchar(50) NOT NULL` pada `public.riwayat` beserta Foreign Key `riwayat_username_pelaku_fkey` (`FOREIGN KEY (username_pelaku) REFERENCES public.app_users(username) ON UPDATE CASCADE ON DELETE RESTRICT`) dan index `riwayat_username_pelaku_idx`.
- Dengan merujuk langsung ke Primary Key `app_users(username)`, kolom `app_users.nama_lengkap` **tidak perlu `UNIQUE`** (dua pengguna boleh memiliki nama lengkap yang sama), sementara **kelima tabel kini terlibat dalam relasi**.
- Menambahkan trigger `trg_sync_riwayat_pelaku` pada `public.riwayat` dan `trg_sync_app_users_nama_to_riwayat` pada `public.app_users`.
- `karyawan_not_blank_check`: `nama`, `divisi`, `jabatan`, `kontak` tidak boleh kosong/spasi saja.
- Partial Functional `UNIQUE INDEX` `karyawan_kontak_unique_idx` + fungsi `enforce_karyawan_kontak_unique()` & trigger `trg_karyawan_kontak_unique`: kontak karyawan (nomor telepon/email) tidak boleh sama dengan karyawan lain ([[Tabel karyawan]]).
- Alasan keputusan: [[ADR-007 - Relasi Riwayat ke Pengguna dan Kontak Karyawan Unik]].

---

## 2. Evolusi Skema Pasca-Migrasi (*Live Database vs Migration History*)

Sesuai dengan panduan pada skill **Documentation and ADRs** (*Documentation Drift Detection*) dan **Supabase** (*Imperative SQL Execution*), hasil pemeriksaan terhadap skema aktif (*live database*) menunjukkan adanya penyempurnaan keamanan dan struktur kunci utama yang dieksekusi langsung di server setelah migrasi ke-4:

1. **Perubahan Primary Key ke Natural Key pada 4 Tabel**:
   - `public.app_users`: Primary Key berpindah dari `id` ke **`username`** (`app_users_pkey`), dan index duplikat `app_users_username_key` dihapus.
   - `public.assets`: Primary Key berpindah dari `id` ke **`kode_aset`** (`assets_pkey`), dan index duplikat `assets_kode_aset_key` dihapus.
   - `public.karyawan`: Primary Key berpindah dari `id` ke **`nik`** (`karyawan_pkey`), dan index duplikat `karyawan_nik_key` dihapus.
   - `public.alokasi`: Primary Key berpindah dari `id` ke **`id_alokasi`** (`alokasi_pkey`), dan index duplikat `alokasi_id_alokasi_key` dihapus.
   *(Kolom `id bigint generated always as identity` tetap dipertahankan di keempat tabel sebagai nomor urut baris internal, sementara `public.riwayat` tetap menggunakan `id` sebagai Primary Key).*
2. **Penambahan Dukungan Registrasi & Update Profil pada `public.app_users`**:
   - Ditambahkan `GRANT INSERT, UPDATE ON TABLE public.app_users TO anon, authenticated`.
   - Ditambahkan constraint `app_users_username_check` dan `app_users_password_hash_check`.
   - Ditambahkan policy RLS `app_users_client_insert` dan `app_users_client_update`.
   - Ditambahkan fungsi `public.enforce_app_users_security()` dan trigger `trg_enforce_app_users_security`.

---

## 3. Laporan Evaluasi Supabase Advisors & Postgres Best Practices

Hasil pemanggilan alat MCP `get_advisors` (`type: "security"` dan `type: "performance"`) pada proyek `agbmfszugvjwvinxfgiy`:

### A. Security Advisors (`get_advisors(type="security")`)
- **Status Tabel Schema `public`**: **0 Peringatan / 0 Masalah Kritis!** Seluruh tabel di schema `public` telah mengaktifkan RLS, memiliki policy yang valid, dan fungsi trigger telah mengatur `SET search_path TO 'public'`.
- **Satu-satunya Catatan (`WARN` — `auth_leaked_password_protection`)** *(masih sama pada pemeriksaan ulang 2026-11-21)*:
  - *Pesan*: *"Leaked password protection is currently disabled"* pada fitur bawaan Supabase Auth ([Dokumentasi Remediasi](https://supabase.com/docs/guides/auth/password-security#password-strength-and-leaked-password-protection)).
  - *Konteks Aplikasi*: Aplikasi ITAM ini tidak menggunakan modul GoTrue/Supabase Auth bawaan melainkan tabel `public.app_users` kustom dengan hash SHA-256, sehingga peringatan global pada modul Supabase Auth tersebut tidak berdampak pada tabel `public.app_users`.

### B. Performance Advisors (`get_advisors(type="performance")`)
- **Status Kesehatan Query & Primary Key**: **0 Masalah Kritis!** Tidak ada Foreign Key yang kehilangan index (*unindexed foreign keys = 0*), dan tidak ada duplikasi index antara `UNIQUE` constraint dan `PRIMARY KEY`.
- **Catatan Informasi (`INFO` — `unused_index`)** *(diperiksa ulang 2026-11-21 setelah Migrasi 12: 4 temuan)*:
  - Advisor melaporkan bahwa `riwayat_oleh_user_idx` (baru), `alokasi_kode_aset_idx`, `alokasi_nik_karyawan_idx`, dan `alokasi_status_alokasi_idx` tercatat belum pernah digunakan dalam statistik scan (`idx_scan = 0`) sejak statistik terakhir di-reset ([Dokumentasi Linter 0005](https://supabase.com/docs/guides/database/database-linter?lint=0005_unused_index)). `riwayat_oleh_user_idx` dipertahankan karena berada pada kolom Foreign Key.
  - **Analisis Best Practices**: Ketiga index pada tabel `alokasi` tersebut **wajib dipertahankan** karena `alokasi_kode_aset_idx` dan `alokasi_nik_karyawan_idx` adalah index pada kolom **Foreign Key** (`ON UPDATE CASCADE ON DELETE CASCADE`). Dalam PostgreSQL, tanpa index pada kolom Foreign Key di tabel anak (`alokasi`), setiap operasi `UPDATE` atau `DELETE` pada tabel induk (`assets` atau `karyawan`) akan memaksa PostgreSQL melakukan *Sequential Scan* penuh disertai kunci tabel. Karena ukuran tabel masih di bawah satu halaman disk (8 KB), planner PostgreSQL memilih *Seq Scan* yang lebih cepat, namun index tersebut akan otomatis dipakai begitu volume transaksi membesar.
