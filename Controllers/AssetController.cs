using System;
using System.Collections.Generic;
using System.Linq;
using ITAssetManagement.Models;

namespace ITAssetManagement.Controllers
{
    // Controller untuk mengelola data master aset IT di Supabase dan cache memori
    public class AssetController
    {
        private readonly RiwayatController _riwayat = new RiwayatController();

        // Mengambil seluruh daftar aset atau menyaringnya sesuai kata kunci pencarian
        public List<AssetItem> GetAll(string keyword = "")
        {
            keyword = (keyword ?? "").Trim().ToLower();
            return DataStore.GetAssets()
                .Where(a => (a.KodeAset ?? "").ToLower().Contains(keyword)
                         || (a.NamaAset ?? "").ToLower().Contains(keyword)
                         || (a.Kategori ?? "").ToLower().Contains(keyword)
                         || (a.Kondisi ?? "").ToLower().Contains(keyword)
                         || (a.Status ?? "").ToLower().Contains(keyword)
                         || (keyword == "dipinjam" && a.IsBorrowed))
                .ToList();
        }

        // Mengambil daftar aset yang siap dipinjamkan (Tersedia & Baik)
        public List<AssetItem> GetReadyToLend() => DataStore.GetAssets().Where(a => a.IsReadyToLend).ToList();

        // Membuat kode aset urut berikutnya secara otomatis (contoh: AST-007)
        public string NextCode() => DataStore.NextCode("AST-", DataStore.GetAssets().Select(a => a.KodeAset));

        // Menghitung rincian stok gudang (tersedia, perbaikan, rusak) dan memvalidasi batas unit terhadap stok dipinjam
        private static OperationResult ResolveStokKondisi(
            AssetItem existing, int jumlah, string kondisi,
            int? jumlahPerbaikan, int? jumlahRusak,
            out int tersedia, out int perbaikan, out int rusak,
            out string teksKondisi, out string status)
        {
            tersedia = 0;
            perbaikan = 0;
            rusak = 0;
            teksKondisi = AssetCondition.Baik;
            status = AssetStatus.Tersedia;

            int sedangDipinjam = existing != null ? existing.Dipinjam : 0;
            int diGudang = Math.Max(0, jumlah - sedangDipinjam);

            if (jumlahPerbaikan.HasValue || jumlahRusak.HasValue)
            {
                perbaikan = jumlahPerbaikan ?? 0;
                rusak = jumlahRusak ?? 0;
                if (perbaikan < 0 || rusak < 0)
                {
                    return OperationResult.Fail("Jumlah unit perbaikan dan rusak tidak boleh bernilai negatif!");
                }
                if (perbaikan + rusak > diGudang)
                {
                    return OperationResult.Fail($"Total unit perbaikan dan rusak tidak boleh melebihi {diGudang} unit di gudang!");
                }
            }
            else if (kondisi == AssetCondition.Perbaikan)
            {
                perbaikan = diGudang;
            }
            else if (kondisi == AssetCondition.Rusak)
            {
                rusak = diGudang;
            }
            else if (kondisi != AssetCondition.Baik && existing != null)
            {
                // Pertahankan rincian stok lama bila caller tidak mengirim angka unit perbaikan/rusak
                rusak = Math.Min(existing.Rusak, diGudang);
                perbaikan = Math.Min(existing.Perbaikan, diGudang - rusak);
            }

            tersedia = Math.Max(0, diGudang - perbaikan - rusak);
            teksKondisi = AssetItem.FormatKondisi(jumlah, perbaikan, rusak);
            status = (tersedia == 0 && sedangDipinjam > 0) ? AssetStatus.Dipinjam : AssetStatus.Tersedia;
            return null;
        }

        // Menambahkan aset baru beserta rincian jumlah unit dan kondisinya ke Supabase dan cache memori
        public OperationResult Add(string nama, string kategori, string kondisi, string user, int jumlah = 1, int? jumlahPerbaikan = null, int? jumlahRusak = null)
        {
            if (string.IsNullOrWhiteSpace(nama)) return OperationResult.Fail("Nama Aset wajib diisi!");
            if (jumlah < 1) return OperationResult.Fail("Jumlah unit aset minimal 1!");

            kondisi = string.IsNullOrWhiteSpace(kondisi) ? AssetCondition.Baik : kondisi.Trim();
            OperationResult validasi = ResolveStokKondisi(null, jumlah, kondisi, jumlahPerbaikan, jumlahRusak,
                out int tersedia, out int perbaikan, out int rusak, out string teksKondisi, out string status);
            if (validasi != null) return validasi;

            try
            {
                string kode = NextCode();
                nama = nama.Trim();
                kategori = string.IsNullOrWhiteSpace(kategori) ? "Lainnya" : kategori.Trim();
                user = string.IsNullOrWhiteSpace(user) ? "admin" : user.Trim();

                DataStore.Insert("assets", new
                {
                    kode_aset = kode,
                    nama_aset = nama,
                    kategori = kategori,
                    jumlah = jumlah,
                    tersedia = tersedia,
                    perbaikan = perbaikan,
                    rusak = rusak,
                    kondisi = teksKondisi,
                    status = status
                });

                DataStore.GetAssets().Add(new AssetItem
                {
                    KodeAset = kode,
                    NamaAset = nama,
                    Kategori = kategori,
                    Jumlah = jumlah,
                    Tersedia = tersedia,
                    Perbaikan = perbaikan,
                    Rusak = rusak,
                    Kondisi = teksKondisi,
                    Status = status
                });

                _riwayat.Add(RiwayatAction.TambahAset, kode, nama, "-", $"Kategori: {kategori}, Kondisi: {teksKondisi}", user, jumlah);
                return OperationResult.Ok("Data aset berhasil ditambahkan!");
            }
            catch
            {
                return OperationResult.Fail("Gagal menyimpan data aset ke server. Periksa koneksi internet Anda.");
            }
        }

