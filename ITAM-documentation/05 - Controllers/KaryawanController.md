---
title: "KaryawanController"
aliases:
  - KaryawanController.cs
  - Controller Karyawan
tags:
  - controllers
  - karyawan
  - crud
layer: controllers
type: reference
updated: 2026-11-21
---

# 🧑‍💼 KaryawanController (`Controllers/KaryawanController.cs`)

⬅️ Kembali ke [[MOC - Layer Controllers]] | [[00 - Home]] | Lihat juga: [[Tabel karyawan]] · [[UC_Karyawan]] · [[UC_Alokasi]]

> [!info] Lokasi File Kode Sumber
> - **File**: [Controllers/KaryawanController.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L2-L155)
> - **Namespace**: `ITAssetManagement.Controllers`
> - **Class**: `public class KaryawanController`

---

## 1. Tanggung Jawab Utama

`KaryawanController` mengelola data master karyawan penerima aset IT di Supabase dan di cache memori:
- Mengambil seluruh daftar karyawan atau menyaringnya berdasarkan kata kunci (`GetAll`).
- Menghasilkan nomor induk karyawan urut otomatis `EMP-001`, `EMP-002`, dst. (`NextNik`).
- Menambahkan karyawan baru dengan pengisian nilai default otomatis (`Add`).
- **Menolak kontak (nomor telepon/email) yang sudah dipakai karyawan lain** dengan dialog peringatan sebelum permintaan dikirim ke server (`Add`, `Update`, `NormalisasiKontak`).
- Memperbarui data karyawan sekaligus menyinkronkan perubahan `nama` dan `divisi` ke tabel [[Tabel alokasi|alokasi]] (`Update`).
- Menghapus karyawan dengan proteksi terhadap pinjaman aset yang masih aktif (`Delete`).

---

## 2. Bedah Detail Setiap Method

