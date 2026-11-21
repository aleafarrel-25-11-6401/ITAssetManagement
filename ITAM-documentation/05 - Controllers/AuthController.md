---
title: "AuthController"
aliases:
  - AuthController.cs
  - Controller Autentikasi
tags:
  - controllers
  - auth
  - security
  - sha256
layer: controllers
type: reference
updated: 2026-11-21
---

# 🔐 AuthController (`Controllers/AuthController.cs`)

⬅️ Kembali ke [[MOC - Layer Controllers]] | [[00 - Home]] | Lihat juga: [[Tabel app_users]] · [[FormLogin & FormRegister]] · [[UC_Profil]] · [[Alur Autentikasi, Registrasi, dan Sesi]]

> [!info] Lokasi File Kode Sumber
> - **File**: [Controllers/AuthController.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AuthController.cs#L1-L229)
> - **Namespace**: `ITAssetManagement.Controllers`
> - **Class**: `public class AuthController`

---

## 1. Tanggung Jawab Utama

`AuthController` menangani seluruh siklus keamanan identitas pengguna aplikasi:
1. Menghasilkan hash kriptografi **SHA-256** (64 karakter hex huruf kecil) dari password pengguna.
2. Memvalidasi aturan penamaan `username` secara **case-sensitive** (wajib diisi, $\ge 3$ karakter, tanpa spasi, dan mempertahankan huruf besar/kecil apa adanya tanpa diubah ke huruf kecil).
3. Memvalidasi **Nama Pengguna (`nama_lengkap`)** yang akan ditampilkan di seluruh antarmuka aplikasi ($\ge 2$ karakter).
4. Memverifikasi kredensial secara **case-sensitive** saat **Login** (`StringComparison.Ordinal`) dan membuat sesi aktif (`UserSession.Current` berisi `Username`, `Role`, dan `NamaLengkap`).
5. Mendaftarkan akun pengguna baru saat **Register** ke [[Tabel app_users|public.app_users]].
6. Memperbarui `username` (*case-sensitive*), `nama_lengkap` (nama tampilan), dan/atau `password` pada halaman **Profil Akun** ([[UC_Profil]]) sekaligus menyinkronkan perubahan nama tampilan ke tabel [[Tabel riwayat|public.riwayat]].

---

## 2. Bedah Detail Setiap Method

### 1. `HashPassword(string password)` ([L13-L20](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AuthController.cs#L13-L20))
- **Visibilitas**: `public static string`
- **Algoritma**: Menggunakan `System.Security.Cryptography.SHA256.Create()` terhadap byte `Encoding.UTF8.GetBytes(password ?? "")`, lalu mengubah hasil byte array 32-byte menjadi string 64 karakter heksadesimal huruf kecil (`BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant()`).
- **Kesesuaian Database**: Output fungsi ini dijamin memenuhi constraint `app_users_password_hash_check` (`password ~ '^[0-9a-f]{64}$'`) pada [[Tabel app_users]].

### 2. `ValidateUsername` & `ValidateNamaLengkap` ([L23-L61](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AuthController.cs#L23-L61))
- **`ValidateUsername`**:
  1. `cleanUsername = (username ?? "").Trim();` *(Mempertahankan huruf besar/kecil sesuai input pengguna — tidak menggunakan `.ToLowerInvariant()`).*
  2. Jika panjang `0` $\rightarrow$ `"Username wajib diisi!"`
  3. Jika panjang `< 3` $\rightarrow$ `"Username minimal 3 karakter!"`
  4. Jika mengandung `" "` (spasi) $\rightarrow$ `"Username tidak boleh mengandung spasi!"`
- **`ValidateNamaLengkap`**:
  1. `cleanNama = (namaLengkap ?? "").Trim();`
  2. Jika panjang `0` $\rightarrow$ `"Nama Pengguna wajib diisi!"`
  3. Jika panjang `< 2` $\rightarrow$ `"Nama Pengguna minimal 2 karakter!"`

### 3. `Login(string username, string password)` ([L64-L96](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AuthController.cs#L64-L96))
- **Dipanggil oleh**: [[FormLogin & FormRegister|FormLogin.btnLogin_Click]] ([FormLogin.cs#L35](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/FormLogin.cs#L35))
- **Alur Eksekusi Case-Sensitive**:
  1. Cek input kosong $\rightarrow$ gagal jika `username` atau `password` kosong.
  2. Panggil `DataStore.FindUser(cleanUsername)` (`GET /rest/v1/app_users?select=username,nama_lengkap,password,role&username=eq.{cleanUsername}&limit=1`).
  3. Hitung `passwordHash = HashPassword(password)`.
  4. Bandingkan `savedUsername` dengan `cleanUsername` menggunakan **`StringComparison.Ordinal` (case-sensitive)** dan `row["password"]` dengan `passwordHash`. Jika username berbeda huruf besar/kecil (misal akun terdaftar `"Budi01"` namun login dengan `"budi01"`), login ditolak!
  5. Baca `nama_lengkap` dan `role` dari database.
  6. Simpan sesi ke `UserSession.Current = new UserSession(savedUsername, role, namaLengkap)` dan kembalikan `OperationResult.Ok("Login berhasil!")`.

### 4. `Register(string username, string namaLengkap, string password, string confirmPassword)` ([L99-L135](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AuthController.cs#L99-L135))
- **Dipanggil oleh**: [[FormLogin & FormRegister|FormRegister.btnDaftar_Click]] ([FormRegister.cs#L40](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/FormRegister.cs#L40))
- **Alur Eksekusi**:
  1. Validasi kelengkapan input, format `username` (`ValidateUsername`), `namaLengkap` (`ValidateNamaLengkap`), panjang `password >= 6`, dan kecocokan `password == confirmPassword`.
  2. Cek apakah `DataStore.FindUser(cleanUsername) != null`. Jika sudah ada, kembalikan `"Username sudah digunakan, silakan pilih username lain!"`. *(Catatan: `nama_lengkap` boleh sama dengan akun lain karena yang menjadi Primary Key dan target Foreign Key `riwayat.username_pelaku` adalah `username`)*.
  3. Panggil `DataStore.Insert("app_users", new { username = cleanUsername, nama_lengkap = cleanNama, password = HashPassword(password), role = "Staff" })`.
  4. Di sisi database, trigger `trg_enforce_app_users_security` mempertahankan huruf besar/kecil `username` (`btrim`) dan mengunci role menjadi `'Staff'` (atau `'Admin'` jika tabel kosong).

### 5. `UpdateProfile(string currentUsername, string newUsername, string newNamaLengkap, string currentPassword, string newPassword, string confirmNewPassword)` ([L138-L227](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Controllers/AuthController.cs#L138-L227))
- **Dipanggil oleh**: [[UC_Profil|UC_Profil.btnSimpan_Click]] ([UC_Profil.cs#L51](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Profil.cs#L51))
- **Fitur Fleksibel**: Pengguna dapat mengubah **username** (termasuk mengubah huruf besar/kecil seperti `"budi"` $\rightarrow$ `"Budi"` karena dicek dengan `StringComparison.Ordinal`), **nama pengguna tampilan (`NamaLengkap`)**, dan/atau **password**, dengan verifikasi `currentPassword`.
- **Alur Sinkronisasi Saat Nama Pengguna / Username Berubah**:
  1. Mengirim `PATCH /rest/v1/app_users?username=eq.{savedCurrent}` dengan `{ username = cleanNewUsername, nama_lengkap = cleanNewNama, password = finalPasswordHash }`. Di database, Foreign Key `riwayat_username_pelaku_fkey` (`ON UPDATE CASCADE`) otomatis memperbarui `riwayat.username_pelaku` jika `username` berubah, dan trigger `trg_sync_app_users_nama_to_riwayat` memperbarui `riwayat.oleh_user` jika `nama_lengkap` berubah.
  2. Memanggil `DataStore.RenameHistoryUser(savedNama, cleanNewNama)` (serta untuk `savedCurrent` bila berbeda) agar `oleh_user` pada `_riwayatCache` di memori ikut tersinkronisasi.
  3. Memperbarui `UserSession.Current.Username = cleanNewUsername` dan `UserSession.Current.NamaLengkap = cleanNewNama` sehingga header profil di [[MainForm (Navigasi & RBAC UI)|MainForm]] langsung menampilkan nama pengguna baru tanpa perlu login ulang!
