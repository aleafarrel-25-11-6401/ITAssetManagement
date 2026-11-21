---
title: "Models.cs (Entitas & Konstanta Domain)"
aliases:
  - Models.cs
  - AssetItem
  - KaryawanItem
  - AlokasiItem
  - RiwayatItem
tags:
  - models
  - entitas
  - domain
layer: models
type: reference
updated: 2026-11-15
---

# 📦 Models.cs (Entitas & Konstanta Domain)

⬅️ Kembali ke [[MOC - Layer Models]] | [[00 - Home]] | Lihat juga: [[DataStore.cs (REST Client & In-Memory Cache)]] · [[UserSession & OperationResult]]

> [!info] Lokasi File Kode Sumber
> - **File**: [Models/Models.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L1-L246)
> - **Namespace**: `ITAssetManagement.Models`
> - **Dependensi**: `System`, `System.ComponentModel` (`[Browsable(false)]`)

---

## 1. Kelas Konstanta Domain (`static class`)

Untuk mencegah kesalahan ketik (*magic strings*) dan memastikan kesesuaian penuh dengan `CHECK` constraints di PostgreSQL Supabase, `Models.cs` mendefinisikan 4 kelas konstanta statis ([Models.cs#L6-L35](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L6-L35)):

| Nama Class | Konstanta | Nilai String | Constraint Database Terkait |
| :--- | :--- | :--- | :--- |
| **`AssetStatus`** ([L7-L11](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L7-L11)) | `Tersedia`<br>`Dipinjam` | `"Tersedia"`<br>`"Dipinjam"` | `assets_status_check` pada [[Tabel assets]] |
| **`AssetCondition`** ([L14-L19](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L14-L19)) | `Baik`<br>`Perbaikan`<br>`Rusak` | `"Baik"`<br>`"Perbaikan"`<br>`"Rusak"` | `assets_kondisi_check` pada [[Tabel assets]] |
| **`AlokasiStatus`** ([L22-L26](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L22-L26)) | `Aktif`<br>`Dikembalikan` | `"Aktif"`<br>`"Dikembalikan"` | `alokasi_status_alokasi_check` pada [[Tabel alokasi]] |
| **`RiwayatAction`** ([L29-L35](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L29-L35)) | `Peminjaman`<br>`Pengembalian`<br>`TambahAset`<br>`HapusAset` | `"Peminjaman"`<br>`"Pengembalian"`<br>`"Tambah Aset"`<br>`"Hapus Aset"` | `riwayat_jenis_aksi_check` pada [[Tabel riwayat]] |

---

## 2. Kelas Model Entitas Bisnis

### A. `public class AssetItem` ([Models.cs#L38-L123](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L38-L123))
Merepresentasikan satu baris barang aset IT beserta kuota stok multi-unitnya dari [[Tabel assets|public.assets]].

| Member | Tipe | Atribut | Baris | Penjelasan Detail |
| :--- | :--- | :---: | :---: | :--- |
| `KodeAset` | `string` | - | [L40](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L40) | Kode unik aset (contoh: `"AST-001"`). Tampil sebagai kolom **"Kode Aset"** di `DataGridView`. |
| `NamaAset` | `string` | - | [L41](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L41) | Nama/tipe perangkat (contoh: `"Laptop ThinkPad X1 Carbon"`). Tampil sebagai kolom **"Nama Aset"**. |
| `Kategori` | `string` | - | [L42](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L42) | Kategori aset (`"Laptop"`, `"Monitor"`, `"Periferal"`, `"Jaringan"`, `"Lainnya"`). |
| `Jumlah` | `int` | - | [L43](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L43) | Total jumlah unit fisik aset yang dimiliki perusahaan (default `1`, minimal `1`). |
| `Tersedia` | `int` | - | [L44](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L44) | Jumlah unit berkondisi **Baik** yang ada di gudang dan siap dipinjamkan. |
| `Kondisi` | `string` | - | [L45](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L45) | Kondisi fisik **dinamis**: `"Baik"`, `"Perbaikan"`, `"Rusak"`, atau rincian campuran seperti `"Baik (2), Rusak (2)"` (disusun `AssetItem.FormatKondisi`). |
| `Status` | `string` | - | [L46](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L46) | `"Tersedia"` atau `"Dipinjam"` (bila seluruh unit tersisa sedang dipinjam). |
| `Perbaikan` | `int` | `[Browsable(false)]` | [L50](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L50) | Jumlah unit yang sedang dalam perbaikan (kolom `perbaikan`). |
| `Rusak` | `int` | `[Browsable(false)]` | [L54](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L54) | Jumlah unit yang sedang rusak (kolom `rusak`). |
| `Bermasalah` | `int` *(get)* | `[Browsable(false)]` | [L58-L70](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L58-L70) | `Perbaikan + Rusak` (fallback data lama berbasis `Kondisi`). Dipakai kartu "Rusak / Perbaikan" di [[UC_Dashboard]]. |
| `Dipinjam` | `int` *(get)* | `[Browsable(false)]` | [L74-L77](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L74-L77) | `Math.Max(0, Jumlah - Tersedia - Math.Max(0, Perbaikan) - Math.Max(0, Rusak))` — unit yang sedang dipinjam aktif. |
| `IsBorrowed` | `bool` *(get)* | `[Browsable(false)]` | [L81-L84](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L81-L84) | `true` jika `Dipinjam > 0 || Status == Dipinjam`. Mencegah penghapusan di `AssetController.Delete`. |
| `IsReadyToLend` | `bool` *(get)* | `[Browsable(false)]` | [L88-L97](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L88-L97) | `true` jika `Tersedia > 0 && Status == Tersedia && Kondisi != Perbaikan && Kondisi != Rusak`. Kondisi campuran seperti `"Baik (2), Rusak (2)"` tetap boleh dipinjam karena 2 unit masih Baik. |
| `FormatKondisi(jumlah, perbaikan, rusak)` | `static string` | - | [L100-L116](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L100-L116) | Menyusun teks kondisi dinamis dari jumlah unit Baik/Perbaikan/Rusak. |
| `ToString()` | `string` | `override` | [L119-L122](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L119-L122) | `$"{KodeAset} - {NamaAset} (Sisa: {Tersedia})"` untuk dropdown `cmbAset` di [[UC_Alokasi]]. |

---

### B. `public class KaryawanItem` ([Models.cs#L126-L139](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L126-L139))
Merepresentasikan satu baris data karyawan dari [[Tabel karyawan|public.karyawan]].

| Member | Tipe | Baris | Penjelasan Detail |
| :--- | :--- | :---: | :--- |
| `Nik` | `string` | [L128](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L128) | ID unik karyawan (contoh: `"EMP-001"`). |
| `Nama` | `string` | [L129](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L129) | Nama lengkap karyawan. |
| `Divisi` | `string` | [L130](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L130) | Divisi kerja karyawan. |
| `Jabatan` | `string` | [L131](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L131) | Jabatan karyawan. |
| `Kontak` | `string` | [L132](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L132) | Nomor HP atau alamat email karyawan. |
| `ToString()` | `string` | [L135-L138](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L135-L138) | Mengembalikan `Nik + " - " + Nama + " (" + Divisi + ")"` untuk tampilan item pada `cmbKaryawan` di [[UC_Alokasi]]. |

---

### C. `public class AlokasiItem` ([Models.cs#L142-L164](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L142-L164))
Merepresentasikan satu baris transaksi peminjaman aset dari [[Tabel alokasi|public.alokasi]].

| Member | Tipe | Atribut | Baris | Penjelasan Detail |
| :--- | :--- | :---: | :---: | :--- |
| `IdAlokasi` | `string` | - | [L144](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L144) | Kode transaksi alokasi (`"ALK-001"`). |
| `KodeAset` | `string` | - | [L145](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L145) | Kode aset yang dipinjam (`"AST-001"`). |
| `NamaAset` | `string` | - | [L146](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L146) | Nama aset yang dipinjam. |
| `Jumlah` | `int` | - | [L147](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L147) | Jumlah unit aset yang dipinjam dalam transaksi ini (default `1`, minimal `1`, maksimal sebesar `asset.Tersedia` saat dipinjam). |
| `NikKaryawan` | `string` | - | [L148](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L148) | ID karyawan peminjam (`"EMP-001"`). |
| `NamaKaryawan` | `string` | - | [L149](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L149) | Nama karyawan peminjam. |
| `Divisi` | `string` | - | [L150](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L150) | Divisi karyawan peminjam. |
| `TanggalPinjam` | `DateTime` | - | [L151](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L151) | Tanggal peminjaman (diformat `"dd/MM/yyyy"` oleh `UIHelper.BindGrid`). |
| `Keterangan` | `string` | - | [L152](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L152) | Catatan keperluan peminjaman. |
| `StatusAlokasi` | `string` | `[Browsable(false)]` | [L156](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L156) | Status transaksi (`"Aktif"` atau `"Dikembalikan"`). Disembunyikan dari tabel karena tabel di [[UC_Dashboard]] dan [[UC_Alokasi]] khusus menampilkan yang `Aktif`. |
| `IsActive` | `bool` *(get)* | `[Browsable(false)]` | [L160-L163](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L160-L163) | Bernilai `true` jika `StatusAlokasi == AlokasiStatus.Aktif`. |

---

### D. `public class RiwayatItem` ([Models.cs#L167-L177](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L167-L177))
Merepresentasikan satu baris log audit aktivitas beserta jumlah unit barang dari [[Tabel riwayat|public.riwayat]].

| Member | Tipe | Baris | Penjelasan Detail |
| :--- | :--- | :---: | :--- |
| `Waktu` | `DateTime` | [L169](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L169) | Waktu kejadian (diformat `"dd/MM/yyyy HH:mm"` oleh `UIHelper.BindGrid`). |
| `JenisAksi` | `string` | [L170](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L170) | Jenis aktivitas (`"Peminjaman"`, `"Pengembalian"`, `"Tambah Aset"`, `"Hapus Aset"`). Dicetak **tebal** dan menentukan warna baris di `UIHelper.ColorizeRows`. |
| `KodeAset` | `string` | [L171](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L171) | Kode aset yang terlibat. |
| `NamaAset` | `string` | [L172](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L172) | Nama aset yang terlibat. |
| `Jumlah` | `int` | [L173](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L173) | Jumlah unit barang yang dipinjam, dikembalikan, ditambahkan, atau dihapus pada aksi ini (tampil di kolom tersendiri **"Jumlah"** agar tidak terpotong di kolom Catatan). |
| `NamaKaryawan` | `string` | [L174](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L174) | Nama karyawan terkait (atau `"-"` jika aksi adalah `"Tambah Aset"` atau `"Hapus Aset"`). |
| `Catatan` | `string` | [L175](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L175) | Detail catatan aksi (mendukung *multi-line word wrap* & tinggi baris otomatis di `UIHelper.BindGrid`). |
| `OlehUser` | `string` | [L176](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L176) | Nama pengguna (`DisplayName`) yang mengeksekusi aksi. |
