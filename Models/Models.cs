using System;
using System.ComponentModel;

namespace ITAssetManagement.Models
{
    // Konstanta pilihan status ketersediaan aset
    public static class AssetStatus
    {
        public const string Tersedia = "Tersedia";
        public const string Dipinjam = "Dipinjam";
    }

    // Konstanta pilihan kondisi fisik aset
    public static class AssetCondition
    {
        public const string Baik = "Baik";
        public const string Perbaikan = "Perbaikan";
        public const string Rusak = "Rusak";
    }

    // Konstanta pilihan status transaksi alokasi
    public static class AlokasiStatus
    {
        public const string Aktif = "Aktif";
        public const string Dikembalikan = "Dikembalikan";
    }

    // Konstanta jenis aktivitas pada menu Riwayat
    public static class RiwayatAction
    {
        public const string Peminjaman = "Peminjaman";
        public const string Pengembalian = "Pengembalian";
        public const string TambahAset = "Tambah Aset";
        public const string HapusAset = "Hapus Aset";
    }

    // Class model untuk menyimpan data barang aset IT beserta jumlah stok unit per kondisinya secara dinamis
    public class AssetItem
    {
        public string KodeAset { get; set; }
        public string NamaAset { get; set; }
        public string Kategori { get; set; }
        public int Jumlah { get; set; } = 1;
        public int Tersedia { get; set; } = 1;
        public string Kondisi { get; set; }
        public string Status { get; set; }

        // Jumlah unit aset yang sedang dalam perbaikan
        [Browsable(false)]
        public int Perbaikan { get; set; }

        // Jumlah unit aset yang sedang rusak
        [Browsable(false)]
        public int Rusak { get; set; }

        // Menghitung total unit aset yang sedang bermasalah (Perbaikan + Rusak)
        [Browsable(false)]
        public int Bermasalah
        {
            get
            {
                int total = Math.Max(0, Perbaikan) + Math.Max(0, Rusak);
                if (total > 0) return Math.Min(Jumlah, total);
                if (Kondisi == AssetCondition.Perbaikan || Kondisi == AssetCondition.Rusak)
                {
                    return Math.Max(1, Jumlah - Dipinjam);
                }
                return 0;
            }
        }

        // Menghitung jumlah unit aset yang sedang dipinjam saat ini
        [Browsable(false)]
        public int Dipinjam
        {
            get { return Math.Max(0, Jumlah - Tersedia - Math.Max(0, Perbaikan) - Math.Max(0, Rusak)); }
        }

        // Mengecek apakah ada minimal 1 unit aset yang sedang dipinjam (disembunyikan dari kolom tabel)
        [Browsable(false)]
        public bool IsBorrowed
        {
            get { return Dipinjam > 0 || Status == AssetStatus.Dipinjam; }
        }

        // Mengecek apakah masih ada unit aset berkondisi Baik yang siap dipinjamkan (Tersedia > 0)
        [Browsable(false)]
        public bool IsReadyToLend
        {
            get
            {
                return Tersedia > 0
                    && Status == AssetStatus.Tersedia
                    && Kondisi != AssetCondition.Perbaikan
                    && Kondisi != AssetCondition.Rusak;
            }
        }

        // Menyusun teks kondisi dinamis sesuai jumlah unit Baik, Perbaikan, dan Rusak
        public static string FormatKondisi(int jumlah, int perbaikan, int rusak)
        {
            jumlah = Math.Max(1, jumlah);
            perbaikan = Math.Max(0, perbaikan);
            rusak = Math.Max(0, rusak);
            int baik = Math.Max(0, jumlah - perbaikan - rusak);

            if (perbaikan == 0 && rusak == 0) return AssetCondition.Baik;
            if (baik == 0 && rusak == 0) return AssetCondition.Perbaikan;
            if (baik == 0 && perbaikan == 0) return AssetCondition.Rusak;

            var rincian = new System.Collections.Generic.List<string>();
            if (baik > 0) rincian.Add($"{AssetCondition.Baik} ({baik})");
            if (perbaikan > 0) rincian.Add($"{AssetCondition.Perbaikan} ({perbaikan})");
            if (rusak > 0) rincian.Add($"{AssetCondition.Rusak} ({rusak})");
            return string.Join(", ", rincian);
        }

