---
title: "ADR-004: Arsitektur Autentikasi Kustom SHA-256 dan Proteksi Trigger Privilege Escalation"
aliases:
  - ADR-004
  - Autentikasi SHA-256
  - Trigger Keamanan app_users
tags:
  - adr
  - security
  - auth
  - trigger
  - supabase
layer: adr
type: adr
status: Accepted
date: 2026-11-15
updated: 2026-11-15
---

# ADR-004: Arsitektur Autentikasi Kustom SHA-256 dan Proteksi Trigger Privilege Escalation

⬅️ Kembali ke [[MOC - Arsitektur & Desain]] | [[MOC - Database Supabase]] | Lihat juga: [[Tabel app_users]] · [[Keamanan RLS, Trigger, dan Fungsi Database]] · [[AuthController]]

## Status
**Accepted**

## Date
2026-11-15

## Context
Aplikasi membutuhkan sistem autentikasi berbasis `username` sederhana (seperti `"admin"`, `"staff"`, `"budi"`) tanpa mewajibkan pengguna memasukkan alamat email atau melakukan verifikasi kotak masuk email. Namun, karena aplikasi desktop mendistribusikan `SupabaseKey` (*Publishable Key*) di dalam `App.config`, tabel `public.app_users` harus dilindungi dari risiko:
1. Penyimpanan password dalam bentuk teks biasa (*plaintext*).
2. Penghapusan akun pengguna secara sembarangan melalui REST API.
3. Pendaftaran akun baru yang memalsukan kolom `"role": "Admin"` atau pembaruan profil yang menaikkan hak akses dari `"Staff"` menjadi `"Admin"` (*Privilege Escalation*).

## Decision
Menerapkan arsitektur pertahanan berlapis (*Defense-in-Depth*) antara [[AuthController]] di C# dan mesin PostgreSQL di Supabase:
1. **Client-Side SHA-256 Hashing + Server-Side Regex Constraint**:
   - `AuthController.HashPassword()` mengubah password menjadi hash SHA-256 64 karakter hex huruf kecil sebelum dikirim melalui jaringan.
   - Database menolak segala nilai `password` yang bukan hash SHA-256 menggunakan constraint `CHECK (password::text ~ '^[0-9a-f]{64}$'::text)` dan policy RLS `WITH CHECK`.
2. **Pencabutan Hak `DELETE` pada `public.app_users`**:
   - Role `anon` dan `authenticated` hanya diberi hak `SELECT`, `INSERT`, dan `UPDATE` pada `public.app_users` (tanpa `DELETE` grant dan tanpa policy RLS `DELETE`).
3. **Database Trigger `trg_enforce_app_users_security` (`enforce_app_users_security()`) & Pemisahan `username` (Case-Sensitive) vs `nama_lengkap`**:
   - Mempertahankan huruf besar/kecil pada `username` (`NEW.username := btrim(NEW.username)` tanpa `lower()`) sehingga login bersifat **case-sensitive** penuh (diperkuat dengan perbandingan `StringComparison.Ordinal` di `AuthController` dan `DataStore.FindUser`).
   - Memisahkan antara `username` (sebagai ID kredensial login) dan `nama_lengkap` (`UserSession.DisplayName`, minimal 2 karakter) sebagai nama pengguna yang ditampilkan pada header `MainForm` dan catatan `riwayat.oleh_user`.
   - Saat `INSERT`: Memeriksa apakah sudah ada Admin di `public.app_users`. Jika sudah ada, memaksa `NEW.role := 'Staff'`. Jika belum ada Admin sama sekali, akun pertama otomatis menjadi `'Admin'`.
   - Saat `UPDATE`: Mengunci `NEW.role := OLD.role` sehingga kolom `role` tidak pernah bisa diubah dari sisi client.

## Alternatives Considered

### 1. Menggunakan Supabase Auth (GoTrue) Berbasis Email & JWT
- **Pros**: Mengelola sesi JWT secara penuh di server.
- **Cons**: Berorientasi pada alamat email (`email + password`), membutuhkan konfigurasi SMTP/non-konfirmasi email, dan menambah kompleksitas manajemen refresh token di aplikasi desktop WinForms.
- **Rejected**: Autentikasi berbasis `username` pada `public.app_users` yang diperkuat oleh trigger PostgreSQL jauh lebih sesuai dengan kebutuhan operasional internal kampus/perusahaan.

### 2. Mengandalkan Validasi `role = "Staff"` Hanya di Kode C# (`AuthController.Register`)
- **Pros**: Tidak perlu menulis fungsi trigger PL/pgSQL.
- **Cons**: Sangat rentan jika seseorang membaca `SupabaseKey` dari `App.config` dan memanggil endpoint REST API `/rest/v1/app_users` secara langsung di luar aplikasi.
- **Rejected**: Keamanan otorisasi tidak boleh hanya bergantung pada kode client.

## Consequences
- **Positif**: Sekalipun endpoint PostgREST dipanggil langsung menggunakan alat eksternal (seperti Postman/`curl`), tidak ada pihak yang bisa membuat akun Admin tambahan, menaikkan role Staff menjadi Admin, menyimpan password plaintext, ataupun menghapus akun pengguna.
- **Positif**: Pengalaman pengguna (UX) tetap sangat sederhana — cukup menggunakan `username` (minimal 3 karakter tanpa spasi) dan `password` (minimal 6 karakter).
