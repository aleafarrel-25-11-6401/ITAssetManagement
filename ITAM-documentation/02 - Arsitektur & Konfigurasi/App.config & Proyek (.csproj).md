---
title: "App.config & Proyek (.csproj)"
aliases:
  - App.config
  - ITAssetManagement.csproj
  - Konfigurasi Proyek
tags:
  - arsitektur
  - konfigurasi
  - csproj
  - supabase
layer: root
type: reference
updated: 2026-11-15
---

# ⚙️ App.config & Proyek (.csproj)

⬅️ Kembali ke [[MOC - Arsitektur & Desain]] | Lihat juga: [[DataStore.cs (REST Client & In-Memory Cache)]] · [[Panduan Instalasi & Menjalankan Aplikasi]]

> [!info] Lokasi File Kode Sumber
> - [App.config](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/App.config#L1-L12)
> - [ITAssetManagement.csproj](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/ITAssetManagement.csproj#L1-L160)
> - [Properties/AssemblyInfo.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Properties/AssemblyInfo.cs#L1-L32)

---

## 1. Bedah Konfigurasi `App.config`

File [App.config](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/App.config#L1-L12) menyimpan pengaturan runtime `.NET Framework` serta kredensial koneksi ke backend Supabase yang dibaca oleh [[DataStore.cs (REST Client & In-Memory Cache)|DataStore.InitClient()]] menggunakan `ConfigurationManager.AppSettings`:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
    </startup>

    <!-- Konfigurasi kredensial koneksi ke backend Supabase (dibaca melalui ConfigurationManager.AppSettings) -->
    <appSettings>
        <add key="SupabaseUrl" value="https://<PROJECT_REF>.supabase.co" />
        <add key="SupabaseKey" value="sb_publishable_<YOUR_SUPABASE_PUBLISHABLE_KEY>" />
    </appSettings>
</configuration>
```

### Detail Kunci Konfigurasi (`<appSettings>`)

| Key | Contoh Nilai | Fungsi di Aplikasi | Dibaca Oleh |
| :--- | :--- | :--- | :--- |
| `SupabaseUrl` | `https://<PROJECT_REF>.supabase.co` | URL dasar proyek Supabase. Di dalam `DataStore`, URL ini digabungkan dengan `/rest/v1/{endpoint}`. | [DataStore.cs#L35](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L35) |
| `SupabaseKey` | `sb_publishable_<YOUR_SUPABASE_PUBLISHABLE_KEY>` | **Supabase Publishable Key** (format baru pengganti *legacy anon JWT key*). Dikirim sebagai header HTTP `apikey`. (Jika menggunakan format JWT lama yang diawali `eyJ`, `DataStore` juga otomatis menambahkan header `Authorization: Bearer <key>`). | [DataStore.cs#L36-L49](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L36-L49) |

> [!tip] Keamanan Publishable Key sesuai *Supabase Best Practices*
> Sesuai panduan resmi [[Keamanan RLS, Trigger, dan Fungsi Database]], aplikasi desktop (*public client*) menggunakan **Publishable Key** (`sb_publishable_...`) dan **tidak pernah** menyimpan `service_role` / `secret` key di dalam `App.config`. Keamanan tabel dijaga di sisi server menggunakan kebijakan *Row-Level Security (RLS)* dan *Trigger* PostgreSQL.

---

## 2. Spesifikasi Proyek `ITAssetManagement.csproj`

File [ITAssetManagement.csproj](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/ITAssetManagement.csproj#L1-L160) mendefinisikan struktur build MSBuild (ToolsVersion `15.0`):

### A. Properti Utama Proyek
- **ProjectGuid**: `{2D3BB965-E0E9-4C2E-899F-89694765B241}`
- **OutputType**: `WinExe` (Aplikasi Desktop Windows GUI tanpa jendela konsol)
- **RootNamespace & AssemblyName**: `ITAssetManagement`
- **TargetFrameworkVersion**: `v4.7.2` (.NET Framework 4.7.2)
- **Akses Kelas Publik**: Seluruh Model (`Models.cs`, `DataStore.cs`), Controller, dan View helper (`UIHelper`, `Dialogs`) bervisibilitas `public` sehingga proyek pengujian eksternal dapat mereferensikan assembly utama secara langsung tanpa `InternalsVisibleTo`.

### B. Daftar Referensi Assembly (.NET Framework GAC)
Proyek ini menggunakan 13 referensi assembly standar bawaan .NET Framework ([ITAssetManagement.csproj#L35-L49](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/ITAssetManagement.csproj#L35-L49)):

| Assembly Reference | Kegunaan Spesifik di Proyek Ini |
| :--- | :--- |
| `System` | Tipe dasar .NET, `Uri.EscapeDataString`, `ServicePointManager` (TLS 1.2). |
| `System.Configuration` | Membaca `<appSettings>` dari `App.config` melalui `ConfigurationManager`. |
| `System.Core` | Ekstensi `System.Linq` (`Where`, `Select`, `FirstOrDefault`, `Any`, `Take`) dan `HashSet<T>`. |
| `System.Web.Extensions` | Menyediakan `JavaScriptSerializer` untuk serialisasi dan deserialisasi JSON ke PostgREST Supabase tanpa paket NuGet eksternal. |
| `System.Net.Http` | Menyediakan `HttpClient`, `HttpRequestMessage`, `HttpMethod`, dan `StringContent` untuk request REST API (`GET`, `POST`, `PATCH`, `DELETE`). |
| `System.Windows.Forms` | Komponen UI desktop (`Form`, `UserControl`, `DataGridView`, `TableLayoutPanel`, `Button`, `TextBox`, `ComboBox`, `Timer`, `MessageBox`). |
| `System.Drawing` | Pengaturan warna (`Color.FromArgb`), font (`Segoe UI`), titik koordinat (`Point`, `Size`), dan ikon/gambar logo. |
| `System.Data` / `System.Xml` / `Microsoft.CSharp` | Dukungan standar proyek Windows Forms .NET Framework. |

---

## 3. Sumber Daya Tersemat (*Embedded Resources*)

Di dalam [ITAssetManagement.csproj#L116-L145](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/ITAssetManagement.csproj#L116-L145), terdapat file `.resx` yang menyimpan aset visual aplikasi:
- `Views/FormLogin.resx` & `Views/MainForm.resx`: Menyimpan gambar logo aplikasi (`pictureBox1.Image`) dan ikon jendela (`$this.Icon`). Saat membuka `FormRegister`, logo dan ikon diteruskan langsung dari `FormLogin` melalui konstruktor `new FormRegister(pictureBox1.Image, Icon)` sehingga tidak terjadi duplikasi resource!
- `Properties/Resources.resx` & `Properties/Settings.settings`: Resource dan pengaturan standar proyek WinForms.

---

## 🔗 Tautan Terkait
- [[ADR-001 - Komunikasi Langsung Supabase PostgREST Tanpa SDK Eksternal]]
- [[DataStore.cs (REST Client & In-Memory Cache)]]
- [[Panduan Instalasi & Menjalankan Aplikasi]]
