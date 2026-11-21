---
title: "RiwayatController"
aliases:
  - RiwayatController.cs
  - Controller Riwayat
tags:
  - controllers
  - riwayat
  - audit-log
layer: controllers
type: reference
updated: 2026-11-21
---

# 📜 RiwayatController (`Controllers/RiwayatController.cs`)

⬅️ Kembali ke [[MOC - Layer Controllers]] | [[00 - Home]] | Lihat juga: [[Tabel riwayat]] · [[UC_Riwayat]] · [[UC_Dashboard]]

> [!info] Lokasi File Kode Sumber
> - **File**: [Controllers/RiwayatController.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/RiwayatController.cs#L1-L93)
> - **Namespace**: `ITAssetManagement.Controllers`
> - **Class**: `public class RiwayatController`

---

## 1. Tanggung Jawab Utama

`RiwayatController` bertanggung jawab atas pencatatan, penyaringan, dan pembersihan log audit aktivitas (`Tambah Aset`, `Hapus Aset`, `Peminjaman`, `Pengembalian`) pada [[Tabel riwayat|public.riwayat]].

---

## 2. Bedah Detail Setiap Method

### 1. `Add(string aksi, string kodeAset, string namaAset, string karyawan, string catatan, string user, int jumlah = 1)` ([L12-L52](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/RiwayatController.cs#L12-L52))
- **Dipanggil secara otomatis oleh**:
  - [[AssetController|AssetController.Add]] ([AssetController.cs#L127](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L127)) saat aset baru ditambahkan (meneruskan `jumlah` unit baru).
  - [[AssetController|AssetController.Delete]] ([AssetController.cs#L210-L217](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AssetController.cs#L210-L217)) saat aset dihapus oleh Admin (mencatat `Hapus Aset` beserta `asset.Jumlah` dan kondisi terakhir).
  - [[AlokasiController|AlokasiController.Allocate]] ([AlokasiController.cs#L91](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L91)) saat aset dipinjamkan (meneruskan `jumlahPinjam` unit ke kolom `Jumlah` tersendiri, dan `keterangan` murni ke `Catatan`).
  - [[AlokasiController|AlokasiController.Return]] ([AlokasiController.cs#L161](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AlokasiController.cs#L161)) saat aset dikembalikan (meneruskan `jumlahKembali` unit).
- **Desain Non-Blocking, Pengiriman `username_pelaku`, & Urutan Kronologis Terbalik**:
  - Mengonversi `DateTime.Now` ke UTC (`sekarang.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")`) dan menyertakan `jumlah = item.Jumlah` serta `username_pelaku = UserSession.Current?.Username` (Foreign Key ke Primary Key `app_users.username`) saat mengirim `POST /rest/v1/riwayat`.
  - Membungkus `DataStore.Insert("riwayat", ...)` di dalam `try-catch` agar gangguan sesaat pada pencatatan log tidak membatalkan transaksi utama yang sudah berhasil.
  - Menyisipkan item baru di indeks ke-`0` (`DataStore.GetRiwayat().Insert(0, item)`) sehingga daftar riwayat di memori tetap terurut dari yang paling baru ke yang paling lama (`waktu.desc`).

### 2. `Search(string aksi, string keyword)` ([L55-L67](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/RiwayatController.cs#L55-L67))
- **Dipanggil oleh**: [[UC_Riwayat|UC_Riwayat.LoadGrid]] ([UC_Riwayat.cs#L44](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Riwayat.cs#L44)).
- **Cara Kerja**:
  - Filter 1 (`aksi`): Jika `string.IsNullOrEmpty(aksi)` (pilihan `"Semua"` di `cmbFilterAksi`), loloskan semua jenis aksi; jika tidak, saring `r.JenisAksi == aksi`.
  - Filter 2 (`keyword`): Saring secara *case-insensitive* pada **6 kolom sekaligus**: `JenisAksi`, `KodeAset`, `NamaAset`, `NamaKaryawan`, `Catatan`, dan `OlehUser`.

### 3. `Latest(int jumlah)` ([L70](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/RiwayatController.cs#L70))
- **Dipanggil oleh**: [[UC_Dashboard|UC_Dashboard.OnLoad]] ([UC_Dashboard.cs#L34](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Dashboard.cs#L34)) dengan argumen `10`.
- **Cara Kerja**: `DataStore.GetRiwayat().Take(Math.Max(0, jumlah)).ToList()` — mengambil 10 aktivitas terbaru untuk tabel ringkasan di Dashboard.

### 4. `ClearAll(bool isAdmin)` ([L73-L91](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/RiwayatController.cs#L73-L91))
- **Dipanggil oleh**: [[UC_Riwayat|UC_Riwayat.btnBersihkan_Click]] ([UC_Riwayat.cs#L77](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Riwayat.cs#L77)).
- **Proteksi & Eksekusi**:
  1. Memeriksa `if (!isAdmin)` $\rightarrow$ menolak eksekusi jika dipanggil oleh non-Admin.
  2. Memeriksa apakah `DataStore.GetRiwayat().Count == 0`.
  3. Mengirim `DELETE /rest/v1/riwayat?waktu=not.is.null` ke Supabase lalu memanggil `DataStore.GetRiwayat().Clear()`.
