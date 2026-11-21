using System;
using System.Windows.Forms;
using ITAssetManagement.Models;
using ITAssetManagement.Controllers;

namespace ITAssetManagement.Views
{
    // Halaman Manajemen Aset untuk menambah, mengubah, menghapus, dan mencari data aset IT
    public partial class UC_Asset : UserControl
    {
        private const string KondisiCampuranLabel = "Campuran (Sebagian)";

        // Objek controller aset, item yang sedang dipilih, dan flag pencegah loop sinkronisasi kontrol
        private readonly AssetController _assetCtrl = new AssetController();
        private AssetItem _selected;
        private bool _syncingKondisi;

        public UC_Asset()
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

        // Memuat ulang isi tabel aset sesuai kata kunci pencarian beserta empty state
        private void LoadGrid()
        {
            string keyword = txtCari.Text;
            string pesanKosong = string.IsNullOrWhiteSpace(keyword)
                ? "Belum ada data aset IT yang terdaftar."
                : $"Tidak ditemukan aset dengan kata kunci \"{keyword.Trim()}\".";
            UIHelper.BindGrid(dgvAset, _assetCtrl.GetAll(keyword), pesanKosong);
        }

        // Mengambil nama tampilan pengguna aktif untuk pencatatan log riwayat
        private string CurrentUser()
        {
            return UserSession.Current != null ? UserSession.Current.DisplayName : "Administrator IT";
        }

        // Menyelaraskan pilihan preset kondisi, jumlah unit Perbaikan/Rusak, dan ringkasan stok tanpa deadlock
        private void SyncKondisiUI(object source = null)
        {
            if (_syncingKondisi) return;
            _syncingKondisi = true;
            try
            {
                int totalUnit = (int)nudJumlah.Value;
                int dipinjam = _selected != null ? _selected.Dipinjam : 0;
                int diGudang = Math.Max(0, totalUnit - dipinjam);

                nudPerbaikan.Maximum = diGudang;
                nudRusak.Maximum = diGudang;

                int perbaikan = Math.Max(0, Math.Min((int)nudPerbaikan.Value, diGudang));
                int rusak = Math.Max(0, Math.Min((int)nudRusak.Value, diGudang));

                if (source == cmbKondisi && cmbKondisi.Text != KondisiCampuranLabel)
                {
                    perbaikan = cmbKondisi.Text == AssetCondition.Perbaikan ? diGudang : 0;
                    rusak = cmbKondisi.Text == AssetCondition.Rusak ? diGudang : 0;
                }
                else if (perbaikan + rusak > diGudang)
                {
                    // Dorong turun kontrol lawan agar perubahan unit tetap mulus saat total gudang penuh
                    if (source == nudPerbaikan)
                    {
                        rusak = Math.Max(0, diGudang - perbaikan);
                    }
                    else if (source == nudRusak)
                    {
                        perbaikan = Math.Max(0, diGudang - rusak);
                    }
                    else
                    {
                        perbaikan = Math.Min(perbaikan, diGudang);
                        rusak = Math.Min(rusak, diGudang - perbaikan);
                    }
                }

                nudPerbaikan.Value = perbaikan;
                nudRusak.Value = rusak;

                bool adaGudang = diGudang > 0;
                cmbKondisi.Enabled = adaGudang;
                nudPerbaikan.Enabled = adaGudang;
                nudRusak.Enabled = adaGudang;

                int baik = Math.Max(0, diGudang - perbaikan - rusak);
                string presetTarget = AssetCondition.Baik;
                if (adaGudang)
                {
                    if (perbaikan == 0 && rusak == 0) presetTarget = AssetCondition.Baik;
                    else if (perbaikan == diGudang && rusak == 0) presetTarget = AssetCondition.Perbaikan;
                    else if (rusak == diGudang && perbaikan == 0) presetTarget = AssetCondition.Rusak;
                    else presetTarget = KondisiCampuranLabel;
                }

                if (presetTarget == KondisiCampuranLabel)
                {
                    if (!cmbKondisi.Items.Contains(KondisiCampuranLabel))
                    {
                        cmbKondisi.Items.Add(KondisiCampuranLabel);
                    }
                }
                else
                {
                    cmbKondisi.Items.Remove(KondisiCampuranLabel);
                }
                cmbKondisi.SelectedItem = presetTarget;

                lblRincianStok.Text = adaGudang
                    ? $"Baik: {baik} | Perbaikan: {perbaikan} | Rusak: {rusak}" + (dipinjam > 0 ? $" | Pinjam: {dipinjam}" : "")
                    : (totalUnit < dipinjam ? $"Min. {dipinjam} unit (sedang dipinjam!)" : $"Semua {dipinjam} unit sedang dipinjam");
            }
            finally
            {
                _syncingKondisi = false;
            }
        }

