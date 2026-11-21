---
title: "ADR-007 - Relasi Riwayat ke Pengguna dan Kontak Karyawan Unik"
aliases:
  - ADR-007
  - Relasi Riwayat ke app_users
  - Kontak Karyawan Unik
tags:
  - adr
  - database
  - schema
  - integritas-data
  - unit-testing
layer: adr
type: adr
status: Accepted
date: 2026-11-21
updated: 2026-11-21
---

# ADR-007: Relasi `riwayat` ke `app_users` dan Kontak Karyawan Unik

⬅️ Kembali ke [[MOC - Arsitektur & Desain]] | [[MOC - Database Supabase]] | Lihat juga: [[Skema Database & ERD Lengkap]] · [[Tabel riwayat]] · [[Tabel app_users]] · [[Tabel karyawan]] · [[ADR-003 - Penggunaan Natural Primary Key dan Denormalisasi Terkendali pada Tabel Alokasi]]

## Status
**Accepted**

## Date
2026-11-21

## Context
1. **Syarat tugas**: minimal 4 tabel harus saling berelasi. Sebelumnya hanya 3 tabel yang punya Foreign Key (`assets`, `karyawan`, `alokasi`); `riwayat` dan `app_users` berdiri sendiri. Aplikasi sudah cukup dengan 3 tabel tersebut, sehingga relasi tambahan tidak boleh mengubah isi tabel maupun perilaku aplikasi.
2. **Kontak karyawan bisa kembar**: kolom `karyawan.kontak` (nomor telepon atau email) sebelumnya tidak punya aturan keunikan di database maupun di aplikasi. Tidak ada kolom `email` terpisah; `kontak` menampung keduanya.
3. **Batas dari ADR-003**: `riwayat` tidak boleh memakai Foreign Key ke `assets` atau `karyawan`, karena log penghapusan aset harus tetap ada setelah asetnya dihapus.
4. **Kondisi kode**: `riwayat.oleh_user` sudah berisi `UserSession.DisplayName`, yaitu `app_users.nama_lengkap`; `AuthController.UpdateProfile` sudah menyinkronkannya secara manual lewat `DataStore.RenameHistoryUser`.

## Decision
1. **Foreign Key `riwayat.username_pelaku` → `app_users.username` (Primary Key)** (`ON UPDATE CASCADE ON DELETE RESTRICT`), didukung index `riwayat_username_pelaku_idx` serta trigger `trg_sync_riwayat_pelaku` dan `trg_sync_app_users_nama_to_riwayat`:
   - Dengan merujuk langsung ke Primary Key `app_users(username)` (bukan `nama_lengkap`), **`app_users.nama_lengkap` tidak menjadi `UNIQUE`** sehingga dua pengguna yang berbeda tetap bebas memakai nama lengkap yang sama.
   - Kolom `riwayat.oleh_user` tetap menyimpan nama tampilan (`nama_lengkap`) agar tampilan tabel di `UC_Riwayat` dan `UC_Dashboard` tidak berubah.
   - Saat aplikasi C# mengirim `INSERT INTO public.riwayat`, trigger `trg_sync_riwayat_pelaku` otomatis mengisi `username_pelaku` dari `app_users`.
2. **Kontak karyawan unik lewat Partial Functional `UNIQUE INDEX` (`karyawan_kontak_unique_idx`) dan Trigger (`trg_karyawan_kontak_unique`)**: menolak `INSERT` atau perubahan `kontak` yang sama dengan karyawan lain setelah spasi, titik, kurung, strip, dan huruf besar/kecil diabaikan. `-` (kontak kosong) dikecualikan.
3. **Constraint tambahan** `karyawan_not_blank_check`: `nama`, `divisi`, `jabatan`, `kontak` tidak boleh kosong atau spasi saja.
4. **Penjagaan di aplikasi** (dialog sebelum permintaan dikirim, tanpa fitur atau layar baru):
   - `KaryawanController.Add`/`Update` menolak kontak yang dipakai karyawan lain dengan pesan `"Nomor telepon/email kontak sudah digunakan karyawan lain!"`.
   - `AuthController.Register`/`UpdateProfile` hanya mengecek keunikan `username` (via `DataStore.FindUser`), sedangkan `nama_lengkap` bebas sama antar akun.
