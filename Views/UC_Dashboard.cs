using System;
using System.Linq;
using System.Windows.Forms;
using ITAssetManagement.Controllers;
using ITAssetManagement.Models;

namespace ITAssetManagement.Views
{
    // Halaman Dashboard untuk menampilkan ringkasan statistik aset, pinjaman aktif, dan riwayat terbaru
    public partial class UC_Dashboard : UserControl
    {
        private readonly AssetController _assetCtrl = new AssetController();
        private readonly AlokasiController _alokasiCtrl = new AlokasiController();
        private readonly RiwayatController _riwayatCtrl = new RiwayatController();

        public UC_Dashboard()
        {
            InitializeComponent();
        }

        // Menghitung angka statistik dan mengisi kedua tabel saat halaman Dashboard dibuka
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            var daftarAset = _assetCtrl.GetAll();
            lblTotalValue.Text = daftarAset.Sum(a => a.Jumlah).ToString();
            lblTersediaValue.Text = daftarAset.Sum(a => a.Tersedia).ToString();
            lblDipinjamValue.Text = daftarAset.Sum(a => a.Dipinjam).ToString();
            lblPerbaikanValue.Text = daftarAset.Sum(a => a.Bermasalah).ToString();

            UIHelper.BindGrid(dgvAlokasiAktif, _alokasiCtrl.GetActive(), "Tidak ada aset yang sedang dipinjam saat ini.");
            UIHelper.BindGrid(dgvRiwayatTerbaru, _riwayatCtrl.Latest(10), "Belum ada catatan riwayat aktivitas terbaru.");
        }
    }
}
