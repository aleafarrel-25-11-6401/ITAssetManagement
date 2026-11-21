using System;
using System.Windows.Forms;
using ITAssetManagement.Models;
using ITAssetManagement.Controllers;

namespace ITAssetManagement.Views
{
    // Form halaman Login untuk memvalidasi akun pengguna sebelum masuk ke MainForm
    public partial class FormLogin : Form
    {
        // Objek controller untuk memeriksa username dan password ke Supabase
        private readonly AuthController _auth = new AuthController();

        public FormLogin()
        {
            InitializeComponent();
        }

        // Event klik tombol Login untuk memvalidasi akun dan membuka halaman utama
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (username == "" || password == "")
            {
                Dialogs.Warn("Username dan Password wajib diisi!");
                return;
            }

            btnLogin.Text = "Memproses...";
            btnLogin.Enabled = false;
            btnLogin.Refresh();

            OperationResult result = _auth.Login(username, password);

            btnLogin.Text = "Login";
            btnLogin.Enabled = true;

            if (!result.Success)
            {
                Dialogs.Error(result.Message, "Login Gagal");
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            OpenMainForm();
        }

        // Membuka halaman pendaftaran akun baru (FormRegister)
        private void btnDaftar_Click(object sender, EventArgs e)
        {
            using (FormRegister formRegister = new FormRegister { Icon = Icon })
            {
                if (formRegister.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(formRegister.RegisteredUsername))
                {
                    txtUsername.Text = formRegister.RegisteredUsername;
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
        }

        // Menyembunyikan FormLogin dan menampilkan MainForm
        private void OpenMainForm()
        {
            MainForm mainForm = new MainForm();
            mainForm.FormClosed += MainForm_FormClosed;

            Hide();
            mainForm.Show();
        }

        // Mengatur aksi saat MainForm ditutup (kembali ke Login jika Logout, atau tutup aplikasi jika klik X)
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm mainForm = sender as MainForm;
            UserSession.Current = null;

            if (mainForm != null && !mainForm.IsLoggingOut)
            {
                Close();
                return;
            }

            txtPassword.Clear();
            Show();
            txtUsername.Focus();
        }
    }
}
