---
title: "UIHelper & Dialogs"
aliases:
  - UIHelper.cs
  - Dialogs.cs
  - UIHelper
  - Dialogs
tags:
  - views
  - uihelper
  - dialogs
  - datagridview
  - debounce
layer: views
type: reference
updated: 2026-11-15
---

# 🎨 UIHelper & Dialogs (`Views/UIHelper.cs` & `Views/Dialogs.cs`)

⬅️ Kembali ke [[MOC - Layer Views & UI]] | [[00 - Home]] | Lihat juga: [[ADR-005 - Penanganan Error Global Ramah Pengguna dan Debounce Pencarian UI]]

> [!info] Lokasi File Kode Sumber
> - **UIHelper**: [Views/UIHelper.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UIHelper.cs#L1-L398)
> - **Dialogs**: [Views/Dialogs.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/Dialogs.cs#L1-L48)

---

## 1. Bedah Lengkap `UIHelper.cs`

`UIHelper` adalah kelas utilitas statis yang menyeragamkan perilaku visual di seluruh halaman aplikasi:

### A. Mekanisme Debounce Pencarian (`Debounce` & `CancelDebounce` — [L22-L59](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UIHelper.cs#L22-L59))
- Ketika pengguna mengetik di `txtCari` pada [[UC_Asset]], [[UC_Karyawan]], atau [[UC_Riwayat]], event `TextChanged` memanggil `UIHelper.Debounce(LoadGrid, 300)`.
- Setiap ketikan baru menghentikan `_debounceTimer` dan mengulang hitungan mundur `300 ms`.
- Hanya setelah pengguna berhenti mengetik selama `300 ms`, `DebounceTimer_Tick` mengeksekusi `_debounceAction?.Invoke()`.
- Saat pengguna berpindah menu di `MainForm`, `UIHelper.CancelDebounce()` dipanggil agar timer halaman lama tidak meledak di halaman baru.

### B. Indikator Loading Mengambang (`ShowLoading` & `HideLoading` — [L62-L124](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UIHelper.cs#L62-L124))
- Dipanggil secara otomatis oleh [[DataStore.cs (REST Client & In-Memory Cache)|DataStore.Send()]] sebelum dan sesudah request HTTP ke Supabase.
- Membuat sebuah kontrol `Label` mengambang (`_loadingBadge`, ukuran `230 x 32 px`, latar biru indigo `RGB(72, 68, 211)`, teks putih tebal `"⏳ Memuat data..."` atau `"⏳ Menyimpan data..."`) yang ditempatkan tepat di tengah atas `Form.ActiveForm` (`Location = ((form.ClientSize.Width - 230) / 2, 12)`).
- Mengaktifkan `Application.UseWaitCursor = true` selama proses berlangsung dan mematikannya di blok `finally` pada `HideLoading()`.

### C. Pengikatan Tabel, Multi-Line Text Wrap, Tinggi Baris Otomatis & Visual Empty State (`BindGrid`, `ApplyColumnFormatting`, `ConfigureColumnLayout`, `Grid_Paint`)
Saat `UIHelper.BindGrid(grid, data, emptyMessage)` dipanggil:
1. Mengatur font standar `Segoe UI, 9pt`, padding sel `Padding(4, 4, 4, 4)`, mengaktifkan **`WrapMode = DataGridViewTriState.True`**, dan mengatur **`AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells`** agar teks catatan/keterangan yang panjang otomatis turun ke baris bawah (*word wrap*) dan memperbesar tinggi baris sehingga tidak terpotong.
2. Menyimpan pesan Empty State kontekstual ke `grid.Tag` (atau menggunakan `GetDefaultEmptyMessage(grid.Name)` jika `emptyMessage` tidak diisi) serta mendaftarkan event `Grid_DataBindingComplete`, `Grid_Paint`, dan `Grid_Resize`.
3. Mengikat `grid.DataSource = data` dan menjalankan `ApplyColumnFormatting(grid)`:
   - Mengubah seluruh judul kolom dari nama properti *PascalCase* menjadi teks berspasi melalui `FormatHeader(col.Name)` (contoh: `"KodeAset"` $\rightarrow$ `"Kode Aset"`, `"TanggalPinjam"` $\rightarrow$ `"Tanggal Pinjam"`).
   - Mengatur format kolom bertipe `DateTime` (`"Waktu"` $\rightarrow$ `"dd/MM/yyyy HH:mm"`, lainnya $\rightarrow$ `"dd/MM/yyyy"`).
   - Mengatur proporsi lebar kolom (`ConfigureColumnLayout`) agar kolom angka (`Jumlah`, `Tersedia`) ringkas (`FillWeight = 48`) sementara kolom `Catatan` dan `Keterangan` mendapatkan ruang ekstra lebar (`FillWeight = 175`, `MinimumWidth = 150`).
4. Memanggil `ColorizeRows(grid)`, `grid.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells)`, `ClearSelection(grid)`, dan `grid.Invalidate()`.
5. **Visual Empty State (`Grid_Paint` — [L231-L271](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UIHelper.cs#L231-L271))**:
   - Apabila `grid.Rows.Count == 0` (misalnya belum ada riwayat aktivitas, tidak ada pinjaman aktif, atau kata kunci pencarian tidak menemukan hasil), `Grid_Paint` menggambar kartu *Empty State* terpusat di area tabel dengan latar lembut `RGB(248, 250, 252)`, border `RGB(226, 232, 240)`, judul tebal (`"Belum Ada Data"` atau `"Data Tidak Ditemukan"`), serta sub-judul kontekstual yang menjelaskan langkah selanjutnya kepada pengguna.

### D. Skema Pewarnaan Baris Otomatis (`ApplyRowColors` & `ColorizeRows` — [L302-L370](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UIHelper.cs#L302-L370))

| Tipe Item Baris | Kondisi | Warna Latar (`BackColor`) | Warna Latar Saat Dipilih (`SelectionBackColor`) | Warna Teks (`ForeColor`) |
| :--- | :--- | :--- | :--- | :--- |
| **`RiwayatItem`** | `JenisAksi == "Peminjaman"` | Kuning Lembut `RGB(254, 243, 199)` | Kuning Emas `RGB(253, 230, 138)` | Cokelat Tua `RGB(146, 64, 14)` |
| **`RiwayatItem`** | `JenisAksi == "Pengembalian"` & `Catatan` diawali `"Kondisi kembali: Rusak"` | Merah Muda `RGB(254, 226, 226)` | Merah Lembut `RGB(254, 202, 202)` | Merah Tua `RGB(153, 27, 27)` |
| **`RiwayatItem`** | `JenisAksi == "Pengembalian"` & `Catatan` diawali `"Kondisi kembali: Perbaikan"` | Oranye Lembut `RGB(255, 237, 213)` | Oranye Muda `RGB(254, 215, 170)` | Oranye Tua `RGB(154, 52, 18)` |
| **`RiwayatItem`** | `JenisAksi == "Pengembalian"` (lainnya / Baik) | Hijau Lembut `RGB(209, 250, 229)` | Hijau Zamrud `RGB(167, 243, 208)` | Hijau Tua `RGB(6, 95, 70)` |
| **`RiwayatItem`** | `JenisAksi == "Tambah Aset"` | Biru Lembut `RGB(219, 234, 254)` | Biru Muda `RGB(191, 219, 254)` | Biru Indigo `RGB(30, 64, 175)` |
| **`RiwayatItem`** | Lainnya (`Hapus Aset` / aksi arsip lainnya) | Abu-abu Lembut `RGB(241, 245, 249)` | Abu-abu Terang `RGB(226, 232, 240)` | Abu Tua `RGB(71, 85, 105)` |
| **`AssetItem`** | `Rusak > 0 \|\| Kondisi == "Rusak"` | Merah Muda `RGB(254, 226, 226)` | Merah Lembut `RGB(254, 202, 202)` | Merah Tua `RGB(153, 27, 27)` |
| **`AssetItem`** | `Perbaikan > 0 \|\| Kondisi == "Perbaikan" \|\| Bermasalah > 0` | Kuning Lembut `RGB(254, 243, 199)` | Kuning Emas `RGB(253, 230, 138)` | Cokelat Tua `RGB(146, 64, 14)` |
| **`AssetItem`** | `IsBorrowed` (kondisi fisik Baik) | Biru Es `RGB(239, 246, 255)` | Biru Muda `RGB(219, 234, 254)` | Biru Indigo `RGB(30, 64, 175)` |

> [!NOTE]
> Pembantu `ApplyRowColors` ([L302-L308](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UIHelper.cs#L302-L308)) juga menyetel `SelectionForeColor` sama dengan `ForeColor` pada seluruh baris di atas agar teks tetap kontras saat dipilih. Selain itu, tata letak kolom pada `ConfigureColumnLayout` ([L183-L213](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UIHelper.cs#L183-L213)) mengatur bobot lebar khusus (`Jumlah`/`Tersedia`: `48F/55`, `KodeAset`/`IdAlokasi`/`NikKaryawan`/`Nik`/`Status`/`Kategori`: `68F/75`, `Waktu`/`TanggalPinjam`/`JenisAksi`: `82F/90`, `Catatan`/`Keterangan`: `175F/150`, dan kolom standar lainnya termasuk `Kondisi`: `100F/90`).

---

## 2. Bedah Lengkap `Dialogs.cs`

`Dialogs` menyediakan 5 method statis pembungkus `MessageBox.Show`:
1. `Dialogs.Info(message, title = "Sukses")` — Ikon `MessageBoxIcon.Information`.
2. `Dialogs.Warn(message, title = "Peringatan")` — Ikon `MessageBoxIcon.Warning`.
3. `Dialogs.Error(message, title = "Gagal")` — Ikon `MessageBoxIcon.Error`.
4. `Dialogs.Confirm(message, title = "Konfirmasi", isWarning = false)` — Tombol `YesNo`, mengembalikan `true` jika pengguna memilih `Yes`. Parameter `isWarning = true` menampilkan ikon `Warning` untuk aksi destruktif (seperti hapus aset, hapus karyawan, dan bersihkan riwayat).
5. `Dialogs.Show(OperationResult result)` — Otomatis memanggil `Info(result.Message)` jika `result.Success == true` atau `Warn(result.Message)` jika `false`.