### 1. `GetAll(string keyword = "")` ([L13-L22](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L13-L22))
- **Dipanggil oleh**: [[UC_Karyawan|UC_Karyawan.LoadGrid]] ([UC_Karyawan.cs#L36](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Karyawan.cs#L36)) dan [[UC_Alokasi|UC_Alokasi.RefreshData]] ([UC_Alokasi.cs#L41](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L41)).
- **Cara Kerja**: Menyaring `DataStore.GetKaryawan()` secara *case-insensitive* berdasarkan 4 properti: `Nik`, `Nama`, `Divisi`, atau `Jabatan`.

### 2. `NextNik()` ([L25](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L25))
- **Dipanggil oleh**: [[UC_Karyawan|UC_Karyawan.ResetForm]] ([UC_Karyawan.cs#L43](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Karyawan.cs#L43)) dan `KaryawanController.Add` ([L37](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L37)).
- **Cara Kerja**: Memanggil `DataStore.NextCode("EMP-", DataStore.GetKaryawan().Select(k => k.Nik))`.

### 3. `Add(string nama, string divisi, string jabatan, string kontak)` ([L28-L60](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L28-L60))
- **Dipanggil oleh**: [[UC_Karyawan|UC_Karyawan.btnTambah_Click]] ([UC_Karyawan.cs#L54](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Karyawan.cs#L54)).
- **Alur Eksekusi**:
  1. Validasi: Jika `nama` kosong/spasi, kembalikan `OperationResult.Fail("Nama Karyawan wajib diisi!")`.
  2. **Validasi kontak unik** *(baru, 2026-11-21)*: jika `KontakDipakaiKaryawanLain(IsiDefault(kontak, "-"), null)` bernilai `true`, kembalikan `OperationResult.Fail("Nomor telepon/email kontak sudah digunakan karyawan lain!")`. Tidak ada permintaan yang dikirim ke server.
  3. Bentuk objek `KaryawanItem` dengan `Nik = NextNik()` dan nilai default untuk kolom opsional melalui `IsiDefault()` (`Divisi = "Umum"`, `Jabatan = "Staff"`, `Kontak = "-"`).
  4. Kirim `POST /rest/v1/karyawan` via `DataStore.Insert("karyawan", ...)` lalu tambahkan ke `DataStore.GetKaryawan()`.

### 4. `Update(KaryawanItem karyawan, string nama, string divisi, string jabatan, string kontak)` ([L63-L115](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L63-L115))
- **Dipanggil oleh**: [[UC_Karyawan|UC_Karyawan.btnUbah_Click]] ([UC_Karyawan.cs#L73](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Karyawan.cs#L73)).
- **Alur Eksekusi & Sinkronisasi Denormalisasi**:
  1. Validasi `karyawan != null` dan `nama` tidak kosong.
  2. **Validasi kontak unik** *(baru, 2026-11-21)*: hanya jika kontak (setelah dinormalisasi) berbeda dari kontak karyawan saat ini **dan** dipakai karyawan lain, kembalikan `OperationResult.Fail("Nomor telepon/email kontak sudah digunakan karyawan lain!")`. Kontak yang tidak diubah pada baris yang sama tidak ditolak; perilaku ini sama dengan trigger database.
  3. Kirim `PATCH /rest/v1/karyawan?nik=eq.{nik}`.
  4. **Sinkronisasi ke Tabel `alokasi`**: Jika `karyawan.Nama != nama` atau `karyawan.Divisi != divisi` dan terdapat transaksi di `DataStore.GetAlokasi()` untuk `NikKaryawan == karyawan.Nik`, maka otomatis mengirim `PATCH /rest/v1/alokasi?nik_karyawan=eq.{nik}` (`{ nama_karyawan = nama, divisi = divisi }`) dan memperbarui objek di `_alokasiCache`!
  5. Perbarui properti objek `karyawan` di `_karyawanCache`.

### 5. `Delete(KaryawanItem karyawan)` ([L118-L136](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L118-L136))
- **Dipanggil oleh**: [[UC_Karyawan|UC_Karyawan.btnHapus_Click]] ([UC_Karyawan.cs#L94](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Karyawan.cs#L94)).
- **Aturan Proteksi Bisnis**:
  - Mengecek `DataStore.GetAlokasi().Any(a => a.NikKaryawan == karyawan.Nik && a.IsActive)`. Jika karyawan masih meminjam aset yang belum dikembalikan, penghapusan ditolak dengan pesan `"Karyawan masih memiliki pinjaman aset aktif!"`.
  - Jika aman, mengirim `DELETE /rest/v1/karyawan?nik=eq.{nik}` dan menghapus dari `_karyawanCache` serta `_alokasiCache`.

### 6. `PesanKontakSama`, `NormalisasiKontak`, dan `KontakDipakaiKaryawanLain` ([L138-L150](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L138-L150)) *(baru, 2026-11-21)*
- **`NormalisasiKontak(string kontak)`** (`public static`): menghapus spasi, titik, tanda kurung, dan strip (`Regex.Replace(kontak ?? "", @"[\s().\-]", "")`) lalu mengubah ke huruf kecil. Aturannya harus sama dengan ekspresi di fungsi database `enforce_karyawan_kontak_unique()` ([[Keamanan RLS, Trigger, dan Fungsi Database]]); ubah keduanya bersamaan.
- **`KontakDipakaiKaryawanLain(kontak, nikSendiri)`** (`private static`): `true` bila ada karyawan lain di `DataStore.GetKaryawan()` dengan kontak ternormalisasi yang sama. Kontak kosong atau `-` menghasilkan string kosong dan selalu lolos.
- **Batasan**: pemeriksaan memakai cache memori (maksimal 60 detik basi, lihat [[ADR-002 - Strategi In-Memory Caching pada DataStore]]). Jika karyawan lain baru menyimpan kontak yang sama dari komputer lain, trigger database tetap menolaknya, tetapi pesan yang tampil di aplikasi pada kasus langka itu adalah pesan umum "Periksa koneksi internet Anda".
- **Pengujian**: `ITAssetManagement.Tests/ControllerWorkflowTests.cs` (kelas `KaryawanKontakTests`).

### 7. `IsiDefault(string teks, string nilaiDefault)` ([L152-L153](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/KaryawanController.cs#L152-L153))
- Helper `private static` yang mengembalikan `nilaiDefault` jika `string.IsNullOrWhiteSpace(teks)` bernilai `true`, atau `teks.Trim()` jika terisi.
