using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ITAssetManagement.Models;

namespace ITAssetManagement.Controllers
{
    // Controller untuk autentikasi (case-sensitive), registrasi akun baru, dan pembaruan profil pengguna di Supabase
    public class AuthController
    {
        // Mengubah teks password menjadi hash SHA-256 (64 karakter hex huruf kecil)
        public static string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password ?? ""));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }

        // Memvalidasi format username secara case-sensitive (wajib diisi, minimal 3 karakter, tanpa spasi, mempertahankan huruf besar/kecil)
        private static bool ValidateUsername(string username, out string cleanUsername, out string error)
        {
            cleanUsername = (username ?? "").Trim();
            if (cleanUsername.Length == 0)
            {
                error = "Username wajib diisi!";
                return false;
            }
            if (cleanUsername.Length < 3)
            {
                error = "Username minimal 3 karakter!";
                return false;
            }
            if (cleanUsername.Contains(" "))
            {
                error = "Username tidak boleh mengandung spasi!";
                return false;
            }
            error = null;
            return true;
        }

        // Memvalidasi nama pengguna / nama lengkap yang akan ditampilkan di aplikasi (minimal 2 karakter)
        private static bool ValidateNamaLengkap(string namaLengkap, out string cleanNama, out string error)
        {
            cleanNama = (namaLengkap ?? "").Trim();
            if (cleanNama.Length == 0)
            {
                error = "Nama Pengguna wajib diisi!";
                return false;
            }
            if (cleanNama.Length < 2)
            {
                error = "Nama Pengguna minimal 2 karakter!";
                return false;
            }
            error = null;
            return true;
        }

        // Memeriksa kecocokan username (case-sensitive) dan hash password lalu menyimpan sesi pengguna (termasuk NamaLengkap)
        public OperationResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
                return OperationResult.Fail("Username dan Password wajib diisi!");

            try
            {
                string cleanUsername = username.Trim();
                Dictionary<string, object> row = DataStore.FindUser(cleanUsername);
                string passwordHash = HashPassword(password);

                string savedUsername = DataStore.ReadString(row, "username");
                if (row == null
                    || !string.Equals(savedUsername, cleanUsername, StringComparison.Ordinal)
                    || !string.Equals(DataStore.ReadString(row, "password"), passwordHash, StringComparison.OrdinalIgnoreCase))
                {
                    return OperationResult.Fail("Username atau Password salah!");
                }

                string namaLengkap = DataStore.ReadString(row, "nama_lengkap");
                if (string.IsNullOrWhiteSpace(namaLengkap)) namaLengkap = savedUsername;

                string roleText = DataStore.ReadString(row, "role");
                UserRole role = roleText.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? UserRole.Admin : UserRole.Staff;

                UserSession.Current = new UserSession(savedUsername, role, namaLengkap);
                return OperationResult.Ok("Login berhasil!");
            }
            catch
            {
                return OperationResult.Fail("Tidak dapat terhubung ke server Supabase.\nPastikan koneksi internet Anda aktif.");
            }
        }

        // Mendaftarkan akun pengguna baru (username case-sensitive + nama pengguna tampilan) ke tabel 'app_users'
        public OperationResult Register(string username, string namaLengkap, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
                return OperationResult.Fail("Username dan Password wajib diisi!");

            if (!ValidateUsername(username, out string cleanUsername, out string errorUsername))
                return OperationResult.Fail(errorUsername);

            if (!ValidateNamaLengkap(namaLengkap, out string cleanNama, out string errorNama))
                return OperationResult.Fail(errorNama);

            if (password.Length < 6)
                return OperationResult.Fail("Password minimal 6 karakter!");

            if (password != confirmPassword)
                return OperationResult.Fail("Konfirmasi password tidak cocok!");

            try
            {
                if (DataStore.FindUser(cleanUsername) != null)
                    return OperationResult.Fail("Username sudah digunakan, silakan pilih username lain!");

                DataStore.Insert("app_users", new
                {
                    username = cleanUsername,
                    nama_lengkap = cleanNama,
                    password = HashPassword(password),
                    role = "Staff"
                });

                return OperationResult.Ok("Akun berhasil didaftarkan! Silakan login.");
            }
            catch
            {
                return OperationResult.Fail("Gagal mendaftarkan akun ke server. Periksa koneksi internet Anda.");
            }
        }

        // Memperbarui username (case-sensitive), nama pengguna (tampilan), dan/atau password akun yang sedang login
        public OperationResult UpdateProfile(
            string currentUsername,
            string newUsername,
            string newNamaLengkap,
            string currentPassword,
            string newPassword,
            string confirmNewPassword)
        {
            if (string.IsNullOrWhiteSpace(currentUsername))
                return OperationResult.Fail("Sesi pengguna tidak aktif, silakan login kembali!");

            if (!ValidateUsername(newUsername, out string cleanNewUsername, out string errorUsername))
                return OperationResult.Fail(errorUsername);

            if (!ValidateNamaLengkap(newNamaLengkap, out string cleanNewNama, out string errorNama))
                return OperationResult.Fail(errorNama);

            if (string.IsNullOrEmpty(currentPassword))
                return OperationResult.Fail("Password saat ini wajib diisi untuk konfirmasi keamanan!");

            string cleanCurrent = currentUsername.Trim();
            bool ubahPassword = !string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(confirmNewPassword);

            if (ubahPassword)
            {
                if ((newPassword ?? "").Length < 6)
                    return OperationResult.Fail("Password baru minimal 6 karakter!");

                if (newPassword != confirmNewPassword)
                    return OperationResult.Fail("Konfirmasi password baru tidak cocok!");
            }

            try
            {
                Dictionary<string, object> row = DataStore.FindUser(cleanCurrent);
                string currentHash = HashPassword(currentPassword);

                if (row == null || !string.Equals(DataStore.ReadString(row, "password"), currentHash, StringComparison.OrdinalIgnoreCase))
                    return OperationResult.Fail("Password saat ini salah!");

                string savedCurrent = DataStore.ReadString(row, "username");
                if (string.IsNullOrEmpty(savedCurrent)) savedCurrent = cleanCurrent;

                string savedNama = DataStore.ReadString(row, "nama_lengkap");
                if (string.IsNullOrWhiteSpace(savedNama)) savedNama = savedCurrent;

                bool ubahUsername = !string.Equals(savedCurrent, cleanNewUsername, StringComparison.Ordinal);
                bool ubahNama = !string.Equals(savedNama, cleanNewNama, StringComparison.Ordinal);

                if (!ubahUsername && !ubahNama && !ubahPassword)
                    return OperationResult.Fail("Ubah username, nama pengguna, atau isi password baru untuk memperbarui profil!");

                if (ubahUsername && DataStore.FindUser(cleanNewUsername) != null)
                    return OperationResult.Fail("Username baru sudah digunakan oleh akun lain!");

                string finalPasswordHash = ubahPassword ? HashPassword(newPassword) : currentHash;

                DataStore.Update("app_users?username=eq." + Uri.EscapeDataString(savedCurrent), new
                {
                    username = cleanNewUsername,
                    nama_lengkap = cleanNewNama,
                    password = finalPasswordHash
                });

                if (ubahNama || ubahUsername)
                {
                    try
                    {
                        DataStore.RenameHistoryUser(savedNama, cleanNewNama);
                        if (!string.Equals(savedCurrent, savedNama, StringComparison.Ordinal))
                        {
                            DataStore.RenameHistoryUser(savedCurrent, cleanNewNama);
                        }
                    }
                    catch { }
                }

                if (UserSession.Current != null)
                {
                    UserSession.Current.Username = cleanNewUsername;
                    UserSession.Current.NamaLengkap = cleanNewNama;
                }

                return OperationResult.Ok("Profil akun berhasil diperbarui!");
            }
            catch
            {
                return OperationResult.Fail("Gagal memperbarui profil di server. Periksa koneksi internet Anda.");
            }
        }
    }
}
