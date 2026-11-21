---
title: "AssetController"
aliases:
  - AssetController.cs
  - Controller Aset
tags:
  - controllers
  - assets
  - crud
layer: controllers
type: reference
updated: 2026-11-15
---

# 💻 AssetController (`Controllers/AssetController.cs`)

⬅️ Kembali ke [[MOC - Layer Controllers]] | [[00 - Home]] | Lihat juga: [[Tabel assets]] · [[UC_Asset]] · [[UC_Dashboard]] · [[UC_Alokasi]]

> [!info] Lokasi File Kode Sumber
> - **File**: [Controllers/AssetController.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L1-L226)
> - **Namespace**: `ITAssetManagement.Controllers`
> - **Class**: `public class AssetController`

---

## 1. Tanggung Jawab Utama

`AssetController` mengelola seluruh logika bisnis untuk data master aset IT perusahaan:
- Menyediakan daftar seluruh aset maupun hasil pencarian kata kunci, termasuk kolom `Kondisi` dan kata kunci `"dipinjam"` (`GetAll`).
- Menyaring daftar aset yang memenuhi syarat untuk dipinjamkan (`GetReadyToLend`).
- Menghasilkan kode aset urut otomatis `AST-001`, `AST-002`, dst. (`NextCode`).
- Memvalidasi dan menghitung distribusi kondisi sebagian unit gudang secara seragam pada tambah & ubah aset tanpa menimpa unit yang sedang dipinjam (`ResolveStokKondisi`).
- Menambah aset baru sekaligus mencatat log aktivitas `"Tambah Aset"` ke [[RiwayatController]] (`Add`).
- Memperbarui data aset & kondisi sebagian unit sekaligus menjaga konsistensi nama aset pada tabel transaksi [[Tabel alokasi|alokasi]] (`Update`).
- Memvalidasi pinjaman aktif, menghapus aset yang tidak sedang dipinjam, serta mencatat log aktivitas `"Hapus Aset"` ke [[RiwayatController]] (`Delete`).

---

## 2. Bedah Detail Setiap Method

