---
title: "MainForm (Navigasi & RBAC UI)"
aliases:
  - MainForm.cs
  - MainForm
  - Navigasi Utama
tags:
  - views
  - winforms
  - mainform
  - rbac
layer: views
type: reference
updated: 2026-11-15
---

# 🧭 MainForm (Navigasi & RBAC UI)

⬅️ Kembali ke [[MOC - Layer Views & UI]] | [[00 - Home]] | Lihat juga: [[Program.cs & Siklus Hidup Aplikasi]] · [[FormLogin & FormRegister]] · [[Matriks Hak Akses (RBAC Admin vs Staff)]]

> [!info] Lokasi File Kode Sumber
> - **Logic**: [Views/MainForm.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/MainForm.cs#L1-L120)
> - **Designer**: [Views/MainForm.Designer.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/MainForm.Designer.cs#L1-L238)
> - **Ukuran Jendela**: `ClientSize = 1118 x 645 px` (`MinimumSize = 980 x 620 px`), posisi `CenterScreen`.

---

## 1. Struktur Tata Letak (`Layout Architecture`)

`MainForm` dibagi menjadi 3 panel kontainer utama:

1. **`pnlSidebar` (Dock: Left, Lebar: `225 px`)**:
   - Memuat logo perusahaan di bagian atas (`pictureBox1`, tinggi `120 px`, `SizeMode = Zoom`).
   - Memuat 6 tombol menu navigasi (`_menus`):
     - `btnDashboard` ("Dashboard")
     - `btnAsset` ("Manajemen Aset") — *Disembunyikan jika Staff*
     - `btnKaryawan` ("Karyawan") — *Disembunyikan jika Staff*
     - `btnAlokasi` ("Alokasi")
     - `btnRiwayat` ("Riwayat")
     - `btnProfil` ("Profil Akun")
   - Memuat tombol `btnLogout` ("Logout") berwarna merah (`RGB: 239, 68, 68`) yang di-anchor di bagian bawah sidebar (`Bottom | Left | Right`).
2. **`pnlHeader` (Dock: Top, Tinggi: `60 px`)**:
   - `lblHalaman` (Kiri): Menampilkan judul halaman aktif sesuai teks tombol menu yang sedang dipilih.
   - `lblProfil` (Kanan, `Dock = Right`): Menampilkan identitas pengguna aktif dari `UserSession.Current.ProfileText` (contoh: `"Administrator IT (Admin IT)"` atau `"Staff Operasional IT (Staff IT)"`).
3. **`pnlHalaman` (Dock: Fill, Area: `893 x 585 px`)**:
   - Kontainer tempat objek `UserControl` halaman aktif dimuat secara dinamis (`page.Dock = DockStyle.Fill`).

---

## 2. Bedah Detail Method di `MainForm.cs`

| Method | Baris Kode | Penjelasan Teknis |
| :--- | :---: | :--- |
| `MainForm()` *(Constructor)* | [L17-L29](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/MainForm.cs#L17-L29) | Menginisialisasi array `_menus`, memanggil `RefreshProfileHeader()`, menyusun menu sesuai `user.IsAdmin` melalui `ArrangeMenus()`, dan membuka halaman awal `ShowPage(btnDashboard)`. |
| `RefreshProfileHeader()` | [L32-L38](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/MainForm.cs#L32-L38) | Mengatur `lblProfil.Text = UserSession.Current.ProfileText`. Dipanggil saat inisialisasi dan dipanggil ulang oleh [[UC_Profil]] ketika pengguna berhasil memperbarui nama pengguna atau username. |
| `ArrangeMenus(bool isAdmin)` | [L41-L56](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/MainForm.cs#L41-L56) | Mengatur visibilitas tombol menu (`tampilkan = isAdmin \|\| (menu != btnAsset && menu != btnKaryawan)`). Untuk menu yang tampil, posisinya dirapatkan ke atas mulai dari `posisiY = 126` dengan jarak antar tombol `menu.Height + 6` (`48 px`) agar tidak ada celah kosong di sidebar Staff! |
| `CreatePage(Button menu)` | [L59-L67](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/MainForm.cs#L59-L67) | *Factory method* yang mengembalikan instance baru dari [[UC_Asset]], [[UC_Karyawan]], [[UC_Alokasi]], [[UC_Riwayat]], [[UC_Profil]], atau [[UC_Dashboard]]. |
| `ShowPage(Button activeMenu)` | [L70-L94](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/MainForm.cs#L70-L94) | 1. Memanggil `UIHelper.CancelDebounce()`.<br>2. Mengubah `lblHalaman.Text`.<br>3. Mewarnai tombol menu aktif dengan warna ungu indigo (`RGB: 128, 128, 255`, teks putih) dan tombol tidak aktif menjadi putih.<br>4. Memanggil `Dispose()` pada kontrol halaman sebelumnya di `pnlHalaman` agar tidak terjadi kebocoran memori (*memory leak* GDI handle).<br>5. Memanggil `DataStore.ClearExpiredCache()` sehingga selama `< 60 detik` navigasi memakai cache RAM instan, dan setelah kedaluwarsa (`>= 60 detik`) otomatis menyegarkan data dari server.<br>6. Memasang `UserControl` baru dengan `DockStyle.Fill`. |
| `btnMenu_Click(object sender, EventArgs e)` | [L97-L104](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/MainForm.cs#L97-L104) | *Shared event handler* untuk keenam tombol menu sidebar; meneruskan tombol yang diklik ke `ShowPage(clickedMenu)`. |
| `btnLogout_Click(object sender, EventArgs e)` | [L107-L118](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/MainForm.cs#L107-L118) | Meminta konfirmasi melalui `Dialogs.Confirm(...)`, membatalkan debounce, memanggil `DataStore.ClearCache()`, menandai `IsLoggingOut = true`, dan memanggil `Close()` untuk kembali ke [[FormLogin & FormRegister|FormLogin]]. |
