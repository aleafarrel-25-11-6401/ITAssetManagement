---
title: "FormLogin & FormRegister"
aliases:
  - FormLogin.cs
  - FormRegister.cs
  - FormLogin
  - FormRegister
tags:
  - views
  - winforms
  - auth
  - login
  - register
layer: views
type: reference
updated: 2026-11-21
---

# 🔑 FormLogin & FormRegister

⬅️ Kembali ke [[MOC - Layer Views & UI]] | [[00 - Home]] | Lihat juga: [[AuthController]] · [[Alur Autentikasi, Registrasi, dan Sesi]]

> [!info] Lokasi File Kode Sumber
> - **FormLogin**: [Views/FormLogin.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/FormLogin.cs#L1-L92) & [Views/FormLogin.Designer.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/FormLogin.Designer.cs#L1-L140)
> - **FormRegister**: [Views/FormRegister.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/FormRegister.cs#L1-L57) & [Views/FormRegister.Designer.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/FormRegister.Designer.cs#L1-L202)

---

## 1. Spesifikasi `FormLogin`

`FormLogin` adalah jendela pertama yang dibuka oleh `Program.Main()`.
- **Properti Form**: `ClientSize = 806 x 539 px`, `FormBorderStyle = FixedSingle`, `MaximizeBox = false`, `StartPosition = CenterScreen`, `AcceptButton = btnLogin` (menekan tombol `Enter` di keyboard langsung mengeksekusi Login).

### Komponen Kontrol UI pada `FormLogin`
| Nama Kontrol | Tipe | Properti Penting | Fungsi |
| :--- | :--- | :--- | :--- |
| `pictureBox1` | `PictureBox` | `Size = 274 x 119`, `Zoom` | Menampilkan logo aplikasi pada halaman Login. |
| `txtUsername` | `TextBox` | `Location = (260, 250)` | Input username pengguna. |
| `txtPassword` | `TextBox` | `UseSystemPasswordChar = true` | Input password tersembunyi. |
| `btnLogin` | `Button` | `BackColor = RGB(128,128,255)` | Tombol utama untuk masuk ke sistem (`btnLogin_Click`). |
| `btnDaftar` | `Button` | `Text = "Belum punya akun? Daftar Akun"` | Membuka dialog modal `FormRegister` (`btnDaftar_Click`). |

### Method di `FormLogin.cs`
1. **`btnLogin_Click`** ([L20-L49](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/FormLogin.cs#L20-L49)):
   - Memvalidasi `username` dan `password` tidak kosong.
   - Mengubah teks tombol menjadi `"Memproses..."` dan `btnLogin.Enabled = false` selama request ke Supabase berlangsung.
   - Memanggil `_auth.Login(username, password)`.
   - Jika gagal: menampilkan `Dialogs.Error(result.Message, "Login Gagal")`, mengosongkan `txtPassword`, dan mengarahkan fokus kembali ke `txtPassword`.
   - Jika berhasil: memanggil `OpenMainForm()`.
2. **`btnDaftar_Click`** ([L52-L63](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/FormLogin.cs#L52-L63)):
   - Membuka `new FormRegister { Icon = Icon }` menggunakan `ShowDialog(this)`.
   - Jika pendaftaran sukses (`DialogResult.OK`), otomatis menyalin `formRegister.RegisteredUsername` ke `txtUsername.Text` dan memfokuskan kursor ke `txtPassword` agar pengguna tinggal mengetik password untuk login!
3. **`OpenMainForm` & `MainForm_FormClosed`** ([L66-L90](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/FormLogin.cs#L66-L90)):
   - Menyembunyikan `FormLogin` saat `MainForm` aktif, dan menampilkannya kembali jika pengguna menekan tombol **Logout** (`mainForm.IsLoggingOut == true`).

---

## 2. Spesifikasi `FormRegister`

`FormRegister` adalah form modal untuk mendaftarkan akun baru.
- **Properti Form**: `ClientSize = 806 x 539 px`, `FormBorderStyle = FixedSingle`, `StartPosition = CenterParent`, `AcceptButton = btnDaftar`.
- **Properti Publik**: `public string RegisteredUsername { get; private set; }` ([L15](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/FormRegister.cs#L15)).

### Komponen Kontrol UI pada `FormRegister`
| Nama Kontrol | Tipe | Properti Penting | Fungsi |
| :--- | :--- | :--- | :--- |
| `lblInfoRole` | `Label` | `"Akun baru otomatis terdaftar sebagai Staff IT."` | Memberi informasi jelas mengenai role default pendaftaran. |
| `txtUsername` | `TextBox` | `MaxLength = 50` | Input username login baru (**case-sensitive**, min. 3 karakter, tanpa spasi). |
| `txtNamaLengkap` | `TextBox` | `MaxLength = 100` | Input **Nama Pengguna (Nama Tampilan)** yang akan ditampilkan di aplikasi saat login (contoh: `"Budi Santoso"`). |
| `txtPassword` | `TextBox` | `UseSystemPasswordChar = true` | Input password baru (min. 6 karakter). |
| `txtKonfirmasi` | `TextBox` | `UseSystemPasswordChar = true` | Input konfirmasi password. |
| `btnDaftar` | `Button` | `Text = "Daftar Akun"` | Mengeksekusi `_auth.Register(username, namaLengkap, password, konfirmasi)` (`btnDaftar_Click`) dan mempertahankan huruf besar/kecil pada `RegisteredUsername = username`. |
| `btnKembali` | `Button` | `Text = "Kembali ke Login"` | Menutup dialog dengan `DialogResult.Cancel` (`btnKembali_Click`). |
