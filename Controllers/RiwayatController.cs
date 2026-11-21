using System;
using System.Collections.Generic;
using System.Linq;
using ITAssetManagement.Models;

namespace ITAssetManagement.Controllers
{
    // Controller untuk mencatat dan menyaring log riwayat aktivitas aset
    public class RiwayatController
    {
        // Menambahkan catatan aktivitas baru beserta jumlah unitnya ke tabel 'riwayat' di Supabase dan cache memori
        public void Add(string aksi, string kodeAset, string namaAset, string karyawan, string catatan, string user, int jumlah = 1)
        {
            DateTime sekarang = DateTime.Now;
            var item = new RiwayatItem
            {
                Waktu = sekarang,
                JenisAksi = aksi ?? "-",
                KodeAset = kodeAset ?? "-",
                NamaAset = namaAset ?? "-",
                Jumlah = Math.Max(1, jumlah),
                NamaKaryawan = karyawan ?? "-",
                Catatan = catatan ?? "-",
                OlehUser = string.IsNullOrWhiteSpace(user) ? "admin" : user.Trim()
            };

            try
            {
                string usernamePelaku = (UserSession.Current != null && !string.IsNullOrWhiteSpace(UserSession.Current.Username))
                    ? UserSession.Current.Username.Trim()
                    : item.OlehUser;

                DataStore.Insert("riwayat", new
                {
                    waktu = sekarang.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    jenis_aksi = item.JenisAksi,
                    kode_aset = item.KodeAset,
                    nama_aset = item.NamaAset,
                    jumlah = item.Jumlah,
                    nama_karyawan = item.NamaKaryawan,
                    catatan = item.Catatan,
                    oleh_user = item.OlehUser,
                    username_pelaku = usernamePelaku
                });
            }
            catch
            {
                // Jangan hentikan transaksi utama jika pencatatan log riwayat terkendala jaringan
            }

            DataStore.GetRiwayat().Insert(0, item);
        }

        // Menyaring daftar riwayat berdasarkan filter jenis aksi dan kata kunci pencarian
        public List<RiwayatItem> Search(string aksi, string keyword)
        {
            keyword = (keyword ?? "").Trim().ToLower();
            return DataStore.GetRiwayat()
                .Where(r => string.IsNullOrEmpty(aksi) || r.JenisAksi == aksi)
                .Where(r => (r.JenisAksi ?? "").ToLower().Contains(keyword)
                         || (r.KodeAset ?? "").ToLower().Contains(keyword)
                         || (r.NamaAset ?? "").ToLower().Contains(keyword)
                         || (r.NamaKaryawan ?? "").ToLower().Contains(keyword)
                         || (r.Catatan ?? "").ToLower().Contains(keyword)
                         || (r.OlehUser ?? "").ToLower().Contains(keyword))
                .ToList();
        }

        // Mengambil sejumlah baris riwayat terbaru untuk ditampilkan pada halaman Dashboard
        public List<RiwayatItem> Latest(int jumlah) => DataStore.GetRiwayat().Take(Math.Max(0, jumlah)).ToList();

        // Menghapus seluruh catatan riwayat aktivitas di Supabase dan cache memori (Khusus Admin)
        public OperationResult ClearAll(bool isAdmin)
        {
            if (!isAdmin)
                return OperationResult.Fail("Hanya Admin yang memiliki akses untuk membersihkan riwayat!");

            if (DataStore.GetRiwayat().Count == 0)
                return OperationResult.Fail("Data riwayat sudah kosong!");

            try
            {
                DataStore.Delete("riwayat?waktu=not.is.null");
                DataStore.GetRiwayat().Clear();
                return OperationResult.Ok("Seluruh catatan riwayat berhasil dibersihkan!");
            }
            catch
            {
                return OperationResult.Fail("Gagal membersihkan data riwayat di server. Periksa koneksi internet Anda.");
            }
        }
    }
}
