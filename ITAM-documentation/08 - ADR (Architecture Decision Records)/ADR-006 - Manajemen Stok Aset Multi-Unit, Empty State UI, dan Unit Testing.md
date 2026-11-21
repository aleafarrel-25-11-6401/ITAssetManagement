---
title: "ADR-006 - Manajemen Stok Aset Multi-Unit, Empty State UI, dan Unit Testing"
aliases:
  - ADR-006
  - Stok Multi-Unit
  - Empty State UI
  - Unit Testing
tags:
  - adr
  - arsitektur
  - multi-unit
  - empty-state
  - unit-testing
layer: architecture
type: explanation
status: Accepted
updated: 2026-11-15
---

# 📐 ADR-006: Manajemen Stok Aset Multi-Unit, Empty State UI, dan Unit Testing

⬅️ Kembali ke [[MOC - Arsitektur & Desain]] | [[00 - Home]] | Lihat juga: [[Tabel assets]] · [[AssetController]] · [[AlokasiController]] · [[UIHelper & Dialogs]]

- **Status**: *Accepted & Amended (Pengujian internal dibersihkan; kode inti dibuat `public`)*
- **Tanggal**: 15 November 2026
- **Komponen Terkait**: [[Tabel assets]], [[Models.cs (Entitas & Konstanta Domain)]], [[DataStore.cs (REST Client & In-Memory Cache)]], [[AssetController]], [[AlokasiController]], [[UIHelper & Dialogs]]

---

## 1. Konteks & Permasalahan (*Context*)

Pada desain awal aplikasi, satu baris pada tabel `public.assets` diasumsikan hanya mewakili satu unit fisik tunggal (`status = 'Tersedia'` atau `'Dipinjam'`). Di dunia nyata, perusahaan sering memiliki beberapa unit untuk tipe perangkat yang sama (misalnya `2` unit Laptop MacBook Pro). Jika `1` unit dipinjam, seharusnya masih tersisa `1` unit yang tetap berstatus `'Tersedia'` dan dapat dipinjamkan kepada karyawan lain, dan baru berubah menjadi `'Dipinjam'` (tidak bisa dipinjam lagi) apabila ke-`2` unit tersebut habis dipinjam.

Selain itu, ketika tabel (`DataGridView`) pada halaman Riwayat, Alokasi, Dashboard, Aset, atau Karyawan tidak memiliki baris data (atau hasil filter pencarian kosong), area tabel sebelumnya hanya menampilkan bidang kosong tanpa penjelasan visual (*Empty State*).

---

## 2. Keputusan Arsitektur (*Decision*)

### A. Kuota Stok Multi-Unit (`jumlah` & `tersedia`) serta Peminjaman Sekaligus (`alokasi.jumlah`)
1. **Skema PostgreSQL (`public.assets` & `public.alokasi`)**:
   - Menambahkan kolom `jumlah integer NOT NULL DEFAULT 1` dan `tersedia integer NOT NULL DEFAULT 1` pada `public.assets` melalui migrasi `tambah_kolom_jumlah_dan_tersedia_pada_assets`, serta `jumlah integer NOT NULL DEFAULT 1` pada `public.alokasi` melalui migrasi `tambah_kolom_jumlah_pada_alokasi`.
   - Menambahkan constraint tingkat database:
     - `assets_jumlah_check CHECK (jumlah >= 1)`
     - `assets_tersedia_check CHECK (tersedia >= 0 AND tersedia <= jumlah)`
     - `alokasi_jumlah_check CHECK (jumlah >= 1)`
2. **Aturan Transisi Status, Peminjaman Sekaligus (`jumlahPinjam`), & Hak Akses Staff di C#**:
   - `AssetItem.Dipinjam` dihitung otomatis sebagai `Math.Max(0, Jumlah - Tersedia - Math.Max(0, Perbaikan) - Math.Max(0, Rusak))`.
   - `AssetItem.IsReadyToLend` bernilai `true` selama `Tersedia > 0 && Status == "Tersedia" && Kondisi != "Perbaikan" && Kondisi != "Rusak"` (sehingga aset dengan kondisi campuran seperti `"Baik (2), Rusak (2)"` tetap siap dipinjamkan untuk unit yang Baik).
   - Baik **Admin IT** maupun **Staff IT** kini diberikan akses penuh pada [[UC_Alokasi]] untuk melakukan peminjaman (`grpPinjam`) maupun pengembalian (`pnlDaftar`).
   - Saat peminjaman (`AlokasiController.Allocate`), pengguna dapat memasukkan `jumlahPinjam` (`1 <= jumlahPinjam <= asset.Tersedia`). `Tersedia` dikurangi sebesar `jumlahPinjam`. `Status` tetap `"Tersedia"` selama `sisaTersedia > 0`, dan baru berubah menjadi `"Dipinjam"` saat `sisaTersedia == 0`.
   - Saat pengembalian (`AlokasiController.Return`), `Tersedia` ditambah kembali sebesar `loan.Jumlah` **hanya untuk unit berkondisi Baik**; unit yang dikembalikan `Rusak`/`Perbaikan` dicatat di kolom `rusak`/`perbaikan` (migrasi `tambah_stok_perbaikan_dan_rusak_dinamis_pada_assets`) sehingga kondisi stok dinamis, mis. `"Baik (2), Rusak (2)"`, dan unit Baik tetap bisa dipinjam. Jumlah unit rusak/perbaikan dipilih per transaksi (`jumlahKondisi`), sisanya kembali Baik; perubahan stok dibatalkan (rollback) bila penutupan transaksi gagal.
   - Pada [[UC_Asset]], Admin dapat menandai sebagian unit di gudang sebagai `Baik`, `Perbaikan`, atau `Rusak` melalui kontrol `nudPerbaikan` & `nudRusak` (tervalidasi seragam lewat `AssetController.ResolveStokKondisi` tanpa mengganggu unit yang sedang dipinjam).
   - Saat penghapusan (`AssetController.Delete`), aset tidak boleh dihapus jika `IsBorrowed` (`Dipinjam > 0`) atau masih ada alokasi aktif; penghapusan yang lolos tercatat ke [[Tabel riwayat]] sebagai aksi `"Hapus Aset"` (didukung migrasi `izinkan_hapus_aset_pada_riwayat`) dan menyimpan jumlah unit pada kolom `riwayat.jumlah`.
   - Saat pengubahan (`AssetController.Update`), total `Jumlah` tidak boleh dikurangi di bawah `Dipinjam` yang sedang berjalan.

