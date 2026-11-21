using System.Windows.Forms;
using ITAssetManagement.Models;

namespace ITAssetManagement.Views
{
    // Class pembantu untuk menampilkan kotak dialog pesan (MessageBox)
    public static class Dialogs
    {
        // Menampilkan kotak pesan informasi berhasil
        public static void Info(string message, string title = "Sukses")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Menampilkan kotak pesan peringatan validasi
        public static void Warn(string message, string title = "Peringatan")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // Menampilkan kotak pesan error/kesalahan
        public static void Error(string message, string title = "Gagal")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Menampilkan kotak konfirmasi Yes/No (gunakan isWarning = true untuk tindakan hapus/berbahaya)
        public static bool Confirm(string message, string title = "Konfirmasi", bool isWarning = false)
        {
            MessageBoxIcon icon = isWarning ? MessageBoxIcon.Warning : MessageBoxIcon.Question;
            DialogResult jawaban = MessageBox.Show(message, title, MessageBoxButtons.YesNo, icon);
            return jawaban == DialogResult.Yes;
        }

        // Menampilkan pesan otomatis sesuai status OperationResult dari controller
        public static void Show(OperationResult result)
        {
            if (result.Success)
            {
                Info(result.Message);
            }
            else
            {
                Warn(result.Message);
            }
        }
    }
}
