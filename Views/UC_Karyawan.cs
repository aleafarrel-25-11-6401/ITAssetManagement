using System;
using System.Windows.Forms;
using ITAssetManagement.Models;
using ITAssetManagement.Controllers;

namespace ITAssetManagement.Views
{
    // Halaman Karyawan untuk menambah, mengubah, menghapus, dan mencari data karyawan
    public partial class UC_Karyawan : UserControl
    {
        // Objek controller karyawan dan variabel penyimpan baris karyawan yang sedang dipilih dari tabel
        private readonly KaryawanController _karyawanCtrl = new KaryawanController();
        private KaryawanItem _selected;

        public UC_Karyawan()
        {
            InitializeComponent();
        }

        // Memuat data tabel dan mereset form saat halaman dibuka
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            LoadGrid();
            ResetForm();
        }

        // Memuat ulang isi tabel karyawan sesuai kata kunci pencarian
        private void LoadGrid()
        {
            string emptyMsg = string.IsNullOrWhiteSpace(txtCari.Text)
                ? "Belum ada data karyawan yang terdaftar. Silakan tambahkan karyawan baru melalui form di atas."
                : "Tidak ditemukan karyawan yang cocok dengan kata kunci \"" + txtCari.Text.Trim() + "\".";
            UIHelper.BindGrid(dgvKaryawan, _karyawanCtrl.GetAll(txtCari.Text), emptyMsg);
        }

        // Mengosongkan form input dan menampilkan NIK karyawan urut berikutnya
        private void ResetForm()
        {
            _selected = null;
            txtNik.Text = _karyawanCtrl.NextNik();
            txtNama.Clear();
            cmbDivisi.SelectedIndex = 0;
            txtJabatan.Clear();
            txtKontak.Clear();
            UIHelper.ClearSelection(dgvKaryawan);
        }

        // Event klik tombol Tambah Karyawan
        private void btnTambah_Click(object sender, EventArgs e)
        {
            OperationResult result = _karyawanCtrl.Add(txtNama.Text, cmbDivisi.Text, txtJabatan.Text, txtKontak.Text);
            Dialogs.Show(result);

            if (result.Success)
            {
                LoadGrid();
                ResetForm();
            }
        }

        // Event klik tombol Simpan Ubah
        private void btnUbah_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                Dialogs.Warn("Klik baris karyawan pada tabel terlebih dahulu sebelum mengubah!");
                return;
            }

            OperationResult result = _karyawanCtrl.Update(_selected, txtNama.Text, cmbDivisi.Text, txtJabatan.Text, txtKontak.Text);
            Dialogs.Show(result);

            if (result.Success)
            {
                LoadGrid();
                ResetForm();
            }
        }

        // Event klik tombol Hapus
        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                Dialogs.Warn("Klik baris karyawan pada tabel terlebih dahulu sebelum menghapus!");
                return;
            }

            if (!Dialogs.Confirm("Hapus data karyawan " + _selected.Nama + "?", "Peringatan Hapus Karyawan", true)) return;

            OperationResult result = _karyawanCtrl.Delete(_selected);
            Dialogs.Show(result);

            if (result.Success)
            {
                LoadGrid();
                ResetForm();
            }
        }

        // Event klik tombol Reset Form
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        // Event ketik kotak pencarian dengan jeda 300ms (debounce)
        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            UIHelper.Debounce(LoadGrid);
        }

        // Event klik baris tabel untuk menyalin data karyawan ke kotak input form
        private void dgvKaryawan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvKaryawan.Rows.Count) return;

            _selected = dgvKaryawan.Rows[e.RowIndex].DataBoundItem as KaryawanItem;
            if (_selected == null) return;

            txtNik.Text = _selected.Nik;
            txtNama.Text = _selected.Nama;
            cmbDivisi.Text = _selected.Divisi;
            txtJabatan.Text = _selected.Jabatan;
            txtKontak.Text = _selected.Kontak;
        }
    }
}