        // Memperbarui data aset beserta rincian jumlah stok dan kondisinya di Supabase dan cache memori
        public OperationResult Update(AssetItem asset, string nama, string kategori, string kondisi, int jumlah = 1, int? jumlahPerbaikan = null, int? jumlahRusak = null)
        {
            if (asset == null) return OperationResult.Fail("Klik baris aset pada tabel terlebih dahulu sebelum mengubah!");
            if (string.IsNullOrWhiteSpace(nama)) return OperationResult.Fail("Nama Aset tidak boleh kosong!");
            if (jumlah < 1) return OperationResult.Fail("Jumlah unit aset minimal 1!");

            int sedangDipinjam = asset.Dipinjam;
            if (jumlah < sedangDipinjam)
            {
                return OperationResult.Fail($"Jumlah unit tidak boleh kurang dari {sedangDipinjam} unit yang sedang dipinjam!");
            }

            kondisi = string.IsNullOrWhiteSpace(kondisi) ? asset.Kondisi : kondisi.Trim();
            OperationResult validasi = ResolveStokKondisi(asset, jumlah, kondisi, jumlahPerbaikan, jumlahRusak,
                out int tersediaBaru, out int perbaikanBaru, out int rusakBaru, out string kondisiBaru, out string statusBaru);
            if (validasi != null) return validasi;

            try
            {
                nama = nama.Trim();
                kategori = string.IsNullOrWhiteSpace(kategori) ? asset.Kategori : kategori.Trim();
                string filterKode = "kode_aset=eq." + Uri.EscapeDataString(asset.KodeAset);

                DataStore.Update("assets?" + filterKode, new
                {
                    nama_aset = nama,
                    kategori = kategori,
                    jumlah = jumlah,
                    tersedia = tersediaBaru,
                    perbaikan = perbaikanBaru,
                    rusak = rusakBaru,
                    kondisi = kondisiBaru,
                    status = statusBaru
                });

                var alokasiTerkait = DataStore.GetAlokasi().Where(a => a.KodeAset == asset.KodeAset).ToList();
                if (asset.NamaAset != nama && alokasiTerkait.Count > 0)
                {
                    DataStore.Update("alokasi?" + filterKode, new { nama_aset = nama });
                    foreach (var alokasi in alokasiTerkait) alokasi.NamaAset = nama;
                }

                asset.NamaAset = nama;
                asset.Kategori = kategori;
                asset.Jumlah = jumlah;
                asset.Tersedia = tersediaBaru;
                asset.Perbaikan = perbaikanBaru;
                asset.Rusak = rusakBaru;
                asset.Kondisi = kondisiBaru;
                asset.Status = statusBaru;
                return OperationResult.Ok("Data aset berhasil diperbarui!");
            }
            catch
            {
                return OperationResult.Fail("Gagal memperbarui data aset di server. Periksa koneksi internet Anda.");
            }
        }

        // Menghapus data aset jika tidak sedang dipinjam dan mencatat penghapusan ke log Riwayat
        public OperationResult Delete(AssetItem asset, string user = null)
        {
            if (asset == null) return OperationResult.Fail("Klik baris aset pada tabel terlebih dahulu sebelum menghapus!");
            if (asset.IsBorrowed || DataStore.GetAlokasi().Any(a => a.KodeAset == asset.KodeAset && a.IsActive))
            {
                return OperationResult.Fail("Aset sedang dipinjam dan tidak dapat dihapus!");
            }

            try
            {
                DataStore.Delete("assets?kode_aset=eq." + Uri.EscapeDataString(asset.KodeAset));
                DataStore.GetAssets().Remove(asset);
                DataStore.GetAlokasi().RemoveAll(a => a.KodeAset == asset.KodeAset);

                _riwayat.Add(
                    RiwayatAction.HapusAset,
                    asset.KodeAset,
                    asset.NamaAset,
                    "-",
                    $"Kategori: {asset.Kategori}, Kondisi terakhir: {asset.Kondisi}",
                    user,
                    asset.Jumlah);
                return OperationResult.Ok("Data aset berhasil dihapus!");
            }
            catch
            {
                return OperationResult.Fail("Gagal menghapus data aset di server. Periksa koneksi internet Anda.");
            }
        }
    }
}
