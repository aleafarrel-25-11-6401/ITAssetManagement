using System;
using System.Windows.Forms;
using ITAssetManagement.Models;
using ITAssetManagement.Controllers;

namespace ITAssetManagement.Views
{
    // Halaman Alokasi untuk melayani peminjaman aset (multi-unit) dan pengembalian aset oleh Admin maupun Staff
    public partial class UC_Alokasi : UserControl
    {
        // Objek controller untuk mengelola aset, karyawan, dan transaksi alokasi
        private readonly AssetController _assetCtrl = new AssetController();
        private readonly KaryawanController _karyawanCtrl = new KaryawanController();
        private readonly AlokasiController _alokasiCtrl = new AlokasiController();

        public UC_Alokasi()
        {
            InitializeComponent();
        }

        // Mengambil nama tampilan pengguna yang sedang login untuk dicatat pada riwayat
        private string AmbilUsername()
        {
            return UserSession.Current != null ? UserSession.Current.DisplayName : "Administrator IT";
        }

        // Memuat daftar aset siap pinjam, karyawan, dan tabel alokasi saat halaman dibuka (Admin & Staff)
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            cmbKondisiKembali.SelectedIndex = 0;
            RefreshData();
        }

        // Memperbarui pilihan ComboBox aset tersedia, batas maksimum jumlah pinjam, karyawan, dan tabel pinjaman aktif
        private void RefreshData()
        {
            var readyAssets = _assetCtrl.GetReadyToLend();
            var karyawanList = _karyawanCtrl.GetAll();

            if (readyAssets.Count > 0)
            {
                cmbAset.DataSource = readyAssets;
                cmbAset.Enabled = true;
            }
            else
            {
                cmbAset.DataSource = null;
                cmbAset.Items.Clear();
                cmbAset.Items.Add("-- Tidak ada aset siap pinjam --");
                cmbAset.SelectedIndex = 0;
                cmbAset.Enabled = false;
            }

            SyncBatasJumlahPinjam();

            if (karyawanList.Count > 0)
            {
                cmbKaryawan.DataSource = karyawanList;
                cmbKaryawan.Enabled = true;
            }
            else
            {
                cmbKaryawan.DataSource = null;
                cmbKaryawan.Items.Clear();
                cmbKaryawan.Items.Add("-- Belum ada data karyawan --");
                cmbKaryawan.SelectedIndex = 0;
                cmbKaryawan.Enabled = false;
            }

            btnAlokasi.Enabled = readyAssets.Count > 0 && karyawanList.Count > 0;

            var activeLoans = _alokasiCtrl.GetActive();
            UIHelper.BindGrid(dgvAlokasi, activeLoans, "Belum ada aset yang sedang dipinjam saat ini.");
            btnKembalikan.Enabled = activeLoans.Count > 0;
            cmbKondisiKembali.Enabled = activeLoans.Count > 0;
            SyncBatasJumlahKondisi();
        }

        // Menyesuaikan batas maksimum NumericUpDown agar tidak bisa melebihi stok tersedia dari aset yang dipilih
        private void SyncBatasJumlahPinjam()
        {
            AssetItem aset = cmbAset.SelectedItem as AssetItem;
            if (aset != null && aset.Tersedia >= 1)
            {
                nudJumlahPinjam.Minimum = 1;
                nudJumlahPinjam.Maximum = aset.Tersedia;
                nudJumlahPinjam.Value = 1;
                nudJumlahPinjam.Enabled = true;
                lblJumlahPinjam.Text = $"Jumlah Pinjam (Maks: {aset.Tersedia})";
            }
            else
            {
                nudJumlahPinjam.Minimum = 1;
                nudJumlahPinjam.Maximum = 1;
                nudJumlahPinjam.Value = 1;
                nudJumlahPinjam.Enabled = false;
                lblJumlahPinjam.Text = "Jumlah Pinjam";
            }
        }

        // Event saat pilihan aset berubah untuk memperbarui batas maksimum jumlah unit yang bisa dipinjam
        private void cmbAset_SelectedIndexChanged(object sender, EventArgs e)
        {
            SyncBatasJumlahPinjam();
        }

        // Event klik tombol Alokasikan Aset untuk meminjamkan sejumlah unit aset terpilih ke karyawan
        private void btnAlokasi_Click(object sender, EventArgs e)
        {
            AssetItem aset = cmbAset.SelectedItem as AssetItem;
            KaryawanItem karyawan = cmbKaryawan.SelectedItem as KaryawanItem;
            int jumlahPinjam = (int)nudJumlahPinjam.Value;

            OperationResult result = _alokasiCtrl.Allocate(aset, karyawan, dtpTanggal.Value, txtKeterangan.Text, AmbilUsername(), jumlahPinjam);
            Dialogs.Show(result);

            if (result.Success)
            {
                txtKeterangan.Clear();
                RefreshData();
            }
        }

        // Event klik tombol Kembalikan Aset untuk memproses pengembalian aset terpilih pada tabel
        private void btnKembalikan_Click(object sender, EventArgs e)
        {
            AlokasiItem loan = null;
            if (dgvAlokasi.SelectedRows.Count > 0)
            {
                loan = dgvAlokasi.SelectedRows[0].DataBoundItem as AlokasiItem;
            }

            if (loan == null)
            {
                Dialogs.Warn("Pilih baris aset pada tabel yang ingin dikembalikan!");
                return;
            }

            string kondisi = cmbKondisiKembali.Text;
            int jumlahKondisi = (int)nudJumlahKondisi.Value;
            string rincian = (kondisi == AssetCondition.Baik || jumlahKondisi >= loan.Jumlah)
                ? $"seluruhnya berkondisi '{kondisi}'"
                : $"{jumlahKondisi} unit berkondisi '{kondisi}', sisanya Baik";
            string pesan = $"Kembalikan {loan.Jumlah} unit aset {loan.KodeAset} - {loan.NamaAset} dari {loan.NamaKaryawan} ({rincian})?";
            if (!Dialogs.Confirm(pesan, "Konfirmasi Pengembalian")) return;

            OperationResult result = _alokasiCtrl.Return(loan, kondisi, AmbilUsername(), jumlahKondisi);
            Dialogs.Show(result);
            RefreshData();
        }

        // Membatasi jumlah unit berkondisi khusus agar tidak melebihi jumlah unit pada transaksi yang dipilih
        private void SyncBatasJumlahKondisi()
        {
            AlokasiItem loan = dgvAlokasi.SelectedRows.Count > 0 ? dgvAlokasi.SelectedRows[0].DataBoundItem as AlokasiItem : null;
            int maks = loan != null ? Math.Max(1, loan.Jumlah) : 1;
            nudJumlahKondisi.Maximum = maks;
            nudJumlahKondisi.Value = maks;
            nudJumlahKondisi.Enabled = loan != null && cmbKondisiKembali.Text != AssetCondition.Baik && maks > 1;
        }

        // Event saat baris pinjaman atau pilihan kondisi kembali berubah
        private void KondisiKembali_Changed(object sender, EventArgs e)
        {
            SyncBatasJumlahKondisi();
        }
    }
}