5. **Pengujian di luar kode produksi**: proyek NUnit `ITAssetManagement.Tests` (di solution yang sama, tetapi folder terpisah) mereferensikan aplikasi utama dan memakai server HTTP palsu lokal, sehingga tidak ada kode test, mode debug, atau perubahan visibilitas di proyek utama (sejalan dengan amandemen [[ADR-006 - Manajemen Stok Aset Multi-Unit, Empty State UI, dan Unit Testing]]).

## Alternatives Considered

### 1. Foreign Key `riwayat.kode_aset` → `assets.kode_aset` atau `riwayat.nama_karyawan` → `karyawan`
- Pros: relasi lebih "bermakna" secara bisnis.
- Cons: `ON DELETE CASCADE` menghapus log audit saat aset dihapus; `RESTRICT` memblokir penghapusan aset; `nama_karyawan` juga tidak unik.
- **Rejected**: melanggar tujuan `riwayat` sebagai log permanen ([[ADR-003 - Penggunaan Natural Primary Key dan Denormalisasi Terkendali pada Tabel Alokasi]]).

### 2. Foreign Key `riwayat.oleh_user` → `app_users.nama_lengkap`
- Pros: tidak menambah kolom di `riwayat`.
- Cons: mewajibkan `UNIQUE (nama_lengkap)` pada `app_users`, padahal di dunia nyata dua pengguna bisa memiliki nama lengkap yang sama, dan di diagram skema membuat `nama_lengkap` ditandai sebagai kunci unik.
- **Rejected (diperbaiki di Migrasi 13)**: diganti dengan `riwayat.username_pelaku` → `app_users.username` (Primary Key).

### 3. Hanya validasi di aplikasi (tanpa constraint/index/trigger di database)
- Cons: tidak melindungi dari klien lain, cache basi, atau akses REST langsung (kunci ada di `App.config`).
- **Rejected**: aturan keunikan harus dijaga di database (`karyawan_kontak_unique_idx` + `trg_karyawan_kontak_unique`); validasi aplikasi bertugas menampilkan pesan yang ramah.

## Consequences
- **Positif**: 5 tabel kini berelasi lewat Foreign Key; `app_users.nama_lengkap` tetap bebas sama antar pengguna karena FK `riwayat.username_pelaku` merujuk ke Primary Key `app_users.username`; kontak tidak bisa kembar; mengganti username atau nama lengkap otomatis menyinkronkan riwayat di database; semua perubahan diuji lewat 21 test NUnit.
- **Negatif / batasan yang diketahui**:
  - Pada kasus langka cache aplikasi basi, penolakan database tampil sebagai pesan umum "Periksa koneksi internet Anda" karena `DataStore.Send` tidak meneruskan alasan penolakan server.
  - Risiko keamanan RLS `true` untuk `anon` tidak ditangani di sini; lihat [[Keamanan RLS, Trigger, dan Fungsi Database]] bagian 4.

## Verifikasi (2026-11-21)
- *Dry-run* migrasi di dalam transaksi yang di-`ROLLBACK`, lalu migrasi diterapkan.
- Uji SQL (11 skenario, semuanya sesuai harapan): kontak kembar berbeda format ditolak, `-` ganda diterima, email beda huruf ditolak, mengubah kontak ke milik orang lain ditolak, nama spasi ditolak, pelaku fiktif ditolak, *cascade rename* berjalan, menghapus pengguna yang punya riwayat ditolak.
- 21 test NUnit lulus (`dotnet test`, lihat [[Panduan Instalasi & Menjalankan Aplikasi]]).
- Supabase Advisors: tidak ada temuan keamanan baru.

## Change Log
- 2026-11-21: ADR dibuat bersamaan dengan Migrasi 12.
