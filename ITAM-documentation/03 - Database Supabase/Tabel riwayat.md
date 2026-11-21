---
title: "Tabel riwayat"
aliases:
  - public.riwayat
  - riwayat
  - Tabel Riwayat
tags:
  - database
  - tabel
  - riwayat
  - audit-log
  - supabase
layer: database
type: reference
updated: 2026-11-21
---

# 📜 Tabel `public.riwayat`

⬅️ Kembali ke [[MOC - Database Supabase]] | [[Skema Database & ERD Lengkap]] | Lihat juga: [[RiwayatController]] · [[UC_Riwayat]]

> [!info] Deskripsi Tabel
> Tabel `public.riwayat` menyimpan jejak audit (*audit trail*) kronologis untuk empat jenis aktivitas utama di dalam aplikasi: **`Tambah Aset`**, **`Hapus Aset`**, **`Peminjaman`**, dan **`Pengembalian`**. Tabel ini dirancang sebagai catatan historis mandiri (*snapshot*) tanpa Foreign Key ke tabel master `assets` dan `karyawan` agar jejak histori tetap utuh meskipun aset atau karyawan di kemudian hari dihapus. Satu-satunya Foreign Key adalah `username_pelaku` ke [[Tabel app_users]] (sejak 2026-11-21), yang menjamin setiap catatan merujuk ke akun pengguna yang sah.

---

## 1. Struktur Kolom & Tipe Data Presisi

