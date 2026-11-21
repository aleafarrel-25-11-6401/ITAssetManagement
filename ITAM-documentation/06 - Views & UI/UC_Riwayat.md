---
title: "UC_Riwayat"
aliases:
  - UC_Riwayat.cs
  - Halaman Riwayat
tags:
  - views
  - usercontrol
  - riwayat
  - audit-log
layer: views
type: reference
updated: 2026-11-15
---

# 📜 UC_Riwayat (`Views/UC_Riwayat.cs`)

⬅️ Kembali ke [[MOC - Layer Views & UI]] | [[00 - Home]] | Lihat juga: [[RiwayatController]] · [[Tabel riwayat]] · [[UIHelper & Dialogs]]

> [!info] Lokasi File Kode Sumber
> - **Logic**: [Views/UC_Riwayat.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Riwayat.cs#L1-L89)
> - **Designer**: [Views/UC_Riwayat.Designer.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Riwayat.Designer.cs#L1-L263)
> - **Hak Akses Adaptif**:
>   - **Admin IT & Staff IT**: Dapat melihat, memfilter, dan mencari seluruh riwayat aktivitas.
>   - **Khusus Admin IT**: Tombol merah **`btnBersihkan` ("Bersihkan Riwayat")** ditampilkan di kanan atas. Jika login sebagai **Staff IT**, `btnBersihkan.Visible = false` dan lebar kotak pencarian `txtCari` otomatis diperpanjang hingga rata kanan tabel (`txtCari.Width = dgvRiwayat.Right - txtCari.Left`).

---

## 1. Komponen Kontrol UI pada `UC_Riwayat`

| Nama Kontrol | Tipe | Konfigurasi / Nilai | Fungsi |
| :--- | :--- | :--- | :--- |
| `cmbFilterAksi` | `ComboBox` | `"Semua"`, `"Peminjaman"`, `"Pengembalian"`, `"Tambah Aset"`, `"Hapus Aset"` | Menyaring tabel berdasarkan jenis aktivitas (`Filter_Changed`). |
| `txtCari` | `TextBox` | Debounce 300ms (`Filter_Changed`) | Mencari kata kunci di kode aset, nama aset, nama karyawan, catatan, atau pelaku (`oleh_user`). |
| `btnBersihkan` | `Button` | Merah `(239, 68, 68)` | Menghapus seluruh catatan riwayat setelah konfirmasi (`btnBersihkan_Click`, khusus Admin). |
| `flpLegenda` | `FlowLayoutPanel` | Wadah wrap otomatis | Menyusun seluruh badge legenda warna di bawah toolbar tanpa tumpang tindih pada berbagai ukuran skala DPI. |
| `lblBadgePinjam` | `Label` | Latar Kuning `(254, 243, 199)` / Teks Cokelat `(146, 64, 14)` | Legenda indikator warna baris **Pinjam** (`Peminjaman`). |
| `lblBadgeKembali` | `Label` | Latar Hijau `(209, 250, 229)` / Teks Hijau Tua `(6, 95, 70)` | Legenda indikator warna baris **Kembali Baik** (`Pengembalian` kondisi Baik). |
| `lblBadgePerbaikan` | `Label` | Latar Oranye `(255, 237, 213)` / Teks Oranye Tua `(154, 52, 18)` | Legenda indikator warna baris **Kembali Perbaikan** (`Pengembalian` kondisi Perbaikan). |
| `lblBadgeRusak` | `Label` | Latar Merah Muda `(254, 226, 226)` / Teks Merah Tua `(153, 27, 27)` | Legenda indikator warna baris **Kembali Rusak** (`Pengembalian` kondisi Rusak). |
| `lblBadgeTambah` | `Label` | Latar Biru `(219, 234, 254)` / Teks Biru Tua `(30, 64, 175)` | Legenda indikator warna baris **Tambah Aset**. |
| `lblBadgeHapus` | `Label` | Latar Abu-abu `(241, 245, 249)` / Teks Abu Tua `(71, 85, 105)` | Legenda indikator warna baris **Hapus Aset**. |
| `dgvRiwayat` | `DataGridView` | `ReadOnly = true`, `FullRowSelect` | Tabel daftar riwayat aktivitas keluar-masuk aset. |

---

## 2. Detail Teknis Penting di `UC_Riwayat.cs`

1. **Penataan Badge Anti-Tumpuk dengan `flpLegenda` (`OnLoad` — [L20-L34](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Riwayat.cs#L20-L34) & Designer — [L165-L263](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Riwayat.Designer.cs#L165-L263))**:
   Keenam badge indikator warna (`Pinjam`, `Kembali Baik`, `Kembali Perbaikan`, `Kembali Rusak`, `Tambah Aset`, dan `Hapus Aset`) ditempatkan di dalam `FlowLayoutPanel` `flpLegenda`, sehingga tata letak badge otomatis mengalir rapi dan tidak pernah saling bertumpuk pada skala DPI Windows 100%, 125%, maupun 150%.
2. **Event Handler Gabungan `Filter_Changed`** ([L51-L61](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Riwayat.cs#L51-L61)):
   - Jika dipicu oleh ketikan di `txtCari`, menggunakan `UIHelper.Debounce(LoadGrid)` (jeda 300ms).
   - Jika dipicu oleh perubahan pilihan dropdown `cmbFilterAksi`, langsung mengeksekusi `LoadGrid()` tanpa jeda!
3. **Empty State Kontekstual & Status Tombol `btnBersihkan` (`LoadGrid` — [L37-L48](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Riwayat.cs#L37-L48))**:
   - Membedakan pesan Empty State antara kondisi filter aktif (`"Tidak ada catatan riwayat yang cocok dengan filter pencarian."`) dan kondisi tabel riwayat benar-benar kosong (`"Belum ada catatan riwayat aktivitas keluar-masuk aset."`).
   - Mengatur `btnBersihkan.Enabled = _riwayatCtrl.Latest(1).Count > 0;` agar tombol Bersihkan Riwayat otomatis nonaktif saat riwayat kosong.
