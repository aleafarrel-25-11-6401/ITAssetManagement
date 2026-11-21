---
title: "ADR-002: Strategi In-Memory Caching pada DataStore"
aliases:
  - ADR-002
  - In-Memory Caching
tags:
  - adr
  - arsitektur
  - cache
  - datastore
layer: adr
type: adr
status: Accepted
date: 2026-10-30
updated: 2026-11-15
---

# ADR-002: Strategi In-Memory Caching pada DataStore

⬅️ Kembali ke [[MOC - Arsitektur & Desain]] | [[00 - Home]] | Lihat juga: [[DataStore.cs (REST Client & In-Memory Cache)]] · [[Alur Sinkronisasi Cache & Denormalisasi Data]]

## Status
**Accepted**

## Date
2026-10-30 (Ditinjau ulang: 2026-11-15)

## Context
Server Supabase proyek ini berada di region Eropa Barat (`eu-west-1`), sehingga setiap kali aplikasi melakukan request HTTP `GET` dari Indonesia, terdapat latensi jaringan (*round-trip time*) sekitar `200–400 ms`. Jika setiap perpindahan tab menu di [[MainForm (Navigasi & RBAC UI)|MainForm]] atau setiap ketikan pencarian di kotak `txtCari` melakukan HTTP `GET` ulang ke Supabase, antarmuka pengguna akan terasa lambat dan membeku (*UI freeze*).

## Decision
Menerapkan pola **Lazy-Loaded Write-Through In-Memory Cache** menggunakan 4 koleksi `List<T>` statis di dalam [[DataStore.cs (REST Client & In-Memory Cache)|DataStore]] (`_assetsCache`, `_karyawanCache`, `_alokasiCache`, `_riwayatCache`) yang diimbangi penyegaran saat berpindah halaman:
1. Data diambil dari Supabase hanya pada pemanggilan pertama saat cache kosong (`_cache == null`), lalu penanda waktu `_cacheLoadedAt` dicatat oleh pembantu `SetCache<T>()` ([DataStore.cs#L73-L77](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L73-L77)).
2. Jika pemanggilan pertama gagal karena koneksi terputus (`!sukses`), cache **tidak** diisi dengan list kosong (`_cache` tetap `null` dan `_cacheLoadedAt` tetap `DateTime.MinValue`) agar percobaan berikutnya otomatis mengambil ulang dari server.
3. Setiap operasi mutasi (`Add`, `Update`, `Delete`, `Allocate`, `Return`, `ClearAll`) mengirim perubahan ke Supabase terlebih dahulu, lalu langsung memperbarui objek di dalam cache memori jika request HTTP berhasil sehingga operasi di halaman yang sama dan navigasi selama masa berlaku cache tetap sinkron.
4. **Masa Berlaku Cache (TTL 60 Detik) & Logout**: Saat pengguna berpindah halaman (`MainForm.ShowPage`, [MainForm.cs#L90](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/MainForm.cs#L90)), aplikasi memanggil `DataStore.ClearExpiredCache()` ([DataStore.cs#L65-L70](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L65-L70)) — cache dipertahankan selama umurnya `< CacheDuration` (60 detik) dan baru dikosongkan untuk memuat ulang snapshot terbaru server setelah melewati 60 detik. Saat pengguna menekan **Logout** (`MainForm.btnLogout_Click`, [MainForm.cs#L115](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/MainForm.cs#L115)), `DataStore.ClearCache()` tetap langsung mengosongkan keempat cache tanpa menunggu TTL.

## Alternatives Considered

### 1. Selalu Mengosongkan Cache pada Setiap Perpindahan Halaman (`ClearCache()` di `ShowPage`)
- **Pros**: Stok langsung segar setiap klik menu.
- **Cons**: Setiap perpindahan tab memicu `1–3` HTTP `GET` sinkron ke Supabase sehingga memunculkan overlay loading terus-menerus dan membuat cache memori nyaris tidak terpakai lintas menu.
- **Rejected**: Menggunakan TTL 60 detik (`ClearExpiredCache()`) menjaga navigasi tetap instan tanpa kehilangan penyegaran berkala.

### 2. Menyimpan Cache di Database Lokal (SQLite)
- **Pros**: Bertahan lintas restart aplikasi.
- **Cons**: Menambah kompleksitas sinkronisasi *two-way* dan membutuhkan library tambahan.
- **Rejected**: Cache di RAM (`List<T>`) per sesi dengan TTL 60 detik sudah sangat memadai untuk skala ribuan baris inventaris aset IT.

## Consequences
- **Positif**: Selama masa berlaku cache **60 detik**, perpindahan halaman antar menu (`Dashboard`, `Manajemen Aset`, `Karyawan`, `Alokasi`, `Riwayat`, `Profil`) mengambil data langsung dari memori RAM (bersama penyaringan kata kunci LINQ dan filter intra-halaman) tanpa memicu request HTTP `GET` atau overlay loading berulang; setelah 60 detik, perpindahan tab otomatis menyegarkan data dari Supabase.
- **Trade-off**: Perubahan dari pengguna/komputer lain maksimal baru terlihat saat pindah tab setelah melewati batas waktu 60 detik (atau langsung setelah Logout/Login ulang), dan setiap method mutasi di Controller wajib memperbarui cache RAM setelah `DataStore.Insert`/`Update`/`Delete` selesai (didokumentasikan pada [[Alur Sinkronisasi Cache & Denormalisasi Data]]).
