---
title: "Tabel assets"
aliases:
  - public.assets
  - assets
  - Tabel Aset
tags:
  - database
  - tabel
  - assets
  - supabase
layer: database
type: reference
updated: 2026-11-15
---

# 💻 Tabel `public.assets`

⬅️ Kembali ke [[MOC - Database Supabase]] | [[Skema Database & ERD Lengkap]] | Lihat juga: [[AssetController]] · [[Models.cs (Entitas & Konstanta Domain)]] · [[UC_Asset]]

> [!info] Deskripsi Tabel
> Tabel `public.assets` adalah tabel master yang menyimpan seluruh inventaris perangkat/aset IT perusahaan beserta **manajemen stok multi-unit (`jumlah` & `tersedia`)**. Setiap baris merepresentasikan satu jenis/tipe perangkat dengan kode unik (`AST-001`, `AST-002`, dst.), kategori, total jumlah unit fisik (`jumlah`), sisa unit yang tersedia untuk dipinjam (`tersedia`), kondisi fisik (`kondisi`), serta status ketersediaannya saat ini (`status`).

---

## 1. Struktur Kolom & Tipe Data Presisi

| Posisi | Nama Kolom | Tipe Data SQL (`udt_name`) | Panjang Maks / Presisi | Nullable? | Default / Identity | Pemetaan Properti `AssetItem` |
| :-: | :--- | :--- | :---: | :---: | :--- | :--- |
| 1 | `id` | `bigint` (`int8`) | Precision 64, Scale 0 | `NO` | `GENERATED ALWAYS AS IDENTITY` | *(Tidak dimuat ke model C#)* |
| 2 | **`kode_aset`** | `character varying` (`varchar`) | `20` | `NO` | **PRIMARY KEY** | `AssetItem.KodeAset` (`string`) |
| 3 | `nama_aset` | `character varying` (`varchar`) | `100` | `NO` | - | `AssetItem.NamaAset` (`string`) |
| 4 | `kategori` | `character varying` (`varchar`) | `50` | `NO` | - | `AssetItem.Kategori` (`string`) |
| 5 | `kondisi` | `character varying` (`varchar`) | `60` | `NO` | - | `AssetItem.Kondisi` (`string`, dinamis: `"Baik"`, `"Perbaikan"`, `"Rusak"`, atau rincian seperti `"Baik (2), Rusak (2)"`) |
| 6 | `status` | `character varying` (`varchar`) | `20` | `NO` | - | `AssetItem.Status` (`string`) |
| 7 | **`jumlah`** | `integer` (`int4`) | Precision 32, Scale 0 | `NO` | `DEFAULT 1` | `AssetItem.Jumlah` (`int`, total unit) |
| 8 | **`tersedia`** | `integer` (`int4`) | Precision 32, Scale 0 | `NO` | `DEFAULT 1` | `AssetItem.Tersedia` (`int`, unit **Baik** siap pinjam) |
| 9 | **`perbaikan`** | `integer` (`int4`) | Precision 32, Scale 0 | `NO` | `DEFAULT 0` | `AssetItem.Perbaikan` (`int`, `[Browsable(false)]`) |
| 10 | **`rusak`** | `integer` (`int4`) | Precision 32, Scale 0 | `NO` | `DEFAULT 0` | `AssetItem.Rusak` (`int`, `[Browsable(false)]`) |

> [!tip] Properti Terhitung & Kondisi Dinamis (*Dynamic Per-Unit Condition*) di Model C#
> Dari kolom `jumlah`, `tersedia`, `perbaikan`, dan `rusak`, class [[Models.cs (Entitas & Konstanta Domain)|AssetItem]] otomatis menghitung:
> - `Dipinjam` (`[Browsable(false)]`): `Math.Max(0, Jumlah - Tersedia - Perbaikan - Rusak)` — jumlah unit yang sedang dipinjam aktif saat ini.
> - `Bermasalah` (`[Browsable(false)]`): `Perbaikan + Rusak` — jumlah unit yang sedang rusak atau dalam perbaikan.
> - `FormatKondisi(jumlah, perbaikan, rusak)`: Menyusun teks `Kondisi` secara dinamis. Jika seluruh unit berkondisi sama, menghasilkan `"Baik"`, `"Perbaikan"`, atau `"Rusak"`. Jika sebagian unit rusak/perbaikan (misalnya dari 4 unit, 2 dikembalikan rusak), menghasilkan **`"Baik (2), Rusak (2)"`** sementara `Tersedia` tetap `2` sehingga 2 unit yang masih baik tetap dapat dipinjam!
> - `IsBorrowed` (`[Browsable(false)]`): `Dipinjam > 0 || Status == AssetStatus.Dipinjam`.
> - `IsReadyToLend` (`[Browsable(false)]`): `Tersedia > 0 && Status == AssetStatus.Tersedia && Kondisi != AssetCondition.Perbaikan && Kondisi != AssetCondition.Rusak`.

---

## 2. Daftar Constraints & Relasi Foreign Key

### A. Constraints pada `public.assets`
| Nama Constraint | Tipe | Definisi SQL (`pg_get_constraintdef`) | Penjelasan |
| :--- | :---: | :--- | :--- |
| `assets_pkey` | `PRIMARY KEY` | `PRIMARY KEY (kode_aset)` | Menjadikan `kode_aset` (misal `AST-001`) sebagai kunci utama unik. |
| `assets_jumlah_check` | `CHECK` | `CHECK (jumlah >= 1)` | Menjamin total jumlah unit setiap jenis aset minimal `1` unit. |
| `assets_tersedia_check` | `CHECK` | `CHECK (tersedia >= 0 AND perbaikan >= 0 AND rusak >= 0 AND (tersedia + perbaikan + rusak) <= jumlah)` | Menjamin rincian unit (`tersedia` + `perbaikan` + `rusak`) tidak pernah negatif dan tidak melebihi total `jumlah` (selisihnya adalah unit yang sedang `Dipinjam`). |
| `assets_kondisi_check` | `CHECK` | `CHECK (char_length(btrim(kondisi)) >= 4)` | Menjamin teks kondisi terisi (`"Baik"`, `"Perbaikan"`, `"Rusak"`, maupun rincian dinamis seperti `"Baik (2), Rusak (2)"`). |
| `assets_status_check` | `CHECK` | `CHECK (status::text = ANY (ARRAY['Tersedia'::text, 'Dipinjam'::text]))` | Menjamin status ketersediaan aset bernilai `'Tersedia'` atau `'Dipinjam'`. |

### B. Relasi Masuk (*Incoming Foreign Key*)
- **`alokasi_kode_aset_fkey`** dari [[Tabel alokasi|public.alokasi]]:
  ```sql
  FOREIGN KEY (kode_aset) REFERENCES public.assets(kode_aset)
  ON UPDATE CASCADE ON DELETE CASCADE
  ```
  - **Kardinalitas**: `1` baris di `assets` berelasi dengan `0..N` baris di `alokasi` (dan dapat memiliki hingga sejumlah `jumlah` transaksi pinjaman berstatus `'Aktif'` secara bersamaan!).
  - **Proteksi Bisnis di Aplikasi**:
    1. Sebelum mengirim `DELETE` ke `public.assets`, [[AssetController]] memeriksa `if (asset.IsBorrowed || DataStore.GetAlokasi().Any(a => a.KodeAset == asset.KodeAset && a.IsActive))` ([AssetController.cs#L199-L202](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L199-L202)). Jika masih ada minimal 1 unit yang sedang dipinjam (`Dipinjam > 0` atau alokasi `Aktif`), penghapusan ditolak!
    2. Saat mengubah `Jumlah` di [[AssetController|AssetController.Update()]] ([AssetController.cs#L143-L147](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L143-L147)), sistem menolak pengurangan `jumlah` di bawah jumlah unit yang sedang dipinjam aktif (`jumlah < asset.Dipinjam`), serta memvalidasi total `perbaikan + rusak <= jumlah - dipinjam` melalui `ResolveStokKondisi()`.

---

## 3. Index & Kebijakan RLS

- **Index**: `CREATE UNIQUE INDEX assets_pkey ON public.assets USING btree (kode_aset)`
  Mendukung pengurutan `order=kode_aset.asc` pada `DataStore.GetAssets()` serta filter `kode_aset=eq.{kode}` pada saat `Update` dan `Delete`.
- **RLS Policy**: `assets_client_all` (`FOR ALL TO anon, authenticated USING (true) WITH CHECK (true)`).

---

## 4. Pemetaan ke Kode C# & Endpoint PostgREST

| Operasi | Fungsi di C# | HTTP Method & Endpoint PostgREST |
| :--- | :--- | :--- |
| Muat semua aset | [[DataStore.cs (REST Client & In-Memory Cache)\|DataStore.GetAssets()]] ([L80-L114](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L80-L114)) | `GET /rest/v1/assets?select=kode_aset,nama_aset,kategori,jumlah,tersedia,perbaikan,rusak,kondisi,status&order=kode_aset.asc` |
| Tambah aset multi-unit baru | [[AssetController\|AssetController.Add()]] ([L84-L134](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L84-L134)) | `POST /rest/v1/assets` (`{ kode_aset, nama_aset, kategori, jumlah, tersedia, perbaikan, rusak, kondisi, status }`) |
| Ubah data & kondisi sebagian unit | [[AssetController\|AssetController.Update()]] ([L137-L193](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L137-L193)) | `PATCH /rest/v1/assets?kode_aset=eq.{kode_aset}` (`{ nama_aset, kategori, jumlah, tersedia, perbaikan, rusak, kondisi, status }`) |
| Kurangi stok saat Dipinjam (`-jumlahPinjam`) | [[AlokasiController\|AlokasiController.Allocate()]] ([L30-L98](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L30-L98)) | `PATCH /rest/v1/assets?kode_aset=eq.{kode_aset}` (`{ tersedia, perbaikan, rusak, kondisi, status }`) |
| Tambah/perbarui stok kondisi saat Kembali | [[AlokasiController\|AlokasiController.Return()]] ([L102-L168](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L102-L168)) | `PATCH /rest/v1/assets?kode_aset=eq.{kode_aset}` (`{ tersedia, perbaikan, rusak, kondisi, status }`) |
| Hapus aset (disertai log `Hapus Aset`) | [[AssetController\|AssetController.Delete()]] ([L196-L224](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L196-L224)) | `DELETE /rest/v1/assets?kode_aset=eq.{kode_aset}` |