        // Mengosongkan form input dan menampilkan kode aset urut berikutnya
        private void ResetForm()
        {
            _syncingKondisi = true;
            try
            {
                _selected = null;
                txtKodeAset.Text = _assetCtrl.NextCode();
                txtNamaAset.Clear();
                cmbKategori.SelectedIndex = 0;
                nudJumlah.Value = 1;
                nudPerbaikan.Maximum = 1;
                nudRusak.Maximum = 1;
                nudPerbaikan.Value = 0;
                nudRusak.Value = 0;
                ResetPilihanKondisi();
                cmbKondisi.SelectedIndex = 0;
            }
            finally
            {
                _syncingKondisi = false;
            }

            SyncKondisiUI();
            UIHelper.ClearSelection(dgvAset);
        }

        // Mengembalikan pilihan standar pada dropdown Kondisi ("Baik", "Perbaikan", "Rusak")
        private void ResetPilihanKondisi()
        {
            cmbKondisi.Items.Clear();
            cmbKondisi.Items.AddRange(new object[]
            {
                AssetCondition.Baik,
                AssetCondition.Perbaikan,
                AssetCondition.Rusak
            });
        }

        private void cmbKondisi_SelectedIndexChanged(object sender, EventArgs e)
        {
            SyncKondisiUI(cmbKondisi);
        }

        private void KondisiUnit_Changed(object sender, EventArgs e)
        {
            SyncKondisiUI(sender);
        }

        // Event klik tombol Tambah Aset
        private void btnTambah_Click(object sender, EventArgs e)
        {
            OperationResult result = _assetCtrl.Add(
                txtNamaAset.Text,
                cmbKategori.Text,
                cmbKondisi.Text,
                CurrentUser(),
                (int)nudJumlah.Value,
                (int)nudPerbaikan.Value,
                (int)nudRusak.Value);
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
                Dialogs.Warn("Klik baris aset pada tabel terlebih dahulu sebelum mengubah!");
                return;
            }

            OperationResult result = _assetCtrl.Update(
                _selected,
                txtNamaAset.Text,
                cmbKategori.Text,
                cmbKondisi.Text,
                (int)nudJumlah.Value,
                (int)nudPerbaikan.Value,
                (int)nudRusak.Value);
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
                Dialogs.Warn("Klik baris aset pada tabel terlebih dahulu sebelum menghapus!");
                return;
            }

            string pesan = "Hapus aset " + _selected.KodeAset + " - " + _selected.NamaAset + "?";
            if (!Dialogs.Confirm(pesan, "Peringatan Hapus Aset", true)) return;

            OperationResult result = _assetCtrl.Delete(_selected, CurrentUser());
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

        // Event klik baris tabel untuk menyalin data aset ke kotak input form
        private void dgvAset_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvAset.Rows.Count) return;

            _selected = dgvAset.Rows[e.RowIndex].DataBoundItem as AssetItem;
            if (_selected == null) return;

            int diGudang = Math.Max(0, _selected.Jumlah - _selected.Dipinjam);
            int perbaikan = _selected.Perbaikan;
            int rusak = _selected.Rusak;
            if (perbaikan == 0 && rusak == 0 && diGudang > 0)
            {
                if (_selected.Kondisi == AssetCondition.Perbaikan) perbaikan = diGudang;
                else if (_selected.Kondisi == AssetCondition.Rusak) rusak = diGudang;
            }

            _syncingKondisi = true;
            try
            {
                txtKodeAset.Text = _selected.KodeAset;
                txtNamaAset.Text = _selected.NamaAset;
                cmbKategori.Text = _selected.Kategori;
                nudJumlah.Value = Math.Max(1, Math.Min((int)nudJumlah.Maximum, _selected.Jumlah));
                nudPerbaikan.Maximum = Math.Max(diGudang, perbaikan);
                nudRusak.Maximum = Math.Max(diGudang, rusak);
                nudPerbaikan.Value = Math.Max(0, Math.Min((int)nudPerbaikan.Maximum, perbaikan));
                nudRusak.Value = Math.Max(0, Math.Min((int)nudRusak.Maximum, rusak));
                ResetPilihanKondisi();
            }
            finally
            {
                _syncingKondisi = false;
            }

            SyncKondisiUI();
        }
    }
}
