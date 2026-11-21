using System;
using System.Windows.Forms;
using ITAssetManagement.Controllers;
using ITAssetManagement.Models;

namespace ITAssetManagement.Views
{
    // Halaman Riwayat untuk menampilkan, memfilter, dan membersihkan log aktivitas keluar-masuk aset
    public partial class UC_Riwayat : UserControl
    {
        // Objek controller untuk mengambil, memfilter, dan membersihkan data riwayat
        private readonly RiwayatController _riwayatCtrl = new RiwayatController();

        public UC_Riwayat()
        {
            InitializeComponent();
        }

        // Mengatur visibilitas tombol Bersihkan Riwayat (khusus Admin) dan memuat daftar riwayat
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            bool isAdmin = UserSession.Current == null || UserSession.Current.IsAdmin;
            btnBersihkan.Visible = isAdmin;

            if (!isAdmin)
            {
                txtCari.Width = dgvRiwayat.Right - txtCari.Left;
            }

            cmbFilterAksi.SelectedIndex = 0;
        }

        // Memuat isi tabel riwayat sesuai filter jenis aksi dan kata kunci pencarian
        private void LoadGrid()
        {
            string aksi = (cmbFilterAksi.SelectedIndex > 0) ? cmbFilterAksi.Text : null;
            bool isFiltered = !string.IsNullOrWhiteSpace(aksi) || !string.IsNullOrWhiteSpace(txtCari.Text);
            string emptyMsg = isFiltered
                ? "Tidak ada catatan riwayat yang cocok dengan filter pencarian."
                : "Belum ada catatan riwayat aktivitas keluar-masuk aset.";

            var results = _riwayatCtrl.Search(aksi, txtCari.Text);
            UIHelper.BindGrid(dgvRiwayat, results, emptyMsg);
            btnBersihkan.Enabled = _riwayatCtrl.Latest(1).Count > 0;
        }

        // Event saat pilihan filter berubah (gunakan debounce 300ms khusus saat mengetik pencarian)
        private void Filter_Changed(object sender, EventArgs e)
        {
            if (sender == txtCari)
            {
                UIHelper.Debounce(LoadGrid);
            }
            else
            {
                LoadGrid();
            }
        }

        // Event klik tombol Bersihkan Riwayat (Khusus Admin)
        private void btnBersihkan_Click(object sender, EventArgs e)
        {
            if (dgvRiwayat.Rows.Count == 0 && string.IsNullOrWhiteSpace(txtCari.Text) && cmbFilterAksi.SelectedIndex <= 0)
            {
                Dialogs.Warn("Data riwayat sudah kosong!");
                return;
            }

            if (!Dialogs.Confirm("Apakah Anda yakin ingin menghapus seluruh catatan riwayat aktivitas?\nTindakan ini tidak dapat dibatalkan.", "Peringatan Bersihkan Riwayat", true))
            {
                return;
            }

            bool isAdmin = UserSession.Current == null || UserSession.Current.IsAdmin;
            OperationResult result = _riwayatCtrl.ClearAll(isAdmin);
            Dialogs.Show(result);

            if (result.Success)
            {
                txtCari.Clear();
                cmbFilterAksi.SelectedIndex = 0;
                LoadGrid();
            }
        }
    }
}
