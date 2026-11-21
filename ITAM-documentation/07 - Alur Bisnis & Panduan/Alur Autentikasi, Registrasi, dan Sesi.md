---
title: "Alur Autentikasi, Registrasi, dan Sesi"
aliases:
  - Alur Autentikasi
  - Alur Login dan Register
  - Siklus Sesi Pengguna
tags:
  - alur-bisnis
  - auth
  - sequence-diagram
  - diataxis
layer: workflow
type: explanation
updated: 2026-11-15
---

# 🔐 Alur Autentikasi, Registrasi, dan Sesi

⬅️ Kembali ke [[MOC - Panduan & Alur Bisnis]] | [[00 - Home]] | Lihat juga: [[AuthController]] · [[Tabel app_users]] · [[FormLogin & FormRegister]] · [[UC_Profil]]

> [!info] Kuadran Diátaxis: **Explanation & How-to**
> Dokumen ini membedah secara menyeluruh bagaimana proses **Login**, **Registrasi Akun Baru**, **Ubah Profil (Username/Password)**, dan **Logout** bekerja mulai dari antarmuka pengguna hingga ke *trigger* keamanan di PostgreSQL Supabase.

---

## 1. Diagram Urutan (*Sequence Diagram*): Login & Registrasi

```mermaid
sequenceDiagram
    autonumber
    actor User as 👤 Pengguna
    participant FL as 🔑 FormLogin
    participant FR as 📝 FormRegister
    participant Auth as ⚙️ AuthController
    participant Store as 📦 DataStore
    participant DB as 🗄️ Supabase (public.app_users)

    rect rgb(239, 246, 255)
    Note over User,DB: A. ALUR REGISTRASI AKUN BARU
    User->>FL: Klik "Belum punya akun? Daftar Akun"
    FL->>FR: ShowDialog()
    User->>FR: Isi Username (Case-Sensitive), Nama Pengguna, Password, Konfirmasi & Klik "Daftar Akun"
    FR->>Auth: Register(username, namaLengkap, password, konfirmasi)
    Auth->>Auth: ValidateUsername() (Case-Sensitive) & ValidateNamaLengkap()
    Auth->>Store: FindUser(cleanUsername)
    Store->>DB: GET /rest/v1/app_users?username=eq.{cleanUsername}&limit=1
    DB-->>Store: [] (Belum terpakai)
    Auth->>Auth: HashPassword(password) -> SHA-256 (64 hex chars)
    Auth->>Store: Insert("app_users", { username, nama_lengkap, password: hash, role: "Staff" })
    Store->>DB: POST /rest/v1/app_users
    Note over DB: Trigger trg_enforce_app_users_security:<br/>1. btrim(username) (Tetap Case-Sensitive!)<br/>2. Paksa role='Staff' jika sudah ada Admin
    DB-->>Store: HTTP 201 Created
    Auth-->>FR: OperationResult.Ok("Akun berhasil didaftarkan!")
    FR-->>FL: DialogResult.OK (Auto-fill txtUsername sesuai case asli)
    end

    rect rgb(236, 253, 245)
    Note over User,DB: B. ALUR LOGIN (CASE-SENSITIVE) & PEMBUATAN SESI
    User->>FL: Ketik Password & Klik "Login"
    FL->>Auth: Login(username, password)
    Auth->>Store: FindUser(username)
    Store->>DB: GET /rest/v1/app_users?select=username,nama_lengkap,password,role&username=eq.{clean}&limit=1
    DB-->>Store: [{ username, nama_lengkap, password, role }]
    Auth->>Auth: Verifikasi StringComparison.Ordinal pada username & HashPassword(password)
    Auth->>Auth: UserSession.Current = new UserSession(savedUsername, role, namaLengkap)
    Auth-->>FL: OperationResult.Ok("Login berhasil!")
    FL->>FL: Hide() & new MainForm().Show() (Header menampilkan NamaLengkap/DisplayName)
    end
```

---

## 2. Diagram Urutan: Ubah Profil (Username/Nama Pengguna/Password) & Sinkronisasi Riwayat

Ketika pengguna mengganti **Username**, **Nama Pengguna** (`nama_lengkap`), atau **Password** di halaman [[UC_Profil]], aplikasi memperbarui [[Tabel app_users]], menyinkronkan kolom `oleh_user` di [[Tabel riwayat]], dan memperbarui teks header di [[MainForm (Navigasi & RBAC UI)|MainForm]]:

```mermaid
sequenceDiagram
    autonumber
    actor User as 👤 Pengguna Aktif
    participant UCP as 👤 UC_Profil
    participant Auth as ⚙️ AuthController
    participant Store as 📦 DataStore
    participant DB as 🗄️ Supabase (app_users & riwayat)
    participant MF as 🧭 MainForm

    User->>UCP: Isi Username Baru / Nama Pengguna / Password Baru + Password Saat Ini -> Klik "Simpan Perubahan"
    UCP->>Auth: UpdateProfile(currentUsername, newUsername, newNamaLengkap, currentPass, newPass, confirmPass)
    Auth->>Store: FindUser(cleanCurrent)
    Store->>DB: GET /rest/v1/app_users?username=eq.{cleanCurrent}
    Auth->>Auth: Verifikasi HashPassword(currentPass) == row["password"]
    alt Jika Username Diubah
        Auth->>Store: FindUser(cleanNewUsername) (Pastikan belum dipakai akun lain, StringComparison.Ordinal)
    end
    Auth->>Store: Update("app_users?username=eq." + savedCurrent, { username, nama_lengkap, password })
    Store->>DB: PATCH /rest/v1/app_users?username=eq.{savedCurrent}
    Note over DB: Trigger trg_enforce_app_users_security:<br/>Mengunci NEW.role := OLD.role
    alt Jika Nama Pengguna / Username Diubah
        Auth->>Store: RenameHistoryUser(savedNama, cleanNewNama) (+ savedCurrent bila berbeda)
        Store->>DB: PATCH /rest/v1/riwayat?oleh_user=eq.{old} ({ oleh_user: cleanNewNama })
        Store->>Store: Update item.OlehUser di _riwayatCache
        Auth->>Auth: Update UserSession.Current (Username & NamaLengkap)
    end
    Auth-->>UCP: OperationResult.Ok("Profil akun berhasil diperbarui!")
    UCP->>MF: RefreshProfileHeader() -> Perbarui lblProfil di kanan atas MainForm
```
