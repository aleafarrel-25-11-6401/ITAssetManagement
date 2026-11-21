---
title: "DataStore.cs (REST Client & In-Memory Cache)"
aliases:
  - DataStore.cs
  - DataStore
  - REST Client
  - In-Memory Cache
tags:
  - models
  - datastore
  - httpclient
  - cache
  - postgrest
layer: models
type: reference
updated: 2026-11-15
---

# 🌐 DataStore.cs (REST Client & In-Memory Cache)

⬅️ Kembali ke [[MOC - Layer Models]] | [[00 - Home]] | Lihat juga: [[ADR-001 - Komunikasi Langsung Supabase PostgREST Tanpa SDK Eksternal]] · [[ADR-002 - Strategi In-Memory Caching pada DataStore]]

> [!info] Lokasi File Kode Sumber
> - **File**: [Models/DataStore.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L1-L298)
> - **Namespace**: `ITAssetManagement.Models`
> - **Class**: `public static class DataStore`

---

## 1. Peran Arsitektural `DataStore`

`DataStore` adalah satu-satunya pintu komunikasi (*Single Gateway / Repository*) antara aplikasi C# dan backend **Supabase PostgREST API** (`/rest/v1/*`). Selain menangani protokol HTTP dan serialisasi JSON, `DataStore` memelihara **4 koleksi cache di memori (RAM)** agar navigasi antar halaman dan pencarian kata kunci berjalan instan tanpa latensi jaringan berulang.

---

## 2. Daftar Field Statis (`private static`)

| Field | Tipe | Baris | Kegunaan |
| :--- | :--- | :---: | :--- |
| `_baseUrl` | `string` | [L16](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L16) | Menyimpan URL proyek Supabase dari `App.config` tanpa garis miring penutup (`https://agbmfszugvjwvinxfgiy.supabase.co`). |
| `_http` | `HttpClient` | [L17](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L17) | Instance tunggal `HttpClient` dengan `Timeout = 15 detik` dan header autentikasi `apikey`. |
| `_json` | `readonly JavaScriptSerializer` | [L18](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L18) | Serializer/deserializer JSON bawaan `.NET Framework` (`System.Web.Extensions`). |
| `_lastErrorTime` | `DateTime` | [L19](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L19) | Mencatat waktu terakhir munculnya dialog error jaringan di `GetRows()` untuk mencegah banjir popup (*rate-limiting* minimal jeda 3 detik). |
| `CacheDuration` | `private static readonly TimeSpan` | [L22](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L22) | Masa berlaku cache memori (`60 detik`) sebelum data disegarkan ulang saat berpindah tab halaman. |
| `_cacheLoadedAt` | `DateTime` | [L23](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L23) | Penanda waktu kapan siklus cache aktif pertama kali diisi (`DateTime.MinValue` saat kosong). |
| `_assetsCache` | `List<AssetItem>` | [L24](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L24) | Cache daftar master aset IT di memori. |
| `_karyawanCache` | `List<KaryawanItem>` | [L25](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L25) | Cache daftar master karyawan di memori. |
| `_alokasiCache` | `List<AlokasiItem>` | [L26](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L26) | Cache daftar transaksi alokasi peminjaman di memori. |
| `_riwayatCache` | `List<RiwayatItem>` | [L27](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L27) | Cache daftar riwayat aktivitas di memori. |

---

## 3. Bedah Mendalam Setiap Method di `DataStore`

### A. Inisialisasi & Manajemen Cache
1. **`InitClient()`** ([L30-L52](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L30-L52)):
   - Mengecek `if (_http != null) return;` (hanya dijalankan satu kali selama aplikasi hidup).
   - Memaksa penggunaan protokol keamanan `SecurityProtocolType.Tls12`.
   - Membaca `SupabaseUrl` dan `SupabaseKey` dari `ConfigurationManager.AppSettings`.
   - Menambahkan header `apikey: <SupabaseKey>`. Jika key diawali `"eyJ"` (format JWT klasik), otomatis menambahkan `Authorization: Bearer <SupabaseKey>`.
