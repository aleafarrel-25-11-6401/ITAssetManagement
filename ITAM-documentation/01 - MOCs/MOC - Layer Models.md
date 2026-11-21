---
title: "MOC - Layer Models"
aliases:
  - Models MOC
  - Peta Models
tags:
  - moc
  - models
  - datastore
type: moc
updated: 2026-11-15
---

# 📦 MOC - Layer Models

⬅️ Kembali ke [[00 - Home]] | Lihat juga: [[MOC - Arsitektur & Desain]] · [[MOC - Database Supabase]] · [[MOC - Layer Controllers]]

> [!info] Tentang Peta Konten Ini
> **MOC - Layer Models** mencakup seluruh kelas yang berada di namespace `ITAssetManagement.Models` (folder `Models/`). Layer ini bertugas merepresentasikan struktur data domain bisnis, konstanta status/kondisi, status sesi pengguna aktif, pembungkus hasil operasi (*result pattern*), serta jembatan komunikasi HTTP ke Supabase beserta *in-memory cache*.

---

## 📄 Daftar Dokumentasi di Layer Models

1. **[[Models.cs (Entitas & Konstanta Domain)]]**
   Dokumentasi lengkap untuk:
   - Kelas konstanta statis: `AssetStatus`, `AssetCondition`, `AlokasiStatus`, `RiwayatAction` (termasuk `HapusAset`).
   - Kelas model entitas: `AssetItem` (dengan stok multi-unit `Jumlah`, `Tersedia`, `Perbaikan`, `Rusak`, properti terhitung `Bermasalah`, `Dipinjam`, `IsBorrowed`, `IsReadyToLend`, serta `FormatKondisi()`), `KaryawanItem`, `AlokasiItem` (dengan `Jumlah` & `IsActive`), dan `RiwayatItem` (dengan `Jumlah`).
   - Penggunaan atribut `[Browsable(false)]` dan *override* `ToString()` untuk integrasi langsung dengan komponen WinForms (`DataGridView` dan `ComboBox`).
2. **[[DataStore.cs (REST Client & In-Memory Cache)]]**
   Dokumentasi lengkap mesin komunikasi HTTP ke Supabase (`/rest/v1/*`):
   - Konfigurasi `HttpClient` tunggal (*singleton-like static initialization*), TLS 1.2, header `apikey`, dan `Prefer: return=minimal`.
   - Empat koleksi cache memori statis (`_assetsCache`, `_karyawanCache`, `_alokasiCache`, `_riwayatCache`) dengan masa berlaku TTL 60 detik (`ClearExpiredCache()`).
   - Generator kode otomatis `NextCode()` (`AST-xxx`, `EMP-xxx`, `ALK-xxx`), `RenameHistoryUser()`, dan *network error throttling* (`_lastErrorTime` 3 detik).
3. **[[UserSession & OperationResult]]**
   Dokumentasi untuk:
   - Enum `UserRole` (`Admin`, `Staff`).
   - Kelas sesi aktif `UserSession` (`UserSession.Current`, `Username`, `NamaLengkap`, `DisplayName`, `IsAdmin`, `ProfileText`).
   - Kelas pola hasil eksekusi `OperationResult` (`Success`, `Message`, `Ok()`, `Fail()`).

---

## 🧩 Diagram Kelas Layer Models

```mermaid
classDiagram
    class AssetStatus {
        <<static>>
        +const string Tersedia = "Tersedia"
        +const string Dipinjam = "Dipinjam"
    }
    class AssetCondition {
        <<static>>
        +const string Baik = "Baik"
        +const string Perbaikan = "Perbaikan"
        +const string Rusak = "Rusak"
    }
    class AlokasiStatus {
        <<static>>
        +const string Aktif = "Aktif"
        +const string Dikembalikan = "Dikembalikan"
    }
    class RiwayatAction {
        <<static>>
        +const string Peminjaman = "Peminjaman"
        +const string Pengembalian = "Pengembalian"
        +const string TambahAset = "Tambah Aset"
        +const string HapusAset = "Hapus Aset"
    }
    class AssetItem {
        +string KodeAset
        +string NamaAset
        +string Kategori
        +int Jumlah
        +int Tersedia
        +string Kondisi
        +string Status
        +int Perbaikan
        +int Rusak
        +int Bermasalah
        +int Dipinjam
        +bool IsBorrowed
        +bool IsReadyToLend
        +static FormatKondisi(int, int, int) string
        +ToString() string
    }
    class KaryawanItem {
        +string Nik
        +string Nama
        +string Divisi
        +string Jabatan
        +string Kontak
        +ToString() string
    }
    class AlokasiItem {
        +string IdAlokasi
        +string KodeAset
        +string NamaAset
        +int Jumlah
        +string NikKaryawan
        +string NamaKaryawan
        +string Divisi
        +DateTime TanggalPinjam
        +string Keterangan
        +string StatusAlokasi
        +bool IsActive
    }
    class RiwayatItem {
        +DateTime Waktu
        +string JenisAksi
        +string KodeAset
        +string NamaAset
        +int Jumlah
        +string NamaKaryawan
        +string Catatan
        +string OlehUser
    }
    class UserSession {
        +static UserSession Current
        +string Username
        +string NamaLengkap
        +UserRole Role
        +string DisplayName
        +bool IsAdmin
        +string ProfileText
    }
    class OperationResult {
        +bool Success
        +string Message
        +static Ok(string) OperationResult
        +static Fail(string) OperationResult
    }
    class DataStore {
        <<static public>>
        -static HttpClient _http
        -static List~AssetItem~ _assetsCache
        -static List~KaryawanItem~ _karyawanCache
        -static List~AlokasiItem~ _alokasiCache
        -static readonly TimeSpan CacheDuration
        -static DateTime _cacheLoadedAt
        -static List~RiwayatItem~ _riwayatCache
        +static GetAssets() List~AssetItem~
        +static GetKaryawan() List~KaryawanItem~
        +static GetAlokasi() List~AlokasiItem~
        +static GetRiwayat() List~RiwayatItem~
        +static FindUser(string) Dictionary
        +static RenameHistoryUser(string, string) void
        +static Insert(string, object) void
        +static Update(string, object) void
        +static Delete(string) void
        +static NextCode(string, IEnumerable~string~) string
        +static ClearCache() void
        +static ClearExpiredCache() void
    }

    DataStore --> AssetItem : caches & maps
    DataStore --> KaryawanItem : caches & maps
    DataStore --> AlokasiItem : caches & maps
    DataStore --> RiwayatItem : caches & maps
    AssetItem ..> AssetStatus : uses
    AssetItem ..> AssetCondition : uses
    AlokasiItem ..> AlokasiStatus : uses
    RiwayatItem ..> RiwayatAction : uses
```
