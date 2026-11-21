using System;
using System.Windows.Forms;
using ITAssetManagement.Controllers;
using ITAssetManagement.Models;

namespace ITAssetManagement.Views
{
    // Halaman Profil Akun untuk mengubah username, nama pengguna (tampilan), dan/atau password pengguna yang sedang login
    public partial class UC_Profil : UserControl
    {
        private readonly AuthController _authCtrl = new AuthController();

        public UC_Profil()
        {
            InitializeComponent();
        }

        // Memuat informasi profil pengguna aktif saat halaman dibuka
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            ResetForm();
        }

        // Menampilkan username, nama pengguna, & role saat ini serta mengosongkan kolom input password
        private void ResetForm()
        {
            UserSession user = UserSession.Current;
            string username = user != null ? user.Username : "";
            string namaLengkap = user != null ? user.DisplayName : "";
            string role = user != null ? (user.IsAdmin ? "Admin IT" : "Staff IT") : "-";

            txtUsernameAktif.Text = username;
            txtNamaAktif.Text = namaLengkap;
            txtRoleAktif.Text = role;

            txtUsernameBaru.Text = username;
            txtNamaBaru.Text = namaLengkap;
            txtPasswordLama.Clear();
            txtPasswordBaru.Clear();
            txtKonfirmasiPassword.Clear();
        }

        // Event klik tombol Simpan Perubahan untuk memperbarui profil di Supabase
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            string currentUsername = UserSession.Current != null ? UserSession.Current.Username : txtUsernameAktif.Text;

            OperationResult result = _authCtrl.UpdateProfile(
                currentUsername,
                txtUsernameBaru.Text,
                txtNamaBaru.Text,
                txtPasswordLama.Text,
                txtPasswordBaru.Text,
                txtKonfirmasiPassword.Text);

            Dialogs.Show(result);

            if (result.Success)
            {
                MainForm mainForm = FindForm() as MainForm;
                if (mainForm != null)
                {
                    mainForm.RefreshProfileHeader();
                }

                ResetForm();
            }
        }

        // Event klik tombol Reset Form
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
    }
}
