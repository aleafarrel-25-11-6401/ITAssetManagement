---
title: "ADR-003: Penggunaan Natural Primary Key dan Denormalisasi Terkendali pada Tabel Alokasi"
aliases:
  - ADR-003
  - Natural Primary Key
  - Denormalisasi Alokasi
tags:
  - adr
  - database
  - schema
  - denormalisasi
layer: adr
type: adr
status: Accepted
date: 2026-11-15
updated: 2026-11-15
---

# ADR-003: Penggunaan Natural Primary Key dan Denormalisasi Terkendali pada Tabel Alokasi

⬅️ Kembali ke [[MOC - Arsitektur & Desain]] | [[MOC - Database Supabase]] | Lihat juga: [[Skema Database & ERD Lengkap]] · [[Tabel alokasi]] · [[Alur Sinkronisasi Cache & Denormalisasi Data]]

## Status
**Accepted**

## Date
2026-11-15

## Context
Dalam domain bisnis manajemen inventaris IT:
1. Setiap entitas utama memiliki kode identifikasi bisnis yang unik dan bermakna bagi manusia (*Natural Key*): `username` pada `app_users`, `kode_aset` (`AST-001`) pada `assets`, `nik` (`EMP-001`) pada `karyawan`, dan `id_alokasi` (`ALK-001`) pada `alokasi`.
2. Tabel transaksi [[Tabel alokasi|public.alokasi]] ditampilkan secara langsung pada `DataGridView` di [[UC_Dashboard]] dan [[UC_Alokasi]], yang membutuhkan informasi `nama_aset`, `nama_karyawan`, dan `divisi` di samping `kode_aset` dan `nik_karyawan`.

## Decision
1. **Natural Primary Key tanpa Duplikasi Index**: Menetapkan `username`, `kode_aset`, `nik`, dan `id_alokasi` sebagai `PRIMARY KEY` pada masing-masing tabel (`app_users`, `assets`, `karyawan`, `alokasi`), sambil tetap mempertahankan kolom `id bigint generated always as identity` sebagai nomor urut baris internal. Menghapus constraint `UNIQUE` terpisah pada kolom-kolom tersebut agar PostgreSQL tidak memelihara dua B-Tree index identik pada kolom yang sama.
2. **Foreign Key dengan `ON UPDATE CASCADE ON DELETE CASCADE`**: Menghubungkan `alokasi.kode_aset` $\rightarrow$ `assets.kode_aset` dan `alokasi.nik_karyawan` $\rightarrow$ `karyawan.nik` dengan aturan `CASCADE`, didukung oleh aturan bisnis di Controller yang melarang penghapusan aset/karyawan selama masih ada pinjaman berstatus `'Aktif'`.
3. **Denormalisasi Terkendali (*Controlled Denormalization*) pada `alokasi` dan *Historical Snapshot* pada `riwayat`**: Menyimpan `nama_aset`, `nama_karyawan`, dan `divisi` di tabel `alokasi` serta menyinkronkannya secara otomatis ketika Admin memperbarui data master di [[AssetController]] atau [[KaryawanController]].

## Alternatives Considered

### 1. Normalisasi Murni 3NF Tanpa Menyimpan `nama_aset` / `nama_karyawan` di `alokasi`
- **Pros**: Tidak perlu sinkronisasi `PATCH /alokasi` saat nama aset atau nama karyawan diubah.
- **Cons**: Membutuhkan *nested join query* PostgREST (`select=...,assets(nama_aset),karyawan(nama,divisi)`) yang menghasilkan objek JSON bersarang dan lebih rumit dipetakan ke model datar `AlokasiItem` untuk `DataGridView`.
- **Rejected**: Denormalisasi terkendali membuat pembacaan `GetAlokasi()` sangat cepat dan struktur JSON PostgREST tetap datar (*flat*) serta konsisten dengan tabel lainnya.

### 2. Memasang Foreign Key Fisik pada Tabel `riwayat`
- **Pros**: Menjamin referensial ke `assets` dan `karyawan`.
- **Cons**: Jika aset yang sudah rusak dihapus dari master `assets`, log audit historis di `riwayat` akan ikut terhapus (jika `CASCADE`) atau memblokir penghapusan aset (jika `RESTRICT`).
- **Rejected**: Tabel `riwayat` adalah catatan sejarah (*audit log*) sehingga harus menyimpan *snapshot* teks secara mandiri.

## Consequences
- **Positif**: Tidak ada peringatan *Duplicate Index* pada **Supabase Performance Advisors**.
- **Positif**: Query `SELECT` pada `alokasi` dan `riwayat` bebas dari *join* berat.
- **Positif**: Integritas nama aset, nama karyawan, divisi, dan pelaku riwayat (`oleh_user`) tetap terjaga 100% melalui sinkronisasi otomatis di `AssetController.Update`, `KaryawanController.Update`, dan `AuthController.UpdateProfile`.

## Change Log
- 2026-11-21: Penegasan, keputusan di atas tentang `riwayat` tanpa Foreign Key berlaku untuk `assets` dan `karyawan`. Kolom `oleh_user` kini memiliki Foreign Key ke `app_users.nama_lengkap` ([[ADR-007 - Relasi Riwayat ke Pengguna dan Kontak Karyawan Unik]]), sehingga sinkronisasi pelaku riwayat dijaga juga oleh `ON UPDATE CASCADE` di database.
