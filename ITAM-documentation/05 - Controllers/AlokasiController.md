---
title: "AlokasiController"
aliases:
  - AlokasiController.cs
  - Controller Alokasi
tags:
  - controllers
  - alokasi
  - transaksi
layer: controllers
type: reference
updated: 2026-11-15
---

# 🤝 AlokasiController (`Controllers/AlokasiController.cs`)

⬅️ Kembali ke [[MOC - Layer Controllers]] | [[00 - Home]] | Lihat juga: [[Tabel alokasi]] · [[Tabel assets]] · [[UC_Alokasi]] · [[Alur Peminjaman (Alokasi) & Pengembalian Aset]]

> [!info] Lokasi File Kode Sumber
> - **File**: [Controllers/AlokasiController.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L1-L170)
> - **Namespace**: `ITAssetManagement.Controllers`
> - **Class**: `public class AlokasiController`

---

## 1. Tanggung Jawab Utama

`AlokasiController` adalah penggerak utama sirkulasi keluar-masuk perangkat IT perusahaan dengan dukungan **stok multi-unit (`Jumlah` & `Tersedia`)** dan **peminjaman beberapa unit sekaligus (`jumlahPinjam`)**:
1. Menyaring daftar transaksi peminjaman yang masih berjalan (`GetActive`).
2. Memproses penyerahan/peminjaman 1 atau beberapa unit aset sekaligus (`jumlahPinjam`) kepada karyawan serta mengurangi sisa stok `Tersedia` (`Allocate`).
3. Memproses pengembalian seluruh unit pada transaksi alokasi (`loan.Jumlah`) dari karyawan, memulihkan sisa stok `Tersedia` (`+loan.Jumlah`), serta memperbarui kondisi fisik perangkat (`Return`).

---

## 2. Bedah Detail Setiap Method