2. **`ClearCache()`** ([L55-L62](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L55-L62)), **`ClearExpiredCache()`** ([L65-L70](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L65-L70)), & **`SetCache<T>()`** ([L73-L77](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L73-L77)):
   - `ClearCache()` mengatur keempat `_*Cache = null` dan mereset `_cacheLoadedAt = DateTime.MinValue`; dipanggil oleh `MainForm.btnLogout_Click` ([MainForm.cs#L115](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/MainForm.cs#L115)) saat Logout atau saat cache kedaluwarsa.
   - `ClearExpiredCache()` dipanggil oleh [[MainForm (Navigasi & RBAC UI)|MainForm.ShowPage]] ([MainForm.cs#L90](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/MainForm.cs#L90)) setiap berpindah tab halaman — memanggil `ClearCache()` hanya bila umur cache telah mencapai `CacheDuration` (60 detik), sehingga pindah menu di bawah 60 detik berlangsung instan dari RAM tanpa membebani jaringan.
   - `SetCache<T>()` digunakan bersama oleh `GetAssets`, `GetKaryawan`, `GetAlokasi`, dan `GetRiwayat` untuk mengisi cache serta mencatat `_cacheLoadedAt = DateTime.Now` saat belum terisi.

### B. Pembacaan Data dengan *Lazy-Loading Cache* (`GetAssets`, `GetKaryawan`, `GetAlokasi`, `GetRiwayat`)
Keempat method pembacaan ([L80-L185](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L80-L185)) mengikuti pola yang seragam dan tahan gangguan jaringan:
```csharp
if (_assetsCache == null)
{
    var rows = GetRows("assets?select=kode_aset,nama_aset,kategori,jumlah,tersedia,perbaikan,rusak,kondisi,status&order=kode_aset.asc", out bool sukses);
    var list = rows.Select(row => new AssetItem { ... }).ToList();
    if (!sukses) return list; // JANGAN simpan ke _assetsCache jika gagal jaringan!
    SetCache(ref _assetsCache, list);
}
return _assetsCache;
```
> [!important] Mengapa `if (!sukses) return list;` Sangat Penting?
> Jika koneksi internet terputus saat pertama kali membuka halaman, `GetRows()` mengembalikan list kosong dengan `sukses = false`. Dengan mengecek `if (!sukses) return list;`, `_assetsCache` dibiarkan tetap bernilai `null`. Hasilnya, ketika koneksi internet pulih dan pengguna membuka halaman kembali, aplikasi akan otomatis mencoba memuat ulang dari Supabase alih-alih terjebak dengan cache kosong!

### C. Algoritma Penomoran Kode Otomatis `NextCode()` ([L225-L231](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L225-L231))
```csharp
public static string NextCode(string prefix, IEnumerable<string> daftarKode)
{
    var terpakai = new HashSet<string>(daftarKode ?? Enumerable.Empty<string>(), StringComparer.OrdinalIgnoreCase);
    int nomor = 1;
    while (terpakai.Contains($"{prefix}{nomor:D3}")) nomor++;
    return $"{prefix}{nomor:D3}";
}
```
- Menggunakan `HashSet<string>` dengan perbandingan *case-insensitive* ($\mathcal{O}(1)$ *lookup*).
- Menghasilkan format 3 digit (`D3`): `AST-001`, `EMP-001`, `ALK-001` (dan otomatis berkembang ke 4 digit `AST-1000` jika mencapai $\ge 1000$).
- Jika ada kode di tengah yang dihapus (misal `AST-003` dihapus sementara `AST-004` ada), `NextCode` secara cerdas mengisi kembali nomor `AST-003` yang kosong tersebut.

### D. Pengiriman Request HTTP `Send()` ([L256-L286](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L256-L286))
- Menampilkan indikator loading melalui `UIHelper.ShowLoading(method == "GET" ? "Memuat data..." : "Menyimpan data...")`.
- Jika `body != null` (untuk `POST` dan `PATCH`), menambahkan header `Prefer: return=minimal` agar Supabase PostgREST tidak perlu mengirim ulang body baris yang baru disimpan (menghemat *bandwidth* dan mempercepat respons).
- Selalu memastikan `UIHelper.HideLoading()` dipanggil di dalam blok `finally`, baik ketika request berhasil maupun ketika terjadi eksepsi jaringan.
