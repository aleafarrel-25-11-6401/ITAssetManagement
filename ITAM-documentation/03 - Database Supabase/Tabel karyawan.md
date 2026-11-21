---
title: "Tabel karyawan"
aliases:
  - public.karyawan
  - karyawan
  - Tabel Karyawan
tags:
  - database
  - tabel
  - karyawan
  - supabase
layer: database
type: reference
updated: 2026-11-21
---

# 🧑‍💼 Tabel `public.karyawan`

⬅️ Kembali ke [[MOC - Database Supabase]] | [[Skema Database & ERD Lengkap]] | Lihat juga: [[KaryawanController]] · [[Models.cs (Entitas & Konstanta Domain)]] · [[UC_Karyawan]]

> [!info] Deskripsi Tabel
> Tabel `public.karyawan` menyimpan data master seluruh pegawai/karyawan perusahaan yang berhak menerima alokasi peminjaman perangkat IT. Setiap karyawan diidentifikasi oleh nomor induk (`nik`) berformat `EMP-001`, `EMP-002`, dst.

---

## 1. Struktur Kolom & Tipe Data Presisi

| Posisi | Nama Kolom | Tipe Data SQL (`udt_name`) | Panjang Maks | Nullable? | Default / Identity | Pemetaan Properti `KaryawanItem` |
| :-: | :--- | :--- | :---: | :---: | :--- | :--- |
| 1 | `id` | `bigint` (`int8`) | Precision 64, Scale 0 | `NO` | `GENERATED ALWAYS AS IDENTITY` | *(Tidak dimuat ke model C#)* |
| 2 | **`nik`** | `character varying` (`varchar`) | `20` | `NO` | **PRIMARY KEY** | `KaryawanItem.Nik` (`string`) |
| 3 | `nama` | `character varying` (`varchar`) | `100` | `NO` | - | `KaryawanItem.Nama` (`string`) |
| 4 | `divisi` | `character varying` (`varchar`) | `50` | `NO` | - | `KaryawanItem.Divisi` (`string`) |
| 5 | `jabatan` | `character varying` (`varchar`) | `100` | `NO` | - | `KaryawanItem.Jabatan` (`string`) |
| 6 | `kontak` | `character varying` (`varchar`) | `50` | `NO` | - | `KaryawanItem.Kontak` (`string`) |

> [!note] Nilai Default di Level Aplikasi (`KaryawanController.IsiDefault`)
> Karena seluruh kolom di tabel `public.karyawan` bersifat `NOT NULL`, [[KaryawanController]] menggunakan fungsi pembantu `IsiDefault()` ([KaryawanController.cs#L152-L153](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L152-L153)) untuk mengisi nilai bawaan apabila pengguna mengosongkan kolom opsional pada UI:
> - `divisi` $\rightarrow$ `"Umum"` (di UI [[UC_Karyawan]], `cmbDivisi` memiliki pilihan: `"Software Dev"`, `"UI/UX Design"`, `"IT Support"`, `"Finance"`, `"HRD"`, `"Marketing"`).
> - `jabatan` $\rightarrow$ `"Staff"`
> - `kontak` $\rightarrow$ `"-"`

---

## 2. Constraints, Relasi Foreign Key, & Index

### A. Primary Key, Check Constraint, & Unique Index
- **Constraint `karyawan_pkey`**: `PRIMARY KEY (nik)`
- **Constraint `karyawan_not_blank_check`** *(baru, 2026-11-21)*: `CHECK (btrim(nama) <> '' AND btrim(divisi) <> '' AND btrim(jabatan) <> '' AND btrim(kontak) <> '')`. Menolak nilai kosong atau berisi spasi saja; aplikasi sudah mengisi default (`Umum`, `Staff`, `-`) sehingga alurnya tidak berubah.
- **Index `karyawan_pkey`**: `CREATE UNIQUE INDEX karyawan_pkey ON public.karyawan USING btree (nik)`
- **Unique Index `karyawan_kontak_unique_idx`** *(baru, 2026-11-21)*:
  ```sql
  CREATE UNIQUE INDEX karyawan_kontak_unique_idx
  ON public.karyawan (lower(regexp_replace(kontak, '[\s().\-]', '', 'g')))
  WHERE lower(regexp_replace(kontak, '[\s().\-]', '', 'g')) <> '';
  ```
  Menjamin di tingkat mesin B-Tree PostgreSQL bahwa tidak ada dua karyawan yang memiliki nomor telepon atau email yang sama (setelah dinormalisasi, dengan mengecualikan `'-'`).

### A2. Kontak Tidak Boleh Sama (`karyawan_kontak_unique_idx` & Trigger `trg_karyawan_kontak_unique`) *(baru, 2026-11-21)*
```sql
CREATE TRIGGER trg_karyawan_kontak_unique
BEFORE INSERT OR UPDATE OF kontak ON public.karyawan
FOR EACH ROW EXECUTE FUNCTION public.enforce_karyawan_kontak_unique();
```
- **Aturan**: nilai `kontak` (nomor telepon atau email) tidak boleh sama dengan milik karyawan lain. Pembandingan memakai `lower(regexp_replace(kontak, '[\s().\-]', '', 'g'))`, jadi spasi, titik, tanda kurung, strip, dan huruf besar/kecil diabaikan.
- **Pengecualian**: kontak kosong (`-`) boleh dipakai banyak karyawan.
- **Perlindungan Ganda**:
  1. **Di Aplikasi C# (`KaryawanController.Add` & `Update`)**: aplikasi langsung menghadang dan menampilkan pesan dialog peringatan:
     > `"Nomor telepon/email kontak sudah digunakan karyawan lain!"`
     sehingga data tidak terkirim dan tidak tersimpan ke server.
  2. **Di Database PostgreSQL**: dijaga oleh **Partial Functional Unique Index `karyawan_kontak_unique_idx`** sekaligus **Trigger `trg_karyawan_kontak_unique`** (error PostgreSQL `23505` `unique_violation`).

### B. Relasi Masuk (*Incoming Foreign Key*)
- **`alokasi_nik_karyawan_fkey`** dari [[Tabel alokasi|public.alokasi]]:
  ```sql
  FOREIGN KEY (nik_karyawan) REFERENCES public.karyawan(nik)
  ON UPDATE CASCADE ON DELETE CASCADE
  ```
  - **Kardinalitas**: `1` karyawan dapat memiliki `0..N` catatan transaksi alokasi.
  - **Aturan Bisnis Penghapusan**: Di dalam `KaryawanController.Delete()` ([KaryawanController.cs#L124-L125](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L124-L125)), aplikasi mengecek:
    ```csharp
    if (DataStore.GetAlokasi().Any(a => a.NikKaryawan == karyawan.Nik && a.IsActive))
        return OperationResult.Fail("Karyawan masih memiliki pinjaman aset aktif!");
    ```
    Sehingga karyawan yang masih membawa aset perusahaan tidak bisa dihapus sebelum mengembalikan asetnya.

### C. Kebijakan RLS
- **RLS Policy**: `karyawan_client_all` (`FOR ALL TO anon, authenticated USING (true) WITH CHECK (true)`).

---

## 3. Pemetaan ke Kode C# & Endpoint PostgREST

| Operasi | Fungsi di C# | HTTP Method & Endpoint PostgREST |
| :--- | :--- | :--- |
| Muat semua karyawan | [[DataStore.cs (REST Client & In-Memory Cache)\|DataStore.GetKaryawan()]] ([L117-L135](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L117-L135)) | `GET /rest/v1/karyawan?select=nik,nama,divisi,jabatan,kontak&order=nik.asc` |
| Tambah karyawan baru | [[KaryawanController\|KaryawanController.Add()]] ([L44-L51](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L44-L51)) | `POST /rest/v1/karyawan` |
| Ubah data karyawan | [[KaryawanController\|KaryawanController.Update()]] ([L81-L87](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L81-L87)) | `PATCH /rest/v1/karyawan?nik=eq.{nik}` (diikuti sinkronisasi ke `alokasi?nik_karyawan=eq.{nik}` jika `nama` atau `divisi` berubah) |
| Hapus karyawan | [[KaryawanController\|KaryawanController.Delete()]] ([L127](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L127)) | `DELETE /rest/v1/karyawan?nik=eq.{nik}` |
