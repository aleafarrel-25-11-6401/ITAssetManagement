using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;
using ITAssetManagement.Views;

namespace ITAssetManagement.Models
{
    // Penghubung REST API Supabase sekaligus penyimpan cache data di memori
    public static class DataStore
    {
        private static string _baseUrl;
        private static HttpClient _http;
        private static readonly JavaScriptSerializer _json = new JavaScriptSerializer();
        private static DateTime _lastErrorTime = DateTime.MinValue;

        // Cache memori dan masa berlakunya (60 detik) agar perpindahan halaman tetap ringan namun tidak basi
        private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);
        private static DateTime _cacheLoadedAt = DateTime.MinValue;
        private static List<AssetItem> _assetsCache;
        private static List<KaryawanItem> _karyawanCache;
        private static List<AlokasiItem> _alokasiCache;
        private static List<RiwayatItem> _riwayatCache;

        // Menyiapkan HttpClient dan membaca kredensial dari App.config satu kali
        private static void InitClient()
        {
            if (_http != null) return;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _baseUrl = (ConfigurationManager.AppSettings["SupabaseUrl"] ?? "").Trim().TrimEnd('/');
            string apiKey = (ConfigurationManager.AppSettings["SupabaseKey"] ?? "").Trim();

            if (string.IsNullOrEmpty(_baseUrl) || string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("Konfigurasi Supabase belum lengkap.");
            }

            var client = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
            client.DefaultRequestHeaders.Add("apikey", apiKey);

            if (apiKey.StartsWith("eyJ"))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
            }

