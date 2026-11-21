---
title: "UC_Karyawan"
aliases:
  - UC_Karyawan.cs
  - Halaman Karyawan
tags:
  - views
  - usercontrol
  - karyawan
  - crud
layer: views
type: reference
updated: 2026-11-15
---

# 🧑‍💼 UC_Karyawan (`Views/UC_Karyawan.cs`)

⬅️ Kembali ke [[MOC - Layer Views & UI]] | [[00 - Home]] | Lihat juga: [[KaryawanController]] · [[Tabel karyawan]] · [[UIHelper & Dialogs]]

> [!info] Lokasi File Kode Sumber
> - **Logic**: [Views/UC_Karyawan.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Karyawan.cs#L1-L131)
> - **Designer**: [Views/UC_Karyawan.Designer.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Karyawan.Designer.cs#L1-L338)
> - **Hak Akses**: **Khusus Admin IT** (Tombol `btnKaryawan` disembunyikan di `MainForm` jika login sebagai Staff).

---

## 1. Komponen Form Input & Kontrol UI

Di dalam `grpForm` ("Form Data Karyawan (Penerima Aset)"), terdapat `tlpInputs` (5 kolom) dan 4 tombol aksi:

| Nama Kontrol | Tipe | Konfigurasi / Pilihan | Fungsi |
| :--- | :--- | :--- | :--- |
| `txtNik` | `TextBox` | `ReadOnly = true` | Menampilkan ID karyawan terpilih atau ID urut berikutnya dari `_karyawanCtrl.NextNik()` (`EMP-xxx`). |
| `txtNama` | `TextBox` | `MaxLength = 100` | Input nama lengkap karyawan. |
| `cmbDivisi` | `ComboBox` | `DropDownList`: `"Software Dev"`, `"UI/UX Design"`, `"IT Support"`, `"Finance"`, `"HRD"`, `"Marketing"` | Pilihan divisi karyawan. |
| `txtJabatan` | `TextBox` | `MaxLength = 100` | Input jabatan karyawan (default `"Staff"` jika dikosongkan). |
| `txtKontak` | `TextBox` | `MaxLength = 50` | Input nomor HP atau email karyawan (default `"-"` jika dikosongkan). |
| `btnTambah` | `Button` | Ungu `(79, 70, 229)` | Menambah data karyawan baru (`btnTambah_Click`). |
| `btnUbah` | `Button` | Biru `(59, 130, 246)` | Menyimpan perubahan data karyawan terpilih (`btnUbah_Click`). |
| `btnHapus` | `Button` | Merah `(239, 68, 68)` | Menghapus data karyawan terpilih setelah konfirmasi (`btnHapus_Click`). |
| `btnReset` | `Button` | Standar | Mengosongkan form input dan mereset pilihan tabel (`btnReset_Click`). |
| `txtCari` | `TextBox` | Terhubung ke `UIHelper.Debounce(LoadGrid)` | Pencarian karyawan berdasarkan NIK, Nama, Divisi, atau Jabatan. |
| `dgvKaryawan` | `DataGridView` | `ReadOnly = true`, `FullRowSelect` | Tabel daftar karyawan. |

---

## 2. Alur Interaksi Pengguna (`Event Handlers`)

1. **`OnLoad`** ([L21-L28](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Karyawan.cs#L21-L28)): Memuat tabel karyawan (`LoadGrid()`) dan menyiapkan NIK otomatis berikutnya (`ResetForm()`).
2. **`LoadGrid`** ([L31-L37](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Karyawan.cs#L31-L37)): Mengikat `dgvKaryawan` disertai pesan Empty State kontekstual (membedakan antara database karyawan yang belum terisi vs pencarian `txtCari` yang tidak menemukan hasil).
2. **`dgvKaryawan_CellClick`** ([L117-L129](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Karyawan.cs#L117-L129)): Menyimpan baris yang diklik ke `_selected` (`KaryawanItem`) dan mengisi `txtNik`, `txtNama`, `cmbDivisi`, `txtJabatan`, serta `txtKontak`.
3. **`btnUbah_Click`** ([L65-L81](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Karyawan.cs#L65-L81)): Memanggil `_karyawanCtrl.Update(...)` yang juga otomatis menyinkronkan nama dan divisi karyawan ke tabel `alokasi`.
4. **`btnHapus_Click`** ([L84-L102](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Karyawan.cs#L84-L102)): Meminta konfirmasi `Dialogs.Confirm("Hapus data karyawan " + _selected.Nama + "?", "Peringatan Hapus Karyawan", true)` sebelum memanggil `_karyawanCtrl.Delete(_selected)`.