### 1. `GetActive()` ([L14](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L14))
- **Dipanggil oleh**: [[UC_Dashboard|UC_Dashboard.OnLoad]] ([UC_Dashboard.cs#L33](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Dashboard.cs#L33)) dan [[UC_Alokasi|UC_Alokasi.RefreshData]] ([UC_Alokasi.cs#L75](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L75)).
- **Cara Kerja**: `DataStore.GetAlokasi().Where(a => a.IsActive).ToList()` — menyaring hanya transaksi yang memiliki `StatusAlokasi == "Aktif"`.

### 2. `Allocate(AssetItem asset, KaryawanItem karyawan, DateTime tanggal, string keterangan, string user, int jumlahPinjam = 1)` ([L30-L98](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L30-L98))
- **Dipanggil oleh**: [[UC_Alokasi|UC_Alokasi.btnAlokasi_Click]] ([UC_Alokasi.cs#L117](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L117)) *(Tersedia untuk Admin IT & Staff IT)*.
- **Urutan Validasi & Transaksi Multi-Unit**:
  1. Validasi `asset != null`, `asset.IsReadyToLend == true` (`Tersedia > 0`, `Status == "Tersedia"`, dan `Kondisi` tidak sepenuhnya `"Perbaikan"` / `"Rusak"`), serta `karyawan != null`.
  2. Validasi kuantitas pinjam:
     - `if (jumlahPinjam < 1)` $\rightarrow$ gagal (`"Jumlah unit yang dipinjam minimal 1!"`).
     - `if (jumlahPinjam > asset.Tersedia)` $\rightarrow$ gagal (`"Jumlah pinjam (...) tidak boleh melebihi stok tersedia (...)!"`).
  3. Jika `keterangan` kosong, isi default `"Peminjaman operasional kerja"`.
  4. Buat kode transaksi baru `idAlokasi = DataStore.NextCode("ALK-", ...)` (misal `"ALK-003"`).
  5. **Perhitungan Stok Multi-Unit**:
     - `int sisaTersedia = Math.Max(0, asset.Tersedia - jumlahPinjam);`
     - `string statusBaru = sisaTersedia > 0 ? AssetStatus.Tersedia : AssetStatus.Dipinjam;`
     - Artinya: apabila aset berjumlah 5 unit (`Tersedia = 5`) lalu dipinjam 3 unit sekaligus, maka `sisaTersedia = 2` dan `statusBaru` tetap `"Tersedia"` sehingga 2 unit sisanya masih dapat dipinjamkan! Baru ketika seluruh unit tersedia habis dipinjam (`sisaTersedia = 0`), `statusBaru` berubah menjadi `"Dipinjam"`.
  6. **Langkah DB 1**: Panggil `UpdateStok(...)` untuk mengirim `PATCH /rest/v1/assets?kode_aset=eq.{kode}` dengan `tersedia = sisaTersedia` dan `status = statusBaru`.
  7. **Langkah DB 2**: Kirim `POST /rest/v1/alokasi` untuk menyimpan baris transaksi baru dengan `jumlah = jumlahPinjam`, `status_alokasi = "Aktif"`, dan `tanggal_pinjam = tanggal.ToString("yyyy-MM-dd")`. Bila `POST` gagal, stok dikembalikan ke nilai semula (rollback via `UpdateStok`) agar unit tidak hilang dari gudang.
  8. **Langkah Cache**: Ubah `asset.Tersedia = sisaTersedia`, `asset.Status = statusBaru` di memori, dan tambahkan objek `AlokasiItem` baru (`Jumlah = jumlahPinjam`) ke `_alokasiCache`.
  9. **Langkah Audit Log**: Panggil `_riwayat.Add(RiwayatAction.Peminjaman, asset.KodeAset, asset.NamaAset, karyawan.Nama, keterangan, user, jumlahPinjam)` — jumlah unit disimpan pada kolom `Jumlah` tersendiri di Riwayat sehingga `Catatan` murni menampilkan `keterangan` tanpa terpotong.

### 3. `Return(AlokasiItem loan, string kondisiKembali, string user, int? jumlahKondisi = null)` ([L102-L168](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L102-L168))
- **Dipanggil oleh**: [[UC_Alokasi|UC_Alokasi.btnKembalikan_Click]] ([UC_Alokasi.cs#L150](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L150)) *(Tersedia untuk Admin IT & Staff IT)*.
- **Urutan Validasi & Pemulihan Stok Multi-Unit**:
  1. Validasi `loan != null` dan `loan.IsActive == true`.
  2. Jika `kondisiKembali` bukan `Baik`, `jumlahKondisi` (default: seluruh `jumlahKembali` unit pada transaksi) harus di antara `1..jumlahKembali`; nilainya menjadi `unitBermasalah`, sedangkan unit sisanya kembali `Baik`. Hitung `jumlahKembali = Math.Max(1, loan.Jumlah)` dan `sisaDipinjam = Math.Max(0, asset.Dipinjam - jumlahKembali)`.
  3. **Kondisi Dinamis per Unit** (hanya unit yang dikembalikan yang terkena kondisi baru):
     - `kondisiKembali == "Rusak"` → `rusakBaru = asset.Rusak + unitBermasalah`.
     - `kondisiKembali == "Perbaikan"` → `perbaikanBaru = asset.Perbaikan + unitBermasalah`.
     - `kondisiKembali == "Baik"` → `Perbaikan`/`Rusak` tidak berubah.
     - `tersediaBaru = Jumlah - sisaDipinjam - perbaikanBaru - rusakBaru` dan `kondisiBaru = AssetItem.FormatKondisi(...)`. Contoh: 4 unit, 2 dipinjam lalu dikembalikan **Rusak** → `Tersedia = 2`, `Rusak = 2`, `Kondisi = "Baik (2), Rusak (2)"` (bukan seluruh 4 unit Rusak).
  4. **Langkah DB 1**: Panggil `UpdateStok(...)` (`PATCH /rest/v1/assets?kode_aset=eq.{kode_aset}`) dengan `{ tersedia, perbaikan, rusak, kondisi, status }` (`status = "Dipinjam"` hanya jika `tersediaBaru == 0 && sisaDipinjam > 0`).
  5. **Langkah DB 2**: `PATCH /rest/v1/alokasi?id_alokasi=eq.{id_alokasi}` menjadi `status_alokasi = "Dikembalikan"`. Bila gagal, stok pada langkah 4 dibatalkan (rollback via `UpdateStok`) agar tidak terhitung ganda saat dicoba ulang.
  6. **Langkah Cache**: Ubah `loan.StatusAlokasi` dan sinkronkan `asset.Tersedia/Perbaikan/Rusak/Kondisi/Status` di `_assetsCache`.
  7. **Langkah Audit Log**: Panggil `_riwayat.Add(RiwayatAction.Pengembalian, loan.KodeAset, loan.NamaAset, loan.NamaKaryawan, $"Kondisi kembali: {kondisiKembali}", user, jumlahKembali)`; bila hanya sebagian unit bermasalah, catatan menjadi `"Kondisi kembali: {kondisiKembali} ({unitBermasalah} unit), sisanya Baik"`.

### 4. `UpdateStok(string kodeAset, int tersedia, int perbaikan, int rusak, string kondisi, string status)` ([L17-L27](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L17-L27)) *(private static)*
- **Dipanggil oleh**: `Allocate` ([L50, L71](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L50-L71)) dan `Return` ([L135, L144](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L135-L144)).
- **Cara Kerja**: Satu `PATCH /rest/v1/assets?kode_aset=eq.{kode}` yang mengirim seluruh rincian stok sekaligus, sehingga perubahan maju dan rollback memakai jalur kode yang sama.
