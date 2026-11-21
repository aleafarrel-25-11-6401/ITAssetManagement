using System;
using System.Threading;
using System.Windows.Forms;
using ITAssetManagement.Views;

namespace ITAssetManagement
{
    // Class utama sebagai titik awal (entry point) program dijalankan
    internal static class Program
    {
        // Mengatur penangkap error global dan menjalankan FormLogin
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Tangkap semua error tak terduga agar tidak pernah memunculkan pesan teknis/kode sistem
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.Run(new FormLogin());
        }

        // Menangani kendala tak terduga pada tampilan tanpa membocorkan detail kode internal
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            UIHelper.HideLoading();
            Dialogs.Warn("Terjadi kendala saat memproses permintaan. Silakan coba kembali.", "Informasi Sistem");
        }

        // Menangani kendala tak terduga pada level sistem tanpa membocorkan detail kode internal
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            UIHelper.HideLoading();
            Dialogs.Warn("Terjadi kendala pada aplikasi. Silakan muat ulang halaman.", "Informasi Sistem");
        }
    }
}