### 1. `GetAll(string keyword = "")` ([L14-L25](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L14-L25))
- **Dipanggil oleh**: [[UC_Asset|UC_Asset.LoadGrid]] ([UC_Asset.cs#L38](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Asset.cs#L38)) dan [[UC_Dashboard|UC_Dashboard.OnLoad]] ([UC_Dashboard.cs#L27](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Dashboard.cs#L27)).
- **Cara Kerja**: Membaca `DataStore.GetAssets()` dan melakukan penyaringan *case-insensitive* (`ToLower().Contains(keyword)`) terhadap 5 properti: `KodeAset`, `NamaAset`, `Kategori`, `Kondisi`, dan `Status`, serta mendukung pencarian `keyword == "dipinjam"` via `a.IsBorrowed` (sehingga aset yang sebagian unitnya dipinjam tetap muncul meski kolom `Status`-nya masih `"Tersedia"`).

### 2. `GetReadyToLend()` ([L28](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L28))
- **Dipanggil oleh**: [[UC_Alokasi|UC_Alokasi.RefreshData]] ([UC_Alokasi.cs#L40](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L40)) untuk mengisi pilihan `cmbAset`.
- **Cara Kerja**: `DataStore.GetAssets().Where(a => a.IsReadyToLend).ToList()` — mengembalikan aset dengan `Tersedia > 0 && Status == "Tersedia"` selama tidak seluruh unit berstatus `"Perbaikan"` atau `"Rusak"` (aset kondisi campuran seperti `"Baik (2), Rusak (2)"` tetap siap dipinjamkan untuk 2 unit yang baik).

### 3. `NextCode()` ([L31](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L31))
- **Dipanggil oleh**: [[UC_Asset|UC_Asset.ResetForm]] ([UC_Asset.cs#L137](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Asset.cs#L137)) dan `AssetController.Add` ([L96](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L96)).
- **Cara Kerja**: Memanggil `DataStore.NextCode("AST-", DataStore.GetAssets().Select(a => a.KodeAset))`.

### 4. `ResolveStokKondisi(...)` ([L34-L81](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L34-L81)) *(private static)*
- **Dipanggil oleh**: `Add` dan `Update`.
- **Cara Kerja**: Menghitung unit di gudang (`diGudang = Math.Max(0, jumlah - sedangDipinjam)`). Bila caller mengirim angka `jumlahPerbaikan` atau `jumlahRusak`, memvalidasi keduanya non-negatif dan `perbaikan + rusak <= diGudang`; jika tidak, menerapkan kondisi cepat (`Perbaikan` / `Rusak`) ke `diGudang` atau mempertahankan rincian kondisi lama saat mengedit label campuran. Lalu menetapkan `tersedia = Math.Max(0, diGudang - perbaikan - rusak)`, `teksKondisi = AssetItem.FormatKondisi(...)`, dan `status = (tersedia == 0 && sedangDipinjam > 0) ? "Dipinjam" : "Tersedia"`.

### 5. `Add(string nama, string kategori, string kondisi, string user, int jumlah = 1, int? jumlahPerbaikan = null, int? jumlahRusak = null)` ([L84-L134](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L84-L134))
- **Dipanggil oleh**: [[UC_Asset|UC_Asset.btnTambah_Click]] ([UC_Asset.cs#L182-L189](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Asset.cs#L182-L189)).
- **Alur Eksekusi**:
  1. Validasi: Jika `nama` kosong, kembalikan `OperationResult.Fail("Nama Aset wajib diisi!")`. Jika `jumlah < 1`, kembalikan `OperationResult.Fail("Jumlah unit aset minimal 1!")`.
  2. Hasilkan `kode = NextCode()`, atur nilai default aman (`kategori = "Lainnya"`, `kondisi = "Baik"`, `user = "admin"`), dan hitung `tersedia`, `perbaikan`, `rusak`, `teksKondisi`, serta `status` lewat `ResolveStokKondisi()`.
  3. Kirim `POST /rest/v1/assets` (termasuk `perbaikan` & `rusak`) via `DataStore.Insert("assets", ...)`.
  4. Tambahkan objek `AssetItem` baru ke `DataStore.GetAssets()` (cache memori).
  5. Panggil `_riwayat.Add(RiwayatAction.TambahAset, kode, nama, "-", $"Kategori: {kategori}, Kondisi: {teksKondisi}", user, jumlah)`.

### 6. `Update(AssetItem asset, string nama, string kategori, string kondisi, int jumlah = 1, int? jumlahPerbaikan = null, int? jumlahRusak = null)` ([L137-L193](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L137-L193))
- **Dipanggil oleh**: [[UC_Asset|UC_Asset.btnUbah_Click]] ([UC_Asset.cs#L208-L215](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Asset.cs#L208-L215)).
- **Alur Eksekusi, Validasi Kuota Stok & Sinkronisasi Denormalisasi**:
  1. Validasi `asset != null`, `nama` tidak kosong, dan `jumlah >= 1`.
  2. **Proteksi Unit Sedang Dipinjam**: Menghitung `sedangDipinjam = asset.Dipinjam`. Jika `jumlah < sedangDipinjam`, perubahan ditolak dengan pesan `"Jumlah unit tidak boleh kurang dari {sedangDipinjam} unit yang sedang dipinjam!"`.
  3. **Kondisi Dinamis Parsial**: Memanggil `ResolveStokKondisi()` sehingga Admin dapat menandai sebagian unit di gudang sebagai `Baik`, `Perbaikan`, maupun `Rusak` sekaligus tanpa mengganggu unit yang sedang dipinjam.
  4. Kirim `PATCH /rest/v1/assets?kode_aset=eq.{kode_aset}` dengan `nama_aset`, `kategori`, `jumlah`, `tersedia`, `perbaikan`, `rusak`, `kondisi`, dan `status` baru.
  5. **Sinkronisasi ke Tabel `alokasi`**: Jika `asset.NamaAset != nama` dan terdapat transaksi di `DataStore.GetAlokasi()` yang memiliki `KodeAset == asset.KodeAset`, maka otomatis mengirim `PATCH /rest/v1/alokasi?kode_aset=eq.{kode_aset}` (`{ nama_aset = nama }`) dan memperbarui properti `NamaAset` di cache `_alokasiCache`!
  6. Perbarui properti objek `asset` di `_assetsCache`.

### 7. `Delete(AssetItem asset, string user = null)` ([L196-L224](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L196-L224))
- **Dipanggil oleh**: [[UC_Asset|UC_Asset.btnHapus_Click]] ([UC_Asset.cs#L237](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Asset.cs#L237)).
- **Aturan Proteksi Bisnis & Jejak Audit**:
  - Jika `asset.IsBorrowed == true` **atau** masih terdapat transaksi di `DataStore.GetAlokasi()` dengan `KodeAset == asset.KodeAset && a.IsActive`, penghapusan ditolak dengan pesan `"Aset sedang dipinjam dan tidak dapat dihapus!"` (meskipun masih ada sisa unit lain yang tersedia atau mencegah cascade menghapus pinjaman aktif).
  - Jika aman, mengirim `DELETE /rest/v1/assets?kode_aset=eq.{kode_aset}`, menghapus objek dari `_assetsCache`, menghapus histori alokasi terkait dari `_alokasiCache` (selaras dengan `ON DELETE CASCADE` di PostgreSQL), lalu memanggil `_riwayat.Add(RiwayatAction.HapusAset, asset.KodeAset, asset.NamaAset, "-", $"Kategori: {asset.Kategori}, Kondisi terakhir: {asset.Kondisi}", user, asset.Jumlah)` agar penghapusan tetap tercatat di riwayat!
