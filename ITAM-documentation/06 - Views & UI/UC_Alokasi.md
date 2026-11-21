---
title: "UC_Alokasi"
aliases:
  - UC_Alokasi.cs
  - Halaman Alokasi
tags:
  - views
  - usercontrol
  - alokasi
  - rbac
layer: views
type: reference
updated: 2026-11-15
---

# 🤝 UC_Alokasi (`Views/UC_Alokasi.cs`)

⬅️ Kembali ke [[MOC - Layer Views & UI]] | [[00 - Home]] | Lihat juga: [[AlokasiController]] · [[Tabel alokasi]] · [[Alur Peminjaman (Alokasi) & Pengembalian Aset]]

> [!info] Lokasi File Kode Sumber
> - **Logic**: [Views/UC_Alokasi.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L1-L171)
> - **Designer**: [Views/UC_Alokasi.Designer.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.Designer.cs#L1-L343)
> - **Hak Akses**:
>   - **Admin IT & Staff IT**: Keduanya memiliki akses penuh ke **Form Peminjaman / Alokasi Aset ke Karyawan** (`grpPinjam`) sekaligus **Daftar & Pengembalian Aset** (`pnlDaftar`).

---

## 1. Struktur Komponen Visual

### A. Panel Atas: `grpPinjam` ("Form Peminjaman / Alokasi Aset ke Karyawan")
| Nama Kontrol | Tipe | Konfigurasi / Sumber Data | Fungsi |
| :--- | :--- | :--- | :--- |
| `cmbAset` | `ComboBox` | `_assetCtrl.GetReadyToLend()` | Menampilkan aset yang siap dipinjam (`IsReadyToLend`: `Tersedia > 0`, `Status == "Tersedia"`, dan tidak seluruhnya `Perbaikan`/`Rusak` — termasuk aset kondisi campuran seperti `"Baik (2), Rusak (2)"`) dengan format `"AST-001 - Nama Aset (Sisa: X)"`. |
| `cmbKaryawan` | `ComboBox` | `_karyawanCtrl.GetAll()` | Menampilkan seluruh karyawan dengan format `"EMP-001 - Nama (Divisi)"`. |
| `dtpTanggal` | `DateTimePicker` | `DateTimePickerFormat.Short` | Pilihan tanggal peminjaman aset. |
| `nudJumlahPinjam` | `NumericUpDown` | `Minimum = 1`, `Maximum = aset.Tersedia` | Input jumlah unit aset yang ingin dipinjam sekaligus (otomatis dibatasi maksimal sebesar stok `Tersedia` dari aset yang dipilih). |
| `txtKeterangan` | `TextBox` | `MaxLength = 255` | Catatan keperluan peminjaman (default `"Peminjaman operasional kerja"`). |
| `btnAlokasi` | `Button` | Ungu `(79, 70, 229)` | Tombol **"Alokasikan Aset"** (`btnAlokasi_Click`). |

### B. Panel Bawah: `pnlDaftar` ("Daftar Aset Sedang Dipinjam (Aktif)")
| Nama Kontrol | Tipe | Konfigurasi / Pilihan | Fungsi |
| :--- | :--- | :--- | :--- |
| `cmbKondisiKembali` | `ComboBox` | `"Baik"`, `"Perbaikan"`, `"Rusak"` | Pilihan kondisi fisik perangkat saat dikembalikan oleh karyawan. |
| `nudJumlahKondisi` | `NumericUpDown` | `Minimum = 1`, `Maximum = loan.Jumlah` (baris terpilih) | Jumlah unit yang berkondisi sesuai pilihan; unit sisanya dikembalikan sebagai Baik. Aktif hanya bila kondisi bukan Baik dan transaksi lebih dari 1 unit. |
| `btnKembalikan` | `Button` | Hijau `(16, 185, 129)` | Tombol **"Kembalikan Aset"** (`btnKembalikan_Click`). |
| `dgvAlokasi` | `DataGridView` | `_alokasiCtrl.GetActive()` | Menampilkan daftar transaksi peminjaman yang masih berstatus `"Aktif"` beserta kolom `Jumlah` unit yang dipinjam. |

---

## 2. Bedah Method di `UC_Alokasi.cs`

1. **`AmbilUsername()`** ([L22-L25](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L22-L25)): Mengambil `UserSession.Current.DisplayName` (`NamaLengkap`) untuk dicatat pada log riwayat peminjaman dan pengembalian.
2. **`OnLoad(EventArgs e)`** ([L28-L35](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L28-L35)):
   - Mengatur `cmbKondisiKembali.SelectedIndex = 0` (`"Baik"`).
   - Memanggil `RefreshData()` (panel peminjaman `grpPinjam` dan pengembalian `pnlDaftar` aktif untuk Admin maupun Staff).
3. **`RefreshData()`** ([L38-L80](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L38-L80)):
   - Memperbarui `cmbAset.DataSource` dengan `_assetCtrl.GetReadyToLend()` (menampilkan format `"AST-002 - MacBook Pro M4 14 Inch (Sisa: 2)"`). Selama `Tersedia > 0`, aset tetap tampil di dropdown hingga unit terakhir dipinjam!
   - **Penanganan Empty State pada Dropdown & Tombol**:
     - Jika `readyAssets.Count == 0`, `cmbAset` menampilkan placeholder `"-- Tidak ada aset siap pinjam --"` dan dinonaktifkan (`Enabled = false`).
     - Jika `karyawanList.Count == 0`, `cmbKaryawan` menampilkan placeholder `"-- Belum ada data karyawan --"` dan dinonaktifkan.
     - Memanggil `SyncBatasJumlahPinjam()` untuk menyesuaikan batas maksimum `nudJumlahPinjam`.
     - Tombol `btnAlokasi` hanya aktif apabila `readyAssets.Count > 0 && karyawanList.Count > 0`.
   - **Penanganan Empty State pada Tabel Pinjaman Aktif**:
     - Mengikat `dgvAlokasi` dengan pesan Empty State `"Belum ada aset yang sedang dipinjam saat ini."`, menonaktifkan `btnKembalikan` dan `cmbKondisiKembali` ketika `activeLoans.Count == 0`, serta memanggil `SyncBatasJumlahKondisi()`.
4. **`SyncBatasJumlahPinjam()`** ([L83-L102](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L83-L102)) & **`cmbAset_SelectedIndexChanged`** ([L105-L108](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L105-L108)):
   - Mengatur `nudJumlahPinjam.Minimum = 1` dan `nudJumlahPinjam.Maximum = aset.Tersedia` setiap kali pilihan aset di `cmbAset` berubah, serta memperbarui label menjadi `"Jumlah Pinjam (Maks: {aset.Tersedia})"`.
5. **`SyncBatasJumlahKondisi()`** ([L156-L163](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L156-L163)) & **`KondisiKembali_Changed`** ([L166-L169](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L166-L169)): Mengatur batas `nudJumlahKondisi` sesuai `Math.Max(1, loan.Jumlah)` pada baris terpilih dan mengaktifkannya hanya bila kondisi bukan `Baik` dan `maks > 1`.
6. **`btnAlokasi_Click`** ([L111-L125](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L111-L125)) & **`btnKembalikan_Click`** ([L128-L153](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Alokasi.cs#L128-L153)):
   - Mengeksekusi peminjaman (sejumlah `(int)nudJumlahPinjam.Value` unit) dan pengembalian setelah dialog konfirmasi rincian kondisi (`"seluruhnya berkondisi '{kondisi}'"` jika `Baik` atau `jumlahKondisi >= loan.Jumlah`, atau `"{jumlahKondisi} unit berkondisi '{kondisi}', sisanya Baik"` jika sebagian), lalu memanggil `RefreshData()`.
