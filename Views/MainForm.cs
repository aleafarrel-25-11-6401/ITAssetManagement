using System;
using System.Drawing;
using System.Windows.Forms;
using ITAssetManagement.Models;

namespace ITAssetManagement.Views
{
    // Form utama aplikasi yang memuat navigasi sidebar kiri dan halaman konten di kanan
    public partial class MainForm : Form
    {
        // Array daftar tombol menu navigasi pada sidebar kiri
        private readonly Button[] _menus;

        // Penanda apakah MainForm ditutup karena klik Logout (true) atau tombol X (false)
        public bool IsLoggingOut { get; private set; }

        public MainForm()
        {
            InitializeComponent();

            _menus = new Button[] { btnDashboard, btnAsset, btnKaryawan, btnAlokasi, btnRiwayat, btnProfil };

            UserSession user = UserSession.Current;
            if (user == null) return;

            RefreshProfileHeader();
            ArrangeMenus(user.IsAdmin);
            ShowPage(btnDashboard);
        }

        // Memperbarui teks profil pengguna di bagian kanan atas header MainForm
        public void RefreshProfileHeader()
        {
            if (UserSession.Current != null)
            {
                lblProfil.Text = UserSession.Current.ProfileText;
            }
        }

        // Menyembunyikan menu khusus Admin jika login sebagai Staff lalu merapatkan posisi tombol ke atas
        private void ArrangeMenus(bool isAdmin)
        {
            int posisiY = 126;

            foreach (Button menu in _menus)
            {
                bool tampilkan = isAdmin || (menu != btnAsset && menu != btnKaryawan);
                menu.Visible = tampilkan;

                if (tampilkan)
                {
                    menu.Top = posisiY;
                    posisiY += menu.Height + 6;
                }
            }
        }

        // Membuat objek halaman UserControl sesuai tombol menu yang diklik
        private UserControl CreatePage(Button menu)
        {
            if (menu == btnAsset) return new UC_Asset();
            if (menu == btnKaryawan) return new UC_Karyawan();
            if (menu == btnAlokasi) return new UC_Alokasi();
            if (menu == btnRiwayat) return new UC_Riwayat();
            if (menu == btnProfil) return new UC_Profil();
            return new UC_Dashboard();
        }

        // Menampilkan halaman UserControl terpilih di dalam pnlHalaman
        private void ShowPage(Button activeMenu)
        {
            UIHelper.CancelDebounce();
            lblHalaman.Text = activeMenu.Text;

            foreach (Button menu in _menus)
            {
                bool aktif = (menu == activeMenu);
                menu.BackColor = aktif ? Color.FromArgb(128, 128, 255) : Color.White;
                menu.ForeColor = aktif ? Color.White : Color.Black;
                menu.FlatStyle = aktif ? FlatStyle.Flat : FlatStyle.Standard;
                menu.UseVisualStyleBackColor = !aktif;
            }

            while (pnlHalaman.Controls.Count > 0)
            {
                pnlHalaman.Controls[0].Dispose();
            }

            // Muat ulang dari server hanya jika masa berlaku cache (60 detik) habis agar pindah halaman tetap ringan
            DataStore.ClearExpiredCache();
            UserControl page = CreatePage(activeMenu);
            page.Dock = DockStyle.Fill;
            pnlHalaman.Controls.Add(page);
        }

        // Event klik bersama untuk semua tombol menu di sidebar
        private void btnMenu_Click(object sender, EventArgs e)
        {
            Button clickedMenu = sender as Button;
            if (clickedMenu != null)
            {
                ShowPage(clickedMenu);
            }
        }

        // Event klik tombol Logout untuk mengosongkan cache dan kembali ke FormLogin
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (!Dialogs.Confirm("Apakah Anda yakin ingin keluar (Logout)?", "Konfirmasi Logout"))
            {
                return;
            }

            UIHelper.CancelDebounce();
            DataStore.ClearCache();
            IsLoggingOut = true;
            Close();
        }
    }
}