            _http = client;
        }

        // Mengosongkan seluruh cache memori beserta penanda waktunya
        public static void ClearCache()
        {
            _assetsCache = null;
            _karyawanCache = null;
            _alokasiCache = null;
            _riwayatCache = null;
            _cacheLoadedAt = DateTime.MinValue;
        }

        // Menghapus cache hanya jika masa berlakunya sudah habis agar perpindahan halaman tetap cepat
        public static void ClearExpiredCache()
        {
            if (_cacheLoadedAt == DateTime.MinValue) return;
            if (DateTime.Now - _cacheLoadedAt < CacheDuration) return;
            ClearCache();
        }

        // Menyimpan hasil fetch ke cache dan mencatat waktu siklus awal bila belum tercatat
        private static void SetCache<T>(ref List<T> cache, List<T> data)
        {
            cache = data;
            if (_cacheLoadedAt == DateTime.MinValue) _cacheLoadedAt = DateTime.Now;
        }

        // Mengambil daftar aset (dari Supabase pada pemanggilan pertama, selanjutnya dari cache)
        public static List<AssetItem> GetAssets()
        {
            if (_assetsCache == null)
            {
                var rows = GetRows("assets?select=kode_aset,nama_aset,kategori,jumlah,tersedia,perbaikan,rusak,kondisi,status&order=kode_aset.asc", out bool sukses);
                var list = rows.Select(row =>
                {
                    int jumlah = Math.Max(1, ReadInt(row, "jumlah", 1));
                    int perbaikan = Math.Max(0, ReadInt(row, "perbaikan", 0));
                    int rusak = Math.Max(0, ReadInt(row, "rusak", 0));
                    int maksTersedia = Math.Max(0, jumlah - perbaikan - rusak);
                    int tersedia = Math.Max(0, Math.Min(maksTersedia, ReadInt(row, "tersedia", maksTersedia)));
                    string kondisiDb = ReadString(row, "kondisi");
                    string kondisi = string.IsNullOrWhiteSpace(kondisiDb)
                        ? AssetItem.FormatKondisi(jumlah, perbaikan, rusak)
                        : kondisiDb;
                    return new AssetItem
                    {
                        KodeAset = ReadString(row, "kode_aset"),
                        NamaAset = ReadString(row, "nama_aset"),
                        Kategori = ReadString(row, "kategori"),
                        Jumlah = jumlah,
                        Tersedia = tersedia,
                        Perbaikan = perbaikan,
                        Rusak = rusak,
                        Kondisi = kondisi,
                        Status = ReadString(row, "status")
                    };
                }).ToList();

                if (!sukses) return list;
                SetCache(ref _assetsCache, list);
            }
            return _assetsCache;
        }

        // Mengambil daftar karyawan (dari Supabase pada pemanggilan pertama, selanjutnya dari cache)
        public static List<KaryawanItem> GetKaryawan()
        {
            if (_karyawanCache == null)
            {
                var rows = GetRows("karyawan?select=nik,nama,divisi,jabatan,kontak&order=nik.asc", out bool sukses);
                var list = rows.Select(row => new KaryawanItem
                {
                    Nik = ReadString(row, "nik"),
                    Nama = ReadString(row, "nama"),
                    Divisi = ReadString(row, "divisi"),
                    Jabatan = ReadString(row, "jabatan"),
                    Kontak = ReadString(row, "kontak")
                }).ToList();

                if (!sukses) return list;
                SetCache(ref _karyawanCache, list);
            }
            return _karyawanCache;
        }

        // Mengambil daftar alokasi peminjaman (dari Supabase pada pemanggilan pertama, selanjutnya dari cache)
        public static List<AlokasiItem> GetAlokasi()
        {
            if (_alokasiCache == null)
            {
                var rows = GetRows("alokasi?select=id_alokasi,kode_aset,nama_aset,jumlah,nik_karyawan,nama_karyawan,divisi,tanggal_pinjam,keterangan,status_alokasi&order=id_alokasi.asc", out bool sukses);
                var list = rows.Select(row => new AlokasiItem
                {
                    IdAlokasi = ReadString(row, "id_alokasi"),
                    KodeAset = ReadString(row, "kode_aset"),
                    NamaAset = ReadString(row, "nama_aset"),
                    Jumlah = Math.Max(1, ReadInt(row, "jumlah", 1)),
                    NikKaryawan = ReadString(row, "nik_karyawan"),
                    NamaKaryawan = ReadString(row, "nama_karyawan"),
                    Divisi = ReadString(row, "divisi"),
                    TanggalPinjam = ReadDate(row, "tanggal_pinjam"),
                    Keterangan = ReadString(row, "keterangan"),
                    StatusAlokasi = ReadString(row, "status_alokasi")
                }).ToList();

                if (!sukses) return list;
                SetCache(ref _alokasiCache, list);
            }
            return _alokasiCache;
        }

        // Mengambil daftar riwayat aktivitas (dari Supabase pada pemanggilan pertama, selanjutnya dari cache)
        public static List<RiwayatItem> GetRiwayat()
        {
            if (_riwayatCache == null)
            {
                var rows = GetRows("riwayat?select=waktu,jenis_aksi,kode_aset,nama_aset,jumlah,nama_karyawan,catatan,oleh_user&order=waktu.desc", out bool sukses);
                var list = rows.Select(row => new RiwayatItem
                {
                    Waktu = ReadDate(row, "waktu"),
                    JenisAksi = ReadString(row, "jenis_aksi"),
                    KodeAset = ReadString(row, "kode_aset"),
                    NamaAset = ReadString(row, "nama_aset"),
                    Jumlah = Math.Max(1, ReadInt(row, "jumlah", 1)),
                    NamaKaryawan = ReadString(row, "nama_karyawan"),
                    Catatan = ReadString(row, "catatan"),
                    OlehUser = ReadString(row, "oleh_user")
                }).ToList();

                if (!sukses) return list;
                SetCache(ref _riwayatCache, list);
            }
            return _riwayatCache;
        }

        // Mencari satu akun pengguna pada tabel 'app_users' berdasarkan username secara case-sensitive
        public static Dictionary<string, object> FindUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;
            string cleanUsername = username.Trim();

            string query = $"app_users?select=username,nama_lengkap,password,role&username=eq.{Uri.EscapeDataString(cleanUsername)}&limit=1";
            var rows = _json.Deserialize<List<Dictionary<string, object>>>(Send("GET", query));
            if (rows == null || rows.Count == 0) return null;

            return string.Equals(ReadString(rows[0], "username"), cleanUsername, StringComparison.Ordinal)
                ? rows[0]
                : null;
        }

        // Memperbarui nama pelaku pada catatan riwayat saat pengguna mengganti nama pengguna atau username
        public static void RenameHistoryUser(string oldName, string newName)
        {
            if (string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName)) return;
            if (string.Equals(oldName, newName, StringComparison.Ordinal)) return;

            Send("PATCH", "riwayat?oleh_user=eq." + Uri.EscapeDataString(oldName), new { oleh_user = newName });
            if (_riwayatCache != null)
            {
                foreach (var item in _riwayatCache.Where(r => string.Equals(r.OlehUser, oldName, StringComparison.OrdinalIgnoreCase)))
                {
                    item.OlehUser = newName;
                }
            }
        }

        public static void Insert(string table, object data) => Send("POST", table, data);

        public static void Update(string queryEndpoint, object data) => Send("PATCH", queryEndpoint, data);

        public static void Delete(string queryEndpoint) => Send("DELETE", queryEndpoint);

        // Membuat kode urut otomatis berikutnya yang belum terpakai (contoh: AST-007, EMP-005, ALK-003)
        public static string NextCode(string prefix, IEnumerable<string> daftarKode)
        {
            var terpakai = new HashSet<string>(daftarKode ?? Enumerable.Empty<string>(), StringComparer.OrdinalIgnoreCase);
            int nomor = 1;
            while (terpakai.Contains($"{prefix}{nomor:D3}")) nomor++;
            return $"{prefix}{nomor:D3}";
        }

        // Mengambil baris data dari Supabase secara aman tanpa membuat aplikasi crash saat offline
        private static List<Dictionary<string, object>> GetRows(string endpoint, out bool sukses)
        {
            try
            {
                var list = _json.Deserialize<List<Dictionary<string, object>>>(Send("GET", endpoint))
                    ?? new List<Dictionary<string, object>>();
                sukses = true;
                return list;
            }
            catch
            {
                sukses = false;
                if ((DateTime.Now - _lastErrorTime).TotalSeconds > 3)
                {
                    _lastErrorTime = DateTime.Now;
                    Dialogs.Warn("Gagal memuat data dari server. Pastikan koneksi internet Anda aktif.", "Informasi Koneksi");
                }
                return new List<Dictionary<string, object>>();
            }
        }

        // Mengirim request HTTP ke Supabase disertai indikator status loading
        private static string Send(string method, string endpoint, object body = null)
        {
            UIHelper.ShowLoading(method == "GET" ? "Memuat data..." : "Menyimpan data...");

            try
            {
                InitClient();
                using (var request = new HttpRequestMessage(new HttpMethod(method), $"{_baseUrl}/rest/v1/{endpoint}"))
                {
                    if (body != null)
                    {
                        request.Headers.Add("Prefer", "return=minimal");
                        request.Content = new StringContent(_json.Serialize(body), Encoding.UTF8, "application/json");
                    }

                    using (var response = _http.SendAsync(request).Result)
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new InvalidOperationException("Permintaan ditolak oleh server.");
                        }
                        return content;
                    }
                }
            }
            finally
            {
                UIHelper.HideLoading();
            }
        }

        // Membaca nilai string, angka bulat, atau tanggal dari hasil query JSON dengan aman
        public static string ReadString(Dictionary<string, object> row, string column)
            => row != null && row.TryGetValue(column, out object value) && value != null ? value.ToString() : "";

        public static int ReadInt(Dictionary<string, object> row, string column, int defaultValue = 0)
            => int.TryParse(ReadString(row, column), out int angka) ? angka : defaultValue;

        public static DateTime ReadDate(Dictionary<string, object> row, string column)
            => DateTime.TryParse(ReadString(row, column), out DateTime tanggal) ? tanggal : DateTime.Now;
    }
}
