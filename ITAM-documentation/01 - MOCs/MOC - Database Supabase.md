---
title: "MOC - Database Supabase"
aliases:
  - Database MOC
  - Peta Database
  - Supabase Wiki
tags:
  - moc
  - database
  - supabase
  - postgresql
type: moc
updated: 2026-11-15
---

# 🗄️ MOC - Database Supabase

⬅️ Kembali ke [[00 - Home]] | Lihat juga: [[MOC - Layer Models]]

> [!info] Tentang Peta Konten Ini
> **MOC - Database Supabase** menghimpun seluruh hasil inspeksi mendalam (*deep inspection*) terhadap database PostgreSQL 17 pada proyek Supabase **IT Asset Management** (`agbmfszugvjwvinxfgiy` di region `eu-west-1`). Dokumentasi ini mencakup ERD, tipe data tingkat kolom, kardinalitas relasi, *constraints*, *indexes*, *Row-Level Security (RLS)*, *functions*, *triggers*, hingga riwayat migrasi dan audit *Postgres Best Practices*.

---

## 🗺️ Gambaran Umum & Diagram Relasi

- **[[Skema Database & ERD Lengkap]]**
  Diagram Entity-Relationship (ERD) lengkap beserta penjelasan relasi antar tabel, kardinalitas (`1:N`), aturan `ON UPDATE CASCADE ON DELETE CASCADE`, serta perbandingan antara *Surrogate Identity Column* (`id`) dan *Natural Primary Key*.

---

## 📋 Referensi Mendalam Per Tabel (Schema `public`)

Setiap catatan tabel di bawah ini mendokumentasikan struktur kolom, tipe data (`varchar(n)`, `bigint`, `date`, `timestamptz`), nilai *default*, *nullability*, *Primary Key*, *Foreign Key*, *Check Constraints*, *Index*, *RLS Policy*, pemetaan ke model C#, serta endpoint PostgREST yang memanggilnya:

1. 👤 **[[Tabel app_users]]** — Tabel akun pengguna (`username` PK case-sensitive, `password` SHA-256 hex, `role` Admin/Staff, `nama_lengkap`).
2. 💻 **[[Tabel assets]]** — Tabel master inventaris aset IT (`kode_aset` PK, `nama_aset`, `kategori`, `jumlah`, `tersedia`, `perbaikan`, `rusak`, `kondisi`, `status`).
3. 🧑‍💼 **[[Tabel karyawan]]** — Tabel master data karyawan penerima aset (`nik` PK, `nama`, `divisi`, `jabatan`, `kontak`).
4. 🤝 **[[Tabel alokasi]]** — Tabel transaksi peminjaman aset multi-unit (`id_alokasi` PK, FK ke `assets` dan `karyawan`, `jumlah`, tanggal pinjam, status alokasi).
5. 📜 **[[Tabel riwayat]]** — Tabel log audit aktivitas keluar-masuk dan CRUD aset (`id` PK, `waktu` timestamptz, `jenis_aksi` termasuk `Hapus Aset`, `jumlah`, `oleh_user`, serta `username_pelaku` FK ke `app_users.username`).

---

## 🔐 Keamanan, Otomasi, & Riwayat Skema

- **[[Keamanan RLS, Trigger, dan Fungsi Database]]**
  Analisis lengkap 7 kebijakan RLS (*Row-Level Security*), hak akses `GRANT` pada role `anon` & `authenticated`, fungsi PL/pgSQL `enforce_app_users_security()`, `enforce_karyawan_kontak_unique()`, `sync_riwayat_pelaku()`, `sync_app_users_nama_to_riwayat()`, dan *event trigger* `rls_auto_enable()`.
- **[[Riwayat Migrasi & Audit Best Practices]]**
  Bedah kronologis 13 migrasi skema di `supabase_migrations.schema_migrations`, analisis evolusi skema (*multi-unit stock*, `nama_lengkap`, `Hapus Aset`, relasi `riwayat` ke `app_users`, dan kontak karyawan unik), serta laporan evaluasi **Supabase Security & Performance Advisors**.

---

## 🔗 Keterkaitan Tabel dengan Kode Aplikasi C#

| Tabel Database      | Model C# Terkait                                                              | Controller Utama                                                   | Endpoint di `DataStore.cs`                                                                |
| :------------------ | :---------------------------------------------------------------------------- | :----------------------------------------------------------------- | :---------------------------------------------------------------------------------------- |
| [[Tabel app_users]] | [[UserSession & OperationResult\|UserSession]] (`Dictionary<string, object>`) | [[AuthController]]                                                 | `FindUser()`, `Insert("app_users")`, `Update("app_users?...")`                            |
| [[Tabel assets]]    | [[Models.cs (Entitas & Konstanta Domain)\|AssetItem]]                         | [[AssetController]], [[AlokasiController]]                         | `GetAssets()`, `Insert("assets")`, `Update("assets?...")`, `Delete("assets?...")`         |
| [[Tabel karyawan]]  | [[Models.cs (Entitas & Konstanta Domain)\|KaryawanItem]]                      | [[KaryawanController]]                                             | `GetKaryawan()`, `Insert("karyawan")`, `Update("karyawan?...")`, `Delete("karyawan?...")` |
| [[Tabel alokasi]]   | [[Models.cs (Entitas & Konstanta Domain)\|AlokasiItem]]                       | [[AlokasiController]], [[AssetController]], [[KaryawanController]] | `GetAlokasi()`, `Insert("alokasi")`, `Update("alokasi?...")`                              |
| [[Tabel riwayat]]   | [[Models.cs (Entitas & Konstanta Domain)\|RiwayatItem]]                       | [[RiwayatController]], [[AuthController]]                          | `GetRiwayat()`, `RenameHistoryUser()`, `Insert("riwayat")`, `Delete("riwayat?...")`       |
