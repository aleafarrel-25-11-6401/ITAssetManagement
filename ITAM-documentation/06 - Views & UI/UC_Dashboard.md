---
title: "UC_Dashboard"
aliases:
  - UC_Dashboard.cs
  - Halaman Dashboard
tags:
  - views
  - usercontrol
  - dashboard
layer: views
type: reference
updated: 2026-11-15
---

# 📊 UC_Dashboard (`Views/UC_Dashboard.cs`)

⬅️ Kembali ke [[MOC - Layer Views & UI]] | [[00 - Home]] | Lihat juga: [[AssetController]] · [[AlokasiController]] · [[RiwayatController]]

> [!info] Lokasi File Kode Sumber
> - **Logic**: [Views/UC_Dashboard.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Dashboard.cs#L1-L37)
> - **Designer**: [Views/UC_Dashboard.Designer.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Dashboard.Designer.cs#L1-L354)
> - **Hak Akses**: Tersedia untuk **Admin IT** dan **Staff IT** (halaman default saat pertama kali masuk ke [[MainForm (Navigasi & RBAC UI)|MainForm]]).

---

## 1. Struktur Komponen Visual

`UC_Dashboard` menggunakan 2 `TableLayoutPanel` responsif:

### A. Panel Kartu Statistik (`tlpCards` — 4 Kolom x 25% Lebar, Berbasis Total Unit Fisik)
| Nama Panel Kartu | Warna Latar (`RGB`) | Label Judul | Label Nilai | Rumus Perhitungan Multi-Unit di `OnLoad()` ([L27-L31](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Dashboard.cs#L27-L31)) |
| :--- | :--- | :--- | :--- | :--- |
| `cardTotal` | Ungu Indigo `(79, 70, 229)` | `Total Aset` | `lblTotalValue` | `daftarAset.Sum(a => a.Jumlah)` *(Total seluruh unit fisik aset)* |
| `cardTersedia` | Hijau Zamrud `(16, 185, 129)` | `Aset Tersedia` | `lblTersediaValue` | `daftarAset.Sum(a => a.Tersedia)` *(Total unit Baik siap pinjam, termasuk dari aset berkondisi campuran)* |
| `cardDipinjam` | Biru Cerah `(59, 130, 246)` | `Sedang Dipinjam` | `lblDipinjamValue` | `daftarAset.Sum(a => a.Dipinjam)` *(Total unit yang sedang dipinjam aktif)* |
| `cardPerbaikan` | Oranye Amber `(245, 158, 11)` | `Rusak / Perbaikan` | `lblPerbaikanValue` | `daftarAset.Sum(a => a.Bermasalah)` *(Total unit rusak + perbaikan)* |

### B. Panel Tabel Ringkasan (`tlpTables` — 2 Baris x 50% Tinggi dengan Empty State)
1. **`pnlAlokasi` (`dgvAlokasiAktif`)**:
   - Judul: `"Aset yang Sedang Dipinjam"`
   - Sumber Data: `UIHelper.BindGrid(dgvAlokasiAktif, _alokasiCtrl.GetActive(), "Tidak ada aset yang sedang dipinjam saat ini.")` ([L33](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Dashboard.cs#L33)).
2. **`pnlRiwayat` (`dgvRiwayatTerbaru`)**:
   - Judul: `"Aktivitas Riwayat Terakhir"`
   - Sumber Data: `UIHelper.BindGrid(dgvRiwayatTerbaru, _riwayatCtrl.Latest(10), "Belum ada catatan riwayat aktivitas terbaru.")` ([L34](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UC_Dashboard.cs#L34)) — otomatis diberi pewarnaan baris sesuai `JenisAksi` oleh `UIHelper.ColorizeRows`.
