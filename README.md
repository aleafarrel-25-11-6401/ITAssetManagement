# IT Asset Management (ITAM)

Aplikasi desktop manajemen inventaris dan sirkulasi peminjaman aset IT berbasis **C# Windows Forms (.NET Framework 4.7.2)** dengan arsitektur **Model–View–Controller (MVC)** dan backend cloud **Supabase (PostgreSQL 17 PostgREST API)**.

---

## Fitur Utama

- **Autentikasi & Role-Based Access Control (RBAC)**:
  - Login *case-sensitive* dengan enkripsi hash password **SHA-256** dan registrasi akun baru.
  - Dua tingkat hak akses: **Admin IT** (akses penuh CRUD master aset, karyawan, alokasi, dan pembersihan riwayat) dan **Staff IT** (operasional peminjaman, pengembalian, melihat daftar aset/riwayat, dan manajemen profil).
- **Dashboard Analitik Real-Time**:
  - Ringkasan total unit aset, stok tersedia, unit sedang dipinjam, dan unit dalam perbaikan/rusak.
  - Tabel daftar peminjaman aktif dan 10 aktivitas riwayat terbaru.
- **Manajemen Inventaris Aset Multi-Unit**:
  - Pelacakan kuota stok per jenis aset (`Jumlah`, `Tersedia`, `Perbaikan`, `Rusak`) dengan format kondisi dinamis (mis. `Baik (2), Rusak (1)`).
  - Pencarian *real-time* dengan *debounce* 300 ms dan tampilan *Empty State* otomatis.
- **Manajemen Data Karyawan**:
  - Pengelolaan data karyawan penerima aset dengan kode NIK otomatis (`EMP-001`, dst.) dan validasi anti-duplikasi nomor telepon/email kontak.
- **Sirkulasi Alokasi Peminjaman & Pengembalian Multi-Unit**:
  - Peminjaman satu atau beberapa unit aset sekaligus dengan validasi ketersediaan stok dan *automatic rollback* bila koneksi terputus.
  - Pengembalian aset dengan pencatatan kondisi kembali per unit (`Baik`, `Perbaikan`, atau `Rusak`).
- **Audit Trail (Riwayat Aktivitas)**:
  - Pencatatan otomatis untuk setiap aksi `Tambah Aset`, `Hapus Aset`, `Peminjaman`, dan `Pengembalian` beserta pewarnaan baris kondisional.

---

## Arsitektur & Struktur Proyek

```text
ITAssetManagement/
├── Controllers/                # Layer logika bisnis & validasi (MVC Controllers)
│   ├── AlokasiController.cs    # Sirkulasi peminjaman & pengembalian aset
│   ├── AssetController.cs      # CRUD master aset & kalkulasi stok kondisi
│   ├── AuthController.cs       # Login, registrasi, update profil, & SHA-256 hashing
│   ├── KaryawanController.cs   # CRUD master karyawan & validasi kontak unik
│   └── RiwayatController.cs    # Pencatatan & pencarian log audit riwayat
├── Models/                     # Layer domain data & akses REST API (MVC Models)
│   ├── DataStore.cs            # HTTP Client PostgREST Supabase & In-Memory Cache (TTL 60s)
│   └── Models.cs               # Entitas domain (AssetItem, KaryawanItem, AlokasiItem, RiwayatItem, UserSession)
├── Views/                      # Layer antarmuka pengguna Windows Forms (MVC Views)
│   ├── FormLogin.cs            # Jendela autentikasi login
│   ├── FormRegister.cs         # Jendela pendaftaran akun baru
│   ├── MainForm.cs             # Navigasi utama sidebar & kontrol akses RBAC
│   ├── UC_Dashboard.cs         # Halaman ringkasan statistik
│   ├── UC_Asset.cs             # Halaman manajemen aset IT
│   ├── UC_Karyawan.cs          # Halaman manajemen karyawan
│   ├── UC_Alokasi.cs           # Halaman peminjaman & pengembalian aset
│   ├── UC_Riwayat.cs           # Halaman log riwayat aktivitas
│   ├── UC_Profil.cs            # Halaman pengaturan profil & ubah password
│   ├── UIHelper.cs             # Helper styling DataGridView, Empty State, & debounce
│   └── Dialogs.cs              # Helper MessageBox standar
├── ITAM-documentation/         # Dokumentasi teknis lengkap (Obsidian Vault / Diátaxis + ADR)
├── App.config.example          # Template konfigurasi koneksi Supabase
├── ITAssetManagement.csproj    # File proyek MSBuild (.NET Framework 4.7.2)
└── Program.cs                  # Entry point aplikasi & Global Exception Handler
```

---

## Skema Database (Supabase PostgreSQL)

Database terdiri dari **5 tabel yang saling berelasi** di schema `public` dengan Row-Level Security (RLS) aktif:
1. `public.app_users` (`username` PK) — Menyimpan akun pengguna (`Admin` / `Staff`), hash password SHA-256, dan nama tampilan (`nama_lengkap`).
2. `public.assets` (`kode_aset` PK) — Menyimpan master aset IT beserta rincian stok (`jumlah`, `tersedia`, `perbaikan`, `rusak`).
3. `public.karyawan` (`nik` PK) — Menyimpan master karyawan dengan validasi keunikan `kontak`.
4. `public.alokasi` (`id_alokasi` PK, FK `kode_aset` → `assets`, FK `nik_karyawan` → `karyawan`) — Menyimpan transaksi peminjaman aset.
5. `public.riwayat` (`id` PK, FK `username_pelaku` → `app_users.username`) — Menyimpan log audit permanen seluruh aktivitas aset.

---

## Cara Menjalankan Proyek (Quick Start)

1. **Clone repository ini**:
   ```bash
   git clone https://github.com/aleafarrel-25-11-6401/ITAssetManagement.git
   cd ITAssetManagement
   ```
2. **Siapkan file konfigurasi `App.config`**:
   Salin file `App.config.example` menjadi `App.config`:
   ```powershell
   Copy-Item App.config.example App.config
   ```
   Buka `App.config` dan isi `SupabaseUrl` serta `SupabaseKey` dengan kredensial proyek Supabase Anda:
   ```xml
   <appSettings>
       <add key="SupabaseUrl" value="https://YOUR_PROJECT_REF.supabase.co" />
       <add key="SupabaseKey" value="sb_publishable_YOUR_SUPABASE_KEY" />
   </appSettings>
   ```
3. **Build dan jalankan di Visual Studio**:
   - Buka `ITAssetManagement.csproj` (atau `ITAssetManagement.sln`) di **Visual Studio 2022**.
   - Tekan **`F5`** (*Start Debugging*) atau **`Ctrl + F5`** (*Start Without Debugging*).
