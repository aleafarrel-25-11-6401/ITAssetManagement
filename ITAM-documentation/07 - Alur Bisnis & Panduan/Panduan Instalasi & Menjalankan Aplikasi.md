---
title: "Panduan Instalasi & Menjalankan Aplikasi"
aliases:
  - Quick Start
  - Panduan Instalasi
  - Tutorial Menjalankan Aplikasi
tags:
  - tutorial
  - diataxis
  - instalasi
  - build
layer: workflow
type: tutorial
updated: 2026-11-15
---

# 🚀 Panduan Instalasi & Menjalankan Aplikasi

⬅️ Kembali ke [[MOC - Panduan & Alur Bisnis]] | [[00 - Home]] | Lihat juga: [[App.config & Proyek (.csproj)]]

> [!info] Kuadran Diátaxis: **Tutorial** *(Learning-Oriented)*
> Panduan ini memandu developer atau penguji baru menyiapkan, mengompilasi, dan menjalankan aplikasi **IT Asset Management** di lingkungan Windows dari awal hingga berhasil masuk ke halaman [[UC_Dashboard]].

---

## 1. Prasyarat Sistem (*Prerequisites*)

1. **Sistem Operasi**: Windows 10 atau Windows 11.
2. **Runtime & SDK**: **.NET Framework 4.7.2** Developer Pack (sudah terinstal secara bawaan pada Visual Studio 2019 / 2022).
3. **IDE / Build Tool**: Visual Studio 2019/2022 (dengan workload *.NET Desktop Development*) atau `MSBuild.exe` / `csc.exe`.
4. **Koneksi Internet Aktif**: Diperlukan agar aplikasi dapat menghubungi server Supabase (`https://agbmfszugvjwvinxfgiy.supabase.co`) melalui HTTPS (TLS 1.2).

---

## 2. Langkah-Langkah Menjalankan Proyek

### Langkah 1: Periksa Konfigurasi Kredensial Supabase di `App.config`
Pastikan file [App.config](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/App.config#L8-L11) di direktori utama proyek memiliki konfigurasi `<appSettings>` berikut:
```xml
<appSettings>
    <add key="SupabaseUrl" value="https://<PROJECT_REF>.supabase.co" />
    <add key="SupabaseKey" value="sb_publishable_<YOUR_SUPABASE_PUBLISHABLE_KEY>" />
</appSettings>
```

### Langkah 2: Build dan Jalankan Aplikasi
- **Melalui Visual Studio**:
  1. Buka file proyek `ITAssetManagement.csproj`.
  2. Tekan `F5` (*Start Debugging*) atau `Ctrl + F5` (*Start Without Debugging*).
- **Melalui Terminal PowerShell (`MSBuild`)**:
  ```powershell
  msbuild ITAssetManagement.csproj /p:Configuration=Debug
  .\bin\Debug\ITAssetManagement.exe
  ```

### Langkah 3: Masuk (Login) atau Daftar Akun Baru
1. Saat jendela [[FormLogin & FormRegister|FormLogin]] terbuka, Anda dapat masuk menggunakan akun yang tersedia di [[Tabel app_users]]:
   - Jika belum mengetahui password akun yang ada, klik tombol **"Belum punya akun? Daftar Akun"** untuk membuka [[FormLogin & FormRegister|FormRegister]].
2. Buat akun baru (username minimal 3 karakter tanpa spasi, password minimal 6 karakter).
3. Setelah pendaftaran berhasil, username Anda akan otomatis terisi di `FormLogin`. Masukkan password Anda lalu tekan `Enter` atau klik **Login**.
4. Jendela utama [[MainForm (Navigasi & RBAC UI)|MainForm]] akan terbuka dan langsung menampilkan halaman [[UC_Dashboard]].

*(Catatan: Pengujian otomatis dapat dibuat pada proyek eksternal terpisah seperti NUnit dengan mereferensikan proyek utama `ITAssetManagement`, karena `DataStore` beserta seluruh Model, Controller, dan View helper sudah bervisibilitas `public`.)*