| Posisi | Nama Kolom | Tipe Data SQL (`udt_name`) | Panjang Maks | Nullable? | Default / Identity | Pemetaan Properti `RiwayatItem` |
| :-: | :--- | :--- | :---: | :---: | :--- | :--- |
| 1 | **`id`** | `bigint` (`int8`) | Precision 64, Scale 0 | `NO` | **PRIMARY KEY**, `GENERATED ALWAYS AS IDENTITY` | *(Tidak dimuat ke model C#)* |
| 2 | `waktu` | `timestamp with time zone` (`timestamptz`) | - | `NO` | `now()` | `RiwayatItem.Waktu` (`DateTime`) |
| 3 | `jenis_aksi` | `character varying` (`varchar`) | `20` | `NO` | - | `RiwayatItem.JenisAksi` (`string`) |
| 4 | `kode_aset` | `character varying` (`varchar`) | `20` | `NO` | - | `RiwayatItem.KodeAset` (`string`) |
| 5 | `nama_aset` | `character varying` (`varchar`) | `100` | `NO` | - | `RiwayatItem.NamaAset` (`string`) |
| 6 | **`jumlah`** | `integer` (`int4`) | Precision 32, Scale 0 | `NO` | `DEFAULT 1`, `CHECK (jumlah >= 1)` | `RiwayatItem.Jumlah` (`int`) |
| 7 | `nama_karyawan` | `character varying` (`varchar`) | `100` | `NO` | - | `RiwayatItem.NamaKaryawan` (`string`) |
| 8 | `catatan` | `character varying` (`varchar`) | `255` | `NO` | - | `RiwayatItem.Catatan` (`string`) |
| 9 | `oleh_user` | `character varying` (`varchar`) | `100` | `NO` | - | `RiwayatItem.OlehUser` (`string`, nama tampilan pelaku) |
| 10 | **`username_pelaku`** | `character varying` (`varchar`) | `50` | `NO` | **FOREIGN KEY** → `app_users.username` (diisi otomatis oleh trigger `trg_sync_riwayat_pelaku`) | *(Tidak dimuat ke `DataGridView` agar tampilan tabel tetap 8 kolom)* |

---

## 2. Daftar Constraints & Index pada `public.riwayat`

### A. Constraints (`pg_constraint`)
| Nama Constraint | Tipe | Definisi SQL (`pg_get_constraintdef`) | Penjelasan |
| :--- | :---: | :--- | :--- |
| `riwayat_pkey` | `PRIMARY KEY` | `PRIMARY KEY (id)` | Kunci utama berbasis `bigint identity`. |
| `riwayat_jumlah_check` | `CHECK` | `CHECK (jumlah >= 1)` | Memastikan jumlah unit barang yang dicatat di riwayat minimal `1`. |
| `riwayat_jenis_aksi_check` | `CHECK` | `CHECK (jenis_aksi::text = ANY (ARRAY['Peminjaman'::text, 'Pengembalian'::text, 'Tambah Aset'::text, 'Hapus Aset'::text]))` | Memastikan kolom `jenis_aksi` hanya menerima 4 nilai konstanta `RiwayatAction`. |
| `riwayat_username_pelaku_fkey` *(baru, 2026-11-21)* | `FOREIGN KEY` | `FOREIGN KEY (username_pelaku) REFERENCES app_users(username) ON UPDATE CASCADE ON DELETE RESTRICT` | Setiap catatan riwayat merujuk langsung ke Primary Key `app_users(username)` (sehingga `app_users.nama_lengkap` tidak perlu `UNIQUE`). Mengganti `username` otomatis memperbarui `username_pelaku` (`ON UPDATE CASCADE`), dan trigger `trg_sync_app_users_nama_to_riwayat` memperbarui `oleh_user` saat `nama_lengkap` berubah. Lihat [[ADR-007 - Relasi Riwayat ke Pengguna dan Kontak Karyawan Unik]]. |

> [!info] Pengisian Otomatis `username_pelaku` oleh Trigger `trg_sync_riwayat_pelaku`
> Ketika `RiwayatController.Add` mengirim `INSERT INTO public.riwayat` dengan kolom `oleh_user` (berisi `UserSession.DisplayName` atau `username`), trigger `BEFORE INSERT OR UPDATE ON public.riwayat` (`sync_riwayat_pelaku()`) otomatis mencari akun yang cocok di `public.app_users` dan mengisi `NEW.username_pelaku := u.username`. Dengan demikian, tampilan tabel Riwayat di aplikasi tetap menampilkan nama lengkap (`OlehUser`) dan `app_users.nama_lengkap` bebas sama antar akun.

### B. Indexes (`pg_indexes`)
1. `riwayat_pkey`: `CREATE UNIQUE INDEX riwayat_pkey ON public.riwayat USING btree (id)`
2. `riwayat_waktu_idx`: `CREATE INDEX riwayat_waktu_idx ON public.riwayat USING btree (waktu DESC)`
   Index *descending* pada kolom `waktu` ini mengoptimalkan query `DataStore.GetRiwayat()` yang selalu meminta urutan terbaru di atas (`order=waktu.desc`) tanpa memerlukan *filesort* di memori PostgreSQL!
3. `riwayat_username_pelaku_idx` *(baru, 2026-11-21)*: `CREATE INDEX riwayat_username_pelaku_idx ON public.riwayat USING btree (username_pelaku)`
   Index pada kolom Foreign Key `username_pelaku` (praktik baik Postgres) agar `ON UPDATE CASCADE` saat `username` diganti dan sinkronisasi `oleh_user` tidak memindai seluruh tabel.

---

## 3. Format Data per Jenis Aksi

Ketika [[RiwayatController]] (`Add`) dipanggil oleh controller lain, berikut adalah pola nilai yang disimpan ke dalam `public.riwayat`:

| Sumber Pemanggil | `jenis_aksi` | `kode_aset` | `nama_aset` | `jumlah` | `nama_karyawan` | Format `catatan` | `oleh_user` |
| :--- | :--- | :--- | :--- | :-: | :--- | :--- | :--- |
| `AssetController.Add` ([L127](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L127)) | `"Tambah Aset"` | `"AST-xxx"` | Nama aset baru | `jumlah` unit baru | `"-"` | `"Kategori: {kategori}, Kondisi: {teksKondisi}"` | Nama Pengguna aktif |
| `AssetController.Delete` ([L210-L217](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L210-L217)) | `"Hapus Aset"` | `"AST-xxx"` | Nama aset yang dihapus | `asset.Jumlah` unit terakhir | `"-"` | `"Kategori: {kategori}, Kondisi terakhir: {kondisi}"` | Nama Pengguna aktif |
| `AlokasiController.Allocate` ([L91](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L91)) | `"Peminjaman"` | `"AST-xxx"` | Nama aset | `jumlahPinjam` unit | Nama karyawan peminjam | Keterangan keperluan pinjam (murni, tanpa dipotong) | Nama Pengguna aktif |
| `AlokasiController.Return` ([L161](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L161)) | `"Pengembalian"` | `"AST-xxx"` | Nama aset | `jumlahKembali` unit | Nama karyawan pengembali | `"Kondisi kembali: {kondisiKembali}"` atau `"Kondisi kembali: {kondisiKembali} ({unitBermasalah} unit), sisanya Baik"` | Nama Pengguna aktif |

---

## 4. Pemetaan ke Kode C# & Endpoint PostgREST

| Operasi | Fungsi di C# | HTTP Method & Endpoint PostgREST |
| :--- | :--- | :--- |
| Muat semua riwayat (urut terbaru) | [[DataStore.cs (REST Client & In-Memory Cache)\|DataStore.GetRiwayat()]] ([L164-L185](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L164-L185)) | `GET /rest/v1/riwayat?select=waktu,jenis_aksi,kode_aset,nama_aset,jumlah,nama_karyawan,catatan,oleh_user&order=waktu.desc` |
| Catat log aktivitas baru | [[RiwayatController\|RiwayatController.Add()]] ([L12-L47](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/RiwayatController.cs#L12-L47)) | `POST /rest/v1/riwayat` (`waktu` dikirim dalam UTC ISO-8601 `yyyy-MM-ddTHH:mm:ssZ`) |
| Perbarui `oleh_user` saat ganti username/nama | [[DataStore.cs (REST Client & In-Memory Cache)\|DataStore.RenameHistoryUser()]] ([L203-L216](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L203-L216)) | `PATCH /rest/v1/riwayat?oleh_user=eq.{oldName}` (`{ oleh_user = newName }`) |
| Bersihkan seluruh riwayat (Khusus Admin) | [[RiwayatController\|RiwayatController.ClearAll()]] ([L68-L86](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/RiwayatController.cs#L68-L86)) | `DELETE /rest/v1/riwayat?waktu=not.is.null` (PostgREST mewajibkan klausa `WHERE` pada `DELETE`; karena `waktu` adalah `NOT NULL`, filter `waktu=not.is.null` menghapus seluruh baris dengan aman). |