        // Mengatur teks beserta sisa stok yang tampil saat objek dimasukkan ke ComboBox
        public override string ToString()
        {
            return $"{KodeAset} - {NamaAset} (Sisa: {Tersedia})";
        }
    }

    // Class model untuk menyimpan data satu karyawan
    public class KaryawanItem
    {
        public string Nik { get; set; }
        public string Nama { get; set; }
        public string Divisi { get; set; }
        public string Jabatan { get; set; }
        public string Kontak { get; set; }

        // Mengatur teks yang tampil saat objek dimasukkan ke ComboBox
        public override string ToString()
        {
            return Nik + " - " + Nama + " (" + Divisi + ")";
        }
    }

    // Class model untuk menyimpan data transaksi peminjaman (alokasi) aset beserta jumlah unitnya
    public class AlokasiItem
    {
        public string IdAlokasi { get; set; }
        public string KodeAset { get; set; }
        public string NamaAset { get; set; }
        public int Jumlah { get; set; } = 1;
        public string NikKaryawan { get; set; }
        public string NamaKaryawan { get; set; }
        public string Divisi { get; set; }
        public DateTime TanggalPinjam { get; set; }
        public string Keterangan { get; set; }

        // Status alokasi (Aktif / Dikembalikan), disembunyikan dari kolom tabel
        [Browsable(false)]
        public string StatusAlokasi { get; set; }

        // Mengecek apakah transaksi pinjaman masih berjalan
        [Browsable(false)]
        public bool IsActive
        {
            get { return StatusAlokasi == AlokasiStatus.Aktif; }
        }
    }

    // Class model untuk menyimpan satu catatan riwayat aktivitas beserta jumlah unit asetnya
    public class RiwayatItem
    {
        public DateTime Waktu { get; set; }
        public string JenisAksi { get; set; }
        public string KodeAset { get; set; }
        public string NamaAset { get; set; }
        public int Jumlah { get; set; } = 1;
        public string NamaKaryawan { get; set; }
        public string Catatan { get; set; }
        public string OlehUser { get; set; }
    }

    // Daftar pilihan level hak akses pengguna
    public enum UserRole
    {
        Admin,
        Staff
    }

    // Class untuk menyimpan data sesi pengguna yang sedang login
    public class UserSession
    {
        // Menyimpan sesi pengguna yang aktif saat ini
        public static UserSession Current { get; set; }

        public string Username { get; set; }
        public string NamaLengkap { get; set; }
        public UserRole Role { get; set; }

        public UserSession(string username, UserRole role, string namaLengkap = null)
        {
            Username = username;
            Role = role;
            NamaLengkap = string.IsNullOrWhiteSpace(namaLengkap) ? username : namaLengkap.Trim();
        }

        // Nama pengguna yang ditampilkan di seluruh antarmuka aplikasi (dikaitkan dengan username)
        public string DisplayName
        {
            get { return string.IsNullOrWhiteSpace(NamaLengkap) ? Username : NamaLengkap; }
        }

        // Bernilai true jika akun yang login adalah Admin
        public bool IsAdmin
        {
            get { return Role == UserRole.Admin; }
        }

        // Format teks profil pengguna (Nama Pengguna + Role) di bagian kanan atas MainForm
        public string ProfileText
        {
            get { return DisplayName + " (" + (IsAdmin ? "Admin IT" : "Staff IT") + ")"; }
        }
    }

    // Class pembungkus hasil proses di Controller (berisi status sukses/gagal dan pesan)
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        // Membuat hasil sukses beserta pesannya
        public static OperationResult Ok(string message)
        {
            return new OperationResult(true, message);
        }

        // Membuat hasil gagal beserta pesan peringatannya
        public static OperationResult Fail(string message)
        {
            return new OperationResult(false, message);
        }
    }
}
