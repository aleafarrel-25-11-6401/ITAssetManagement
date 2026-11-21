---
title: "ADR-001: Komunikasi Langsung Supabase PostgREST Tanpa SDK Eksternal"
aliases:
  - ADR-001
  - Tanpa SDK Supabase
tags:
  - adr
  - arsitektur
  - supabase
  - postgrest
  - ponytail
layer: adr
type: adr
status: Accepted
date: 2026-10-29
updated: 2026-11-15
---

# ADR-001: Komunikasi Langsung Supabase PostgREST Tanpa SDK Eksternal

⬅️ Kembali ke [[MOC - Arsitektur & Desain]] | [[00 - Home]] | Lihat juga: [[DataStore.cs (REST Client & In-Memory Cache)]] · [[App.config & Proyek (.csproj)]]

## Status
**Accepted**

## Date
2026-10-29 (Ditinjau ulang: 2026-11-15)

## Context
Aplikasi **IT Asset Management** dibangun menggunakan **.NET Framework 4.7.2 (Windows Forms)** dengan backend database cloud **Supabase**. Kami membutuhkan cara yang andal, cepat, mudah di-build di komputer laboratorium/pengembangan mana pun, dan bebas dari masalah kompatibilitas paket NuGet saat berkomunikasi dengan tabel-tabel di Supabase (`app_users`, `assets`, `karyawan`, `alokasi`, `riwayat`).

## Decision
Menggunakan **`System.Net.Http.HttpClient`** dan **`System.Web.Script.Serialization.JavaScriptSerializer`** bawaan `.NET Framework 4.7.2` di dalam satu kelas terpusat [[DataStore.cs (REST Client & In-Memory Cache)|DataStore]] untuk memanggil endpoint REST API **Supabase PostgREST** (`/rest/v1/*`) secara langsung tanpa menginstal paket NuGet pihak ketiga.

## Alternatives Considered

### 1. Menggunakan NuGet `supabase-csharp` Resmi
- **Pros**: Menyediakan fluent query builder C#.
- **Cons**: Menarik puluhan paket dependensi turunan (`Newtonsoft.Json`, `Postgrest-csharp`, `Gotrue-csharp`, `Realtime-csharp`, `System.Reactive`, dll.) yang sering menimbulkan konflik `bindingRedirect` pada proyek `.NET Framework 4.7.2` klasik dan memerlukan `nuget restore` sebelum kompilasi.
- **Rejected**: Terlalu berat (*bloat*) untuk kebutuhan CRUD 5 tabel yang lugas (melanggar prinsip kesederhanaan **Ponytail / YAGNI**).

### 2. Menggunakan `Newtonsoft.Json` (Json.NET) + `HttpClient`
- **Pros**: Deserialisasi JSON populer di ekosistem .NET.
- **Cons**: Tetap membutuhkan paket NuGet eksternal dan file `packages.config` / `Newtonsoft.Json.dll` di folder output.
- **Rejected**: Assembly standar `System.Web.Extensions` di `.NET Framework 4.7.2` sudah menyediakan `JavaScriptSerializer` yang mampu men-serialize objek anonim (`new { ... }`) dan men-deserialize respons PostgREST ke `List<Dictionary<string, object>>` dalam kurang dari 20 baris kode.

## Consequences
- **Positif**: Proyek memiliki **0 dependensi NuGet eksternal** — dapat langsung di-clone dan dikompilasi (`MSBuild`) di komputer Windows mana pun yang memiliki `.NET Framework 4.7.2` tanpa perlu koneksi internet untuk *package restore*.
- **Positif**: Ukuran hasil kompilasi (`bin/Debug`) sangat ringan (hanya 1 file `.exe` dan `.exe.config`).
- **Positif**: Penggunaan header `Prefer: return=minimal` pada request `POST` dan `PATCH` di `DataStore.Send()` menghemat ukuran payload balikan HTTP secara signifikan.
- **Trade-off**: Query filter ditulis menggunakan sintaks URL query string PostgREST (seperti `?kode_aset=eq.AST-001` dengan `Uri.EscapeDataString`), sehingga seluruh pemanggilan diisolasi secara rapi di dalam `DataStore` dan `Controllers`.
