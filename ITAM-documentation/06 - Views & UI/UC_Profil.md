---
title: "UC_Profil"
aliases:
  - UC_Profil.cs
  - Halaman Profil Akun
tags:
  - views
  - usercontrol
  - profil
  - auth
layer: views
type: reference
updated: 2026-11-15
---

# 👤 UC_Profil (`Views/UC_Profil.cs`)

⬅️ Kembali ke [[MOC - Layer Views & UI]] | [[00 - Home]] | Lihat juga: [[AuthController]] · [[MainForm (Navigasi & RBAC UI)]] · [[Tabel app_users]]

> [!info] Lokasi File Kode Sumber
> - **Logic**: [Views/UC_Profil.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Profil.cs#L1-L79)
> - **Designer**: [Views/UC_Profil.Designer.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Profil.Designer.cs#L1-L324)
> - **Hak Akses**: Tersedia untuk **Admin IT** dan **Staff IT**.

---

## 1. Struktur Komponen Visual

Halaman `UC_Profil` terdiri atas 2 `GroupBox`:

### A. `grpInfo` ("Informasi Akun Saat Ini" — 3 Kolom)
- `txtUsernameAktif` (`ReadOnly = true`): Menampilkan username login aktif (`UserSession.Current.Username`, *case-sensitive*).
- `txtNamaAktif` (`ReadOnly = true`): Menampilkan **Nama Pengguna (Tampil)** yang sedang aktif (`UserSession.Current.DisplayName`).
- `txtRoleAktif` (`ReadOnly = true`): Menampilkan `"Admin IT"` atau `"Staff IT"`.

### B. `grpUbah` ("Form Ubah Username, Nama Pengguna & Password")
| Nama Kontrol | Tipe | Konfigurasi | Fungsi |
| :--- | :--- | :--- | :--- |
| `txtUsernameBaru` | `TextBox` | `MaxLength = 50` | Diisi awal dengan username aktif; mendukung perubahan huruf besar/kecil (*case-sensitive*, misal `"budi"` $\rightarrow$ `"Budi"`). |
| `txtNamaBaru` | `TextBox` | `MaxLength = 100` | Diisi awal dengan Nama Pengguna (`DisplayName`); digunakan untuk mengubah nama tampilan pengguna di aplikasi. |
| `txtPasswordLama` | `TextBox` | `UseSystemPasswordChar = true` | **Wajib diisi** untuk mengonfirmasi identitas pemilik akun sebelum perubahan disimpan. |
| `txtPasswordBaru` | `TextBox` | `UseSystemPasswordChar = true` | Password baru (minimal 6 karakter; boleh dikosongkan jika hanya ingin mengubah username atau nama pengguna). |
| `txtKonfirmasiPassword` | `TextBox` | `UseSystemPasswordChar = true` | Konfirmasi password baru. |
| `btnSimpan` | `Button` | Ungu `(79, 70, 229)` | Menyimpan perubahan ke Supabase melalui `_authCtrl.UpdateProfile(...)` (`btnSimpan_Click`). |
| `btnReset` | `Button` | Standar | Mengembalikan isi form ke kondisi awal (`btnReset_Click`). |

---

## 2. Alur Pembaruan Header `MainForm` secara Langsung ([L47-L71](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Profil.cs#L47-L71))

Saat `btnSimpan_Click` dieksekusi dan `_authCtrl.UpdateProfile(...)` mengembalikan `result.Success == true`:
```csharp
MainForm mainForm = FindForm() as MainForm;
if (mainForm != null)
{
    mainForm.RefreshProfileHeader();
}
ResetForm();
```
Dengan memanggil `mainForm.RefreshProfileHeader()`, teks `lblProfil` di pojok kanan atas [[MainForm (Navigasi & RBAC UI)|MainForm]] dan `txtUsernameAktif` pada `UC_Profil` langsung diperbarui secara *real-time* mengikuti username baru.
