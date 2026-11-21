using System;
using System.Collections.Generic;
using System.Linq;
using ITAssetManagement.Models;

namespace ITAssetManagement.Controllers
{
    // Controller untuk menangani peminjaman (alokasi multi-unit) dan pengembalian aset (Admin & Staff)
    public class AlokasiController
    {
        private readonly RiwayatController _riwayat = new RiwayatController();

        // Mengambil daftar transaksi peminjaman aset yang masih berstatus Aktif
        public List<AlokasiItem> GetActive() => DataStore.GetAlokasi().Where(a => a.IsActive).ToList();

        // Mengirim rincian stok aset (tersedia/perbaikan/rusak/kondisi/status) ke Supabase
        private static void UpdateStok(string kodeAset, int tersedia, int perbaikan, int rusak, string kondisi, string status)
        {
            DataStore.Update("assets?kode_aset=eq." + Uri.EscapeDataString(kodeAset), new
            {
                tersedia = tersedia,
                perbaikan = perbaikan,
                rusak = rusak,
                kondisi = kondisi,
                status = status
            });
        }

        // Memproses peminjaman sejumlah unit aset kepada karyawan (tidak boleh melebihi stok tersedia)
        public OperationResult Allocate(AssetItem asset, KaryawanItem karyawan, DateTime tanggal, string keterangan, string user, int jumlahPinjam = 1)
        {
            if (asset == null) return OperationResult.Fail("Tidak ada aset tersedia yang dipilih!");
            if (!asset.IsReadyToLend) return OperationResult.Fail("Stok aset ini sudah habis dipinjam atau sedang dalam perbaikan!");
            if (karyawan == null) return OperationResult.Fail("Pilih karyawan penerima terlebih dahulu!");
            if (jumlahPinjam < 1) return OperationResult.Fail("Jumlah unit yang dipinjam minimal 1!");
            if (jumlahPinjam > asset.Tersedia)
            {
                return OperationResult.Fail($"Jumlah pinjam ({jumlahPinjam} unit) tidak boleh melebihi stok tersedia ({asset.Tersedia} unit)!");
            }

            try
            {
                keterangan = string.IsNullOrWhiteSpace(keterangan) ? "Peminjaman operasional kerja" : keterangan.Trim();
                user = string.IsNullOrWhiteSpace(user) ? "admin" : user.Trim();
                string idAlokasi = DataStore.NextCode("ALK-", DataStore.GetAlokasi().Select(a => a.IdAlokasi));

                int sisaTersedia = Math.Max(0, asset.Tersedia - jumlahPinjam);
                string statusBaru = sisaTersedia > 0 ? AssetStatus.Tersedia : AssetStatus.Dipinjam;

                UpdateStok(asset.KodeAset, sisaTersedia, asset.Perbaikan, asset.Rusak, asset.Kondisi, statusBaru);

                try
                {
                    DataStore.Insert("alokasi", new
                    {
                        id_alokasi = idAlokasi,
                        kode_aset = asset.KodeAset,
                        nama_aset = asset.NamaAset,
                        jumlah = jumlahPinjam,
                        nik_karyawan = karyawan.Nik,
                        nama_karyawan = karyawan.Nama,
                        divisi = karyawan.Divisi,
                        tanggal_pinjam = tanggal.ToString("yyyy-MM-dd"),
                        keterangan = keterangan,
                        status_alokasi = AlokasiStatus.Aktif
                    });
                }
                catch
                {
                    // Transaksi gagal disimpan: kembalikan stok agar unit tidak "hilang" dari gudang
                    UpdateStok(asset.KodeAset, asset.Tersedia, asset.Perbaikan, asset.Rusak, asset.Kondisi, asset.Status);
                    throw;
                }

                asset.Tersedia = sisaTersedia;
                asset.Status = statusBaru;
                DataStore.GetAlokasi().Add(new AlokasiItem
                {
                    IdAlokasi = idAlokasi,
                    KodeAset = asset.KodeAset,
                    NamaAset = asset.NamaAset,
                    Jumlah = jumlahPinjam,
                    NikKaryawan = karyawan.Nik,
                    NamaKaryawan = karyawan.Nama,
                    Divisi = karyawan.Divisi,
                    TanggalPinjam = tanggal.Date,
                    Keterangan = keterangan,
                    StatusAlokasi = AlokasiStatus.Aktif
                });

                _riwayat.Add(RiwayatAction.Peminjaman, asset.KodeAset, asset.NamaAset, karyawan.Nama, keterangan, user, jumlahPinjam);
                return OperationResult.Ok($"{jumlahPinjam} unit {asset.NamaAset} berhasil dialokasikan kepada {karyawan.Nama}! (Sisa tersedia: {sisaTersedia} unit)");
            }
            catch
            {
                return OperationResult.Fail("Gagal memproses alokasi aset ke server. Periksa koneksi internet Anda.");
            }
        }