### B. Komponen Visual Empty State & Pewarnaan Kondisional Terpusat di `UIHelper`
- Memperluas `UIHelper.BindGrid(DataGridView grid, object data, string emptyMessage = null)` dengan event handler `Grid_Paint` dan `Grid_Resize` yang merender kartu informasi *Empty State* tepat di tengah `DataGridView` setiap kali `grid.Rows.Count == 0`, serta merapikan tata letak kolom dan legenda **6 badge warna** pada `flpLegenda` di [[UC_Riwayat]].
- Memperluas `UIHelper.ColorizeRows` (dengan pembantu `ApplyRowColors`) agar baris `Pengembalian` dibedakan berdasarkan kondisi kembali (**Hijau** untuk Baik, **Oranye** untuk Perbaikan, **Merah** untuk Rusak) dan aksi arsip **Hapus Aset** tampil berwarna **Abu-abu**.
- Pada [[UC_Alokasi]], menambahkan placeholder otomatis pada `ComboBox` (`"-- Tidak ada aset siap pinjam --"` / `"-- Belum ada data karyawan --"`), menyinkronkan batas maksimum `nudJumlahPinjam` terhadap `aset.Tersedia` dan `nudJumlahKondisi` terhadap `loan.Jumlah`, serta menonaktifkan tombol aksi saat tidak ada item yang dapat diproses.

### C. Pemisahan `username` (Case-Sensitive) & `nama_lengkap` (`DisplayName`) serta Kebersihan Kode Inti
- Menambahkan kolom `nama_lengkap varchar(100)` pada `public.app_users` (migrasi `tambah_nama_lengkap_dan_username_case_sensitive`) serta menghapus fungsi `lower()` pada trigger `enforce_app_users_security()` dan `.ToLowerInvariant()` pada `AuthController`/`DataStore`/`FormRegister` agar `username` bersifat **case-sensitive** penuh dan nama yang tampil di aplikasi adalah `NamaLengkap` (`UserSession.DisplayName`).
- *(Amandemen 15 Nov 2026)*: Seluruh mode pengujian luring dan pengisian cache uji di `DataStore`, pengecualian assembly pada `AssemblyInfo.cs`, overload lama tak terpakai di `AuthController`, serta proyek uji kustom terdahulu dihapus seluruhnya agar aplikasi utama berisi **100% kode inti**. `DataStore` dijadikan `public static class` sehingga proyek pengujian eksternal (seperti NUnit) dapat dibuat secara terpisah di luar aplikasi inti tanpa memodifikasi kode produksi.

---

## 3. Konsekuensi (*Consequences*)

- **Positif**:
  - Sirkulasi peminjaman aset mencerminkan kondisi inventaris dunia nyata secara presisi dan dijaga ganda oleh validasi C# serta `CHECK` constraint PostgreSQL.
  - Antarmuka pengguna selalu memberikan umpan balik visual yang jelas meskipun tabel atau hasil pencarian kosong.
  - Kode inti aplikasi bersih dari logika tiruan/mode test, dengan seluruh lapisan domain (`Models`, `DataStore`, `Controllers`, `Views`) dapat direferensikan langsung sebagai `public` oleh proyek pengujian eksternal.

## Change Log
- 2026-11-21: Proyek pengujian eksternal yang direncanakan di atas kini ada: `ITAssetManagement.Tests` (NUnit 3, `net472`, masuk ke `ITAssetManagement.sln`) dengan server HTTP palsu lokal. Detail dan hasil: [[ADR-007 - Relasi Riwayat ke Pengguna dan Kontak Karyawan Unik]] dan [[Panduan Instalasi & Menjalankan Aplikasi]].
