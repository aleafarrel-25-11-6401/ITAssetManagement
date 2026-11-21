---
title: "Tabel alokasi"
aliases:
  - public.alokasi
  - alokasi
  - Tabel Alokasi
tags:
  - database
  - tabel
  - alokasi
  - supabase
layer: database
type: reference
updated: 2026-11-15
---

# 🤝 Tabel `public.alokasi`

⬅️ Kembali ke [[MOC - Database Supabase]] | [[Skema Database & ERD Lengkap]] | Lihat juga: [[AlokasiController]] · [[UC_Alokasi]] · [[Alur Peminjaman (Alokasi) & Pengembalian Aset]]

> [!info] Deskripsi Tabel
> Tabel `public.alokasi` adalah tabel transaksional inti yang menghubungkan [[Tabel assets|public.assets]] dan [[Tabel karyawan|public.karyawan]]. Setiap baris merepresentasikan satu transaksi peminjaman aset (`ALK-001`, `ALK-002`, dst.) mulai dari saat aset diserahkan kepada karyawan (`status_alokasi = 'Aktif'`) hingga aset tersebut dikembalikan (`status_alokasi = 'Dikembalikan'`).

---

## 1. Struktur Kolom & Tipe Data Presisi

| Posisi | Nama Kolom | Tipe Data SQL (`udt_name`) | Panjang Maks | Nullable? | Default / Identity | Pemetaan Properti `AlokasiItem` |
| :-: | :--- | :--- | :---: | :---: | :--- | :--- |
| 1 | `id` | `bigint` (`int8`) | Precision 64, Scale 0 | `NO` | `GENERATED ALWAYS AS IDENTITY` | *(Tidak dimuat ke model C#)* |
| 2 | **`id_alokasi`** | `character varying` (`varchar`) | `20` | `NO` | **PRIMARY KEY** | `AlokasiItem.IdAlokasi` (`string`) |
| 3 | **`kode_aset`** | `character varying` (`varchar`) | `20` | `NO` | **FOREIGN KEY** $\rightarrow$ `assets(kode_aset)` | `AlokasiItem.KodeAset` (`string`) |
| 4 | `nama_aset` | `character varying` (`varchar`) | `100` | `NO` | - | `AlokasiItem.NamaAset` (`string`) |
| 5 | **`jumlah`** | `integer` (`int4`) | Precision 32, Scale 0 | `NO` | `DEFAULT 1`, `CHECK (jumlah >= 1)` | `AlokasiItem.Jumlah` (`int`) |
| 6 | **`nik_karyawan`** | `character varying` (`varchar`) | `20` | `NO` | **FOREIGN KEY** $\rightarrow$ `karyawan(nik)` | `AlokasiItem.NikKaryawan` (`string`) |
| 7 | `nama_karyawan` | `character varying` (`varchar`) | `100` | `NO` | - | `AlokasiItem.NamaKaryawan` (`string`) |
| 8 | `divisi` | `character varying` (`varchar`) | `50` | `NO` | - | `AlokasiItem.Divisi` (`string`) |
| 9 | `tanggal_pinjam` | `date` (`date`) | - | `NO` | `CURRENT_DATE` | `AlokasiItem.TanggalPinjam` (`DateTime`) |
| 10 | `keterangan` | `character varying` (`varchar`) | `255` | `NO` | - | `AlokasiItem.Keterangan` (`string`) |
| 11 | `status_alokasi` | `character varying` (`varchar`) | `20` | `NO` | - | `AlokasiItem.StatusAlokasi` (`string`, `[Browsable(false)]`) |

> [!tip] Evolusi Tipe Data `tanggal_pinjam` (`timestamptz` $\rightarrow$ `date`)
> Pada migrasi keempat (`20261115042450_ubah_tipe_data_varchar_dan_date`), tipe kolom `tanggal_pinjam` diubah dari `timestamptz` menjadi `date` (`DEFAULT CURRENT_DATE`) karena pencatatan tanggal pinjam hanya membutuhkan presisi kalender harian (`yyyy-MM-dd`), sedangkan log presisi jam/menit disimpan di [[Tabel riwayat|public.riwayat.waktu]]. Di UI `DataGridView`, [[UIHelper & Dialogs|UIHelper.BindGrid]] otomatis memformat kolom bertipe `DateTime` selain `"Waktu"` sebagai `"dd/MM/yyyy"`.

---

## 2. Daftar Constraints & Index pada `public.alokasi`

### A. Constraints (`pg_constraint`)
| Nama Constraint | Tipe | Definisi SQL (`pg_get_constraintdef`) |
| :--- | :---: | :--- |
| `alokasi_pkey` | `PRIMARY KEY` | `PRIMARY KEY (id_alokasi)` |
| `alokasi_kode_aset_fkey` | `FOREIGN KEY` | `FOREIGN KEY (kode_aset) REFERENCES assets(kode_aset) ON UPDATE CASCADE ON DELETE CASCADE` |
| `alokasi_nik_karyawan_fkey` | `FOREIGN KEY` | `FOREIGN KEY (nik_karyawan) REFERENCES karyawan(nik) ON UPDATE CASCADE ON DELETE CASCADE` |
| `alokasi_jumlah_check` | `CHECK` | `CHECK (jumlah >= 1)` |
| `alokasi_status_alokasi_check` | `CHECK` | `CHECK (status_alokasi::text = ANY (ARRAY['Aktif'::text, 'Dikembalikan'::text]))` |

### B. Indexes (`pg_indexes`)
Sesuai aturan **Supabase Postgres Best Practices** (`schema-foreign-key-indexes`), seluruh kolom Foreign Key dan kolom filter status pada `public.alokasi` telah dilengkapi dengan B-Tree Index:
1. `alokasi_pkey`: `CREATE UNIQUE INDEX alokasi_pkey ON public.alokasi USING btree (id_alokasi)`
2. `alokasi_kode_aset_idx`: `CREATE INDEX alokasi_kode_aset_idx ON public.alokasi USING btree (kode_aset)` — mempercepat *cascade update/delete* dari tabel `assets` dan sinkronisasi nama aset.
3. `alokasi_nik_karyawan_idx`: `CREATE INDEX alokasi_nik_karyawan_idx ON public.alokasi USING btree (nik_karyawan)` — mempercepat *cascade update/delete* dari tabel `karyawan` dan sinkronisasi nama/divisi karyawan.
4. `alokasi_status_alokasi_idx`: `CREATE INDEX alokasi_status_alokasi_idx ON public.alokasi USING btree (status_alokasi)` — disiapkan untuk query penyaringan berdasarkan status alokasi.

---

## 3. Pemetaan ke Kode C# & Endpoint PostgREST

| Operasi | Fungsi di C# | HTTP Method & Endpoint PostgREST |
| :--- | :--- | :--- |
| Muat semua alokasi | [[DataStore.cs (REST Client & In-Memory Cache)\|DataStore.GetAlokasi()]] ([L138-L161](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L138-L161)) | `GET /rest/v1/alokasi?select=id_alokasi,kode_aset,nama_aset,jumlah,nik_karyawan,nama_karyawan,divisi,tanggal_pinjam,keterangan,status_alokasi&order=id_alokasi.asc` |
| Buat pinjaman baru | [[AlokasiController\|AlokasiController.Allocate()]] ([L30-L98](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L30-L98)) | `POST /rest/v1/alokasi` |
| Proses pengembalian | [[AlokasiController\|AlokasiController.Return()]] ([L102-L168](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L102-L168)) | `PATCH /rest/v1/alokasi?id_alokasi=eq.{id_alokasi}` (`{ status_alokasi = "Dikembalikan" }`) |
| Sinkronisasi `nama_aset` | [[AssetController\|AssetController.Update()]] ([L175](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L175)) | `PATCH /rest/v1/alokasi?kode_aset=eq.{kode_aset}` (`{ nama_aset = nama }`) |
| Sinkronisasi `nama_karyawan` & `divisi` | [[KaryawanController\|KaryawanController.Update()]] ([L92-L96](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L92-L96)) | `PATCH /rest/v1/alokasi?nik_karyawan=eq.{nik}` (`{ nama_karyawan = nama, divisi = divisi }`) |
