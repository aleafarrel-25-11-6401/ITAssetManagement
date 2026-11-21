---
title: "ADR-005: Penanganan Error Global Ramah Pengguna dan Debounce Pencarian UI"
aliases:
  - ADR-005
  - Global Exception Handling
  - Debounce Pencarian
tags:
  - adr
  - views
  - ux
  - exception-handling
  - debounce
layer: adr
type: adr
status: Accepted
date: 2026-11-15
updated: 2026-11-15
---

# ADR-005: Penanganan Error Global Ramah Pengguna dan Debounce Pencarian UI

⬅️ Kembali ke [[MOC - Arsitektur & Desain]] | [[MOC - Layer Views & UI]] | Lihat juga: [[Program.cs & Siklus Hidup Aplikasi]] · [[UIHelper & Dialogs]]

## Status
**Accepted**

## Date
2026-11-15

## Context
Pada aplikasi desktop Windows Forms yang digunakan oleh staf operasional sehari-hari, terdapat dua masalah UX yang sering muncul:
1. **Pesan Error Teknis (.NET Unhandled Exception Dialog)**: Jika terjadi gangguan jaringan tak terduga atau kegagalan komponen UI, dialog bawaan Windows Forms menampilkan *stack trace* internal dan tombol *Continue / Quit* yang membingungkan pengguna non-teknis serta berisiko membocorkan struktur kode internal.
2. **Kedipan (*Flicker*) dan Beban Render `DataGridView` Saat Mengetik Cepat**: Jika event `txtCari_TextChanged` langsung memanggil `UIHelper.BindGrid()` dan `ColorizeRows()` pada setiap huruf yang diketik (misal mengetik 10 huruf dalam 1 detik), `DataGridView` akan melakukan *data-binding* ulang dan pewarnaan baris sebanyak 10 kali berturut-turut.

## Decision
1. **Penanganan Error Berlapis yang Ramah Pengguna**:
   - Di level `Program.Main()` ([Program.cs#L19-L38](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Program.cs#L19-L38)): Mendaftarkan `Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException)`, `Application.ThreadException`, dan `AppDomain.CurrentDomain.UnhandledException` yang memanggil `UIHelper.HideLoading()` dan menampilkan pesan sopan dalam Bahasa Indonesia melalui `Dialogs.Warn(...)`.
   - Di level `DataStore.GetRows()` ([DataStore.cs#L234-L253](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/DataStore.cs#L234-L253)): Membatasi kemunculan peringatan koneksi offline maksimal 1 kali setiap 3 detik (`_lastErrorTime`) agar pengguna tidak terjebak dalam rentetan popup beruntun ketika membuka halaman Dashboard yang memuat beberapa tabel sekaligus.
2. **Debounce Pencarian 300ms Terpusat di `UIHelper.Debounce()`**:
   - Menunda pemanggilan `LoadGrid()` menggunakan `System.Windows.Forms.Timer` tunggal (`_debounceTimer`) dengan interval `300 ms` ([UIHelper.cs#L22-L59](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/UIHelper.cs#L22-L59)).
   - Menyediakan `UIHelper.CancelDebounce()` yang dipanggil setiap kali pengguna berpindah halaman (`MainForm.ShowPage`) atau Logout (`MainForm.btnLogout_Click`) agar callback pencarian dari halaman yang sudah di-`Dispose()` tidak pernah terpanggil.

## Alternatives Considered

### 1. Mencari Hanya Saat Menekan Tombol "Cari" / Tombol `Enter`
- **Pros**: Tidak memerlukan timer debounce.
- **Cons**: Kurang responsif dibandingkan pencarian *live* yang otomatis menyaring tabel saat pengguna selesai mengetik.
- **Rejected**: Debounce 300ms memberikan keseimbangan terbaik antara pencarian *real-time* dan efisiensi perenderan UI.

## Consequences
- **Positif**: Aplikasi tidak pernah mengalami *crash* mendadak atau menampilkan *stack trace* teknis kepada pengguna akhir.
- **Positif**: Pencarian pada halaman Aset, Karyawan, dan Riwayat terasa sangat mulus dan bebas kedipan (*flicker-free*).