        // Memproses pengembalian satu transaksi alokasi. Sebanyak jumlahKondisi unit dicatat berkondisi kondisiKembali,
        // unit sisanya kembali Baik (null = seluruh unit transaksi berkondisi kondisiKembali)
        public OperationResult Return(AlokasiItem loan, string kondisiKembali, string user, int? jumlahKondisi = null)
        {
            if (loan == null) return OperationResult.Fail("Pilih baris aset yang ingin dikembalikan!");
            if (!loan.IsActive) return OperationResult.Fail("Aset pada transaksi ini sudah dikembalikan!");

            int jumlahKembali = Math.Max(1, loan.Jumlah);
            kondisiKembali = string.IsNullOrWhiteSpace(kondisiKembali) ? AssetCondition.Baik : kondisiKembali.Trim();

            int unitBermasalah = kondisiKembali == AssetCondition.Baik ? 0 : jumlahKondisi ?? jumlahKembali;
            if (kondisiKembali != AssetCondition.Baik && (unitBermasalah < 1 || unitBermasalah > jumlahKembali))
            {
                return OperationResult.Fail($"Jumlah unit berkondisi {kondisiKembali} harus antara 1 dan {jumlahKembali}!");
            }

            try
            {
                user = string.IsNullOrWhiteSpace(user) ? "staff" : user.Trim();

                var asset = DataStore.GetAssets().FirstOrDefault(a => a.KodeAset == loan.KodeAset);
                int totalJumlah = asset != null ? Math.Max(1, asset.Jumlah) : jumlahKembali;
                int sisaDipinjam = Math.Max(0, (asset != null ? asset.Dipinjam : jumlahKembali) - jumlahKembali);
                int perbaikanBaru = asset != null ? asset.Perbaikan : 0;
                int rusakBaru = asset != null ? asset.Rusak : 0;

                // Hanya unit yang dikembalikan yang ditambahkan ke stok Perbaikan/Rusak; stok lain tidak ikut berubah
                if (kondisiKembali == AssetCondition.Perbaikan) perbaikanBaru += unitBermasalah;
                else if (kondisiKembali == AssetCondition.Rusak) rusakBaru += unitBermasalah;

                // Unit Baik = sisa setelah dikurangi unit yang masih dipinjam, Perbaikan, dan Rusak
                int tersediaBaru = Math.Max(0, totalJumlah - sisaDipinjam - perbaikanBaru - rusakBaru);
                string kondisiBaru = AssetItem.FormatKondisi(totalJumlah, perbaikanBaru, rusakBaru);
                string statusBaru = (tersediaBaru == 0 && sisaDipinjam > 0) ? AssetStatus.Dipinjam : AssetStatus.Tersedia;

                UpdateStok(loan.KodeAset, tersediaBaru, perbaikanBaru, rusakBaru, kondisiBaru, statusBaru);

                try
                {
                    DataStore.Update("alokasi?id_alokasi=eq." + Uri.EscapeDataString(loan.IdAlokasi), new { status_alokasi = AlokasiStatus.Dikembalikan });
                }
                catch
                {
                    // Transaksi gagal ditutup: batalkan perubahan stok agar tidak terhitung ganda saat dicoba ulang
                    if (asset != null) UpdateStok(asset.KodeAset, asset.Tersedia, asset.Perbaikan, asset.Rusak, asset.Kondisi, asset.Status);
                    throw;
                }

                loan.StatusAlokasi = AlokasiStatus.Dikembalikan;
                if (asset != null)
                {
                    asset.Tersedia = tersediaBaru;
                    asset.Perbaikan = perbaikanBaru;
                    asset.Rusak = rusakBaru;
                    asset.Kondisi = kondisiBaru;
                    asset.Status = statusBaru;
                }

                string catatan = unitBermasalah > 0 && unitBermasalah < jumlahKembali
                    ? $"Kondisi kembali: {kondisiKembali} ({unitBermasalah} unit), sisanya Baik"
                    : $"Kondisi kembali: {kondisiKembali}";
                _riwayat.Add(RiwayatAction.Pengembalian, loan.KodeAset, loan.NamaAset, loan.NamaKaryawan, catatan, user, jumlahKembali);
                return OperationResult.Ok("Aset berhasil dikembalikan!");
            }
            catch
            {
                return OperationResult.Fail("Gagal memproses pengembalian aset ke server. Periksa koneksi internet Anda.");
            }
        }
    }
}
