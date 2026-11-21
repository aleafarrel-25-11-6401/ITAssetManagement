---
title: "UC_Asset"
aliases:
  - UC_Asset.cs
  - Halaman Manajemen Aset
tags:
  - views
  - usercontrol
  - assets
  - crud
layer: views
type: reference
updated: 2026-11-15
---

# 💻 UC_Asset (`Views/UC_Asset.cs`)

⬅️ Kembali ke [[MOC - Layer Views & UI]] | [[00 - Home]] | Lihat juga: [[AssetController]] · [[Tabel assets]] · [[UIHelper & Dialogs]]

> [!info] Lokasi File Kode Sumber
> - **Logic**: [Views/UC_Asset.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Asset.cs#L1-L297)
> - **Designer**: [Views/UC_Asset.Designer.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Asset.Designer.cs#L1-L484)
> - **Hak Akses**: **Khusus Admin IT** (Tombol `btnAsset` disembunyikan di `MainForm` jika login sebagai Staff).

---

## 1. Komponen Form Input & Kontrol UI

Di dalam `grpForm` ("Form Data Aset IT"), terdapat dua sub-baris input (`tlpInputs` dan `tlpKondisi`) beserta 4 tombol aksi:

| Nama Kontrol | Tipe | Konfigurasi / Pilihan | Fungsi |
| :--- | :--- | :--- | :--- |
| `txtKodeAset` | `TextBox` | `ReadOnly = true` | Menampilkan kode aset terpilih atau kode otomatis berikutnya dari `_assetCtrl.NextCode()`. |
| `txtNamaAset` | `TextBox` | `MaxLength = 100` | Input nama atau tipe perangkat aset IT. |
| `cmbKategori` | `ComboBox` | `DropDownList`: `"Laptop"`, `"Monitor"`, `"Periferal"`, `"Jaringan"`, `"Lainnya"` | Pilihan kategori aset IT. |
| `nudJumlah` | `NumericUpDown` | `Minimum = 1`, `Maximum = 9999`, `Value = 1` | Input total jumlah unit fisik untuk aset tersebut (mendukung multi-unit). |
| `cmbKondisi` | `ComboBox` | `DropDownList`: `"Baik"`, `"Perbaikan"`, `"Rusak"` (+ label `"Campuran (Sebagian)"` yang ditambahkan sementara oleh `SyncKondisiUI` saat aset terpilih memiliki kondisi campuran di gudang; daftar dipulihkan via `ResetPilihanKondisi()`) | Pilihan kondisi fisik aset di sub-baris `tlpKondisi`. Memilih `"Baik"`, `"Perbaikan"`, atau `"Rusak"` menetapkan nilai `nudPerbaikan` / `nudRusak` (push-down otomatis agar tidak deadlock); unit yang sedang dipinjam tidak ikut berubah. |
| `nudPerbaikan` | `NumericUpDown` | Range dinamis `0..diGudang` (`Minimum = 0`) | Menentukan jumlah unit di gudang yang berstatus **Perbaikan**. Terhubung sinkron dengan `nudRusak` dan `cmbKondisi` melalui `SyncKondisiUI` ([L50-L128](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Asset.cs#L50-L128)) — menaikkan satu nilai otomatis menurunkan nilai lainnya bila jumlah melebihi unit di gudang. |
| `nudRusak` | `NumericUpDown` | Range dinamis `0..diGudang` (`Minimum = 0`) | Menentukan jumlah unit di gudang yang berstatus **Rusak**. |
| `lblRincianStok` | `Label` | Read-only ringkasan real-time | Menampilkan ringkasan distribusi unit saat ini, format `"Baik: {baik} | Perbaikan: {perbaikan} | Rusak: {rusak}"` (ditambah `| Pinjam: {pinjam}` bila ada unit sedang dipinjam). |
| `btnTambah` | `Button` | Ungu `(79, 70, 229)` | Menambah aset baru beserta jumlah unitnya (`btnTambah_Click`). |
| `btnUbah` | `Button` | Biru `(59, 130, 246)` | Menyimpan perubahan nama, kategori, jumlah unit, dan kondisi pada aset yang dipilih (`btnUbah_Click`). |
| `btnHapus` | `Button` | Merah `(239, 68, 68)` | Menghapus aset yang dipilih setelah konfirmasi (`btnHapus_Click`). |
| `btnReset` | `Button` | Standar | Mengosongkan form, mengatur `nudJumlah.Value = 1`, dan menampilkan kode aset urut berikutnya (`btnReset_Click`). |
| `txtCari` | `TextBox` | Terhubung ke `UIHelper.Debounce(LoadGrid)` | Kotak pencarian *real-time* dengan jeda 300ms. |
| `dgvAset` | `DataGridView` | `ReadOnly = true`, `FullRowSelect` | Tabel daftar aset (menampilkan kolom `Kode Aset`, `Nama Aset`, `Kategori`, `Jumlah`, `Tersedia`, `Kondisi`, `Status`) dengan pewarnaan baris otomatis dan *Empty State* kontekstual. |

---

## 2. Alur Interaksi Pengguna (`Event Handlers`)

1. **Saat Halaman Dibuka (`OnLoad`)**:
   Memanggil `LoadGrid()` ([L34-L41](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Asset.cs#L34-L41)) untuk mengisi `dgvAset` dan `ResetForm()` ([L131-L155](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Asset.cs#L131-L155)) untuk menyiapkan kode aset baru di `txtKodeAset` serta mengatur ulang `nudPerbaikan`, `nudRusak`, dan `lblRincianStok`.
2. **Saat Baris Tabel Diklik (`dgvAset_CellClick` — [L260-L295](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Asset.cs#L260-L295))**:
   Mengambil objek `AssetItem` dari `dgvAset.Rows[e.RowIndex].DataBoundItem`, menyimpannya ke field `_selected`, menyalin `KodeAset`, `NamaAset`, `Kategori`, serta `Jumlah`, lalu memuat `Perbaikan` dan `Rusak` ke `nudPerbaikan`/`nudRusak` (dengan fallback dari `Kondisi` lama jika `perbaikan == 0 && rusak == 0 && diGudang > 0`) dan memanggil `SyncKondisiUI()` agar opsi `"Campuran (Sebagian)"` maupun label `lblRincianStok` langsung terbarui.
3. **Saat Mengetik di Kotak Pencarian (`txtCari_TextChanged` — [L254-L257](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Asset.cs#L254-L257))**:
   Memanggil `UIHelper.Debounce(LoadGrid)` sehingga penyaringan tabel baru dijalankan 300ms setelah pengguna selesai mengetik.
4. **Saat Menghapus Aset (`btnHapus_Click` — [L226-L245](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Asset.cs#L226-L245))**:
   Memastikan `_selected != null`, menampilkan dialog konfirmasi berbahaya (`Dialogs.Confirm(..., isWarning: true)`), lalu memanggil `_assetCtrl.Delete(_selected, CurrentUser())` agar penghapusan sekaligus tercatat ke `riwayat` sebagai `"Hapus Aset"`.
