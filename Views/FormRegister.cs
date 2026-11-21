using System;
using System.Windows.Forms;
using ITAssetManagement.Controllers;
using ITAssetManagement.Models;

namespace ITAssetManagement.Views
{
    // Form halaman Daftar Akun untuk membuat akun pengguna baru dengan aman
    public partial class FormRegister : Form
    {
        private readonly AuthController _auth = new AuthController();

        // Menyimpan username (case-sensitive) yang baru berhasil didaftarkan untuk diteruskan kembali ke FormLogin
        public string RegisteredUsername { get; private set; }

        public FormRegister()
        {
            InitializeComponent();
        }

        // Event klik tombol Daftar Akun untuk memvalidasi input dan menyimpan akun ke Supabase
        private void btnDaftar_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string namaLengkap = txtNamaLengkap.Text.Trim();
            string password = txtPassword.Text;
            string konfirmasi = txtKonfirmasi.Text;

            btnDaftar.Text = "Memproses...";
            btnDaftar.Enabled = false;
            btnDaftar.Refresh();

            OperationResult result = _auth.Register(username, namaLengkap, password, konfirmasi);

            btnDaftar.Text = "Daftar Akun";
            btnDaftar.Enabled = true;

            if (!result.Success)
            {
                Dialogs.Warn(result.Message, "Pendaftaran Gagal");
                return;
            }

            Dialogs.Info(result.Message, "Pendaftaran Berhasil");
            RegisteredUsername = username;
            DialogResult = DialogResult.OK;
            Close();
        }

        // Event klik tombol Kembali ke Login
        private void btnKembali_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
