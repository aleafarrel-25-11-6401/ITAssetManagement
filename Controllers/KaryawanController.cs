using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ITAssetManagement.Models;

namespace ITAssetManagement.Controllers
{
    // Controller untuk mengelola data master karyawan di Supabase dan cache memori
    public class KaryawanController
    {
        // Mengambil seluruh daftar karyawan atau menyaringnya sesuai kata kunci pencarian
        public List<KaryawanItem> GetAll(string keyword = "")
        {
            keyword = (keyword ?? "").Trim().ToLower();
            return DataStore.GetKaryawan()
                .Where(k => (k.Nik ?? "").ToLower().Contains(keyword)
                         || (k.Nama ?? "").ToLower().Contains(keyword)
                         || (k.Divisi ?? "").ToLower().Contains(keyword)
                         || (k.Jabatan ?? "").ToLower().Contains(keyword)
                         || (k.Kontak ?? "").ToLower().Contains(keyword))
                .ToList();
        }

        // Membuat ID karyawan (NIK) urut berikutnya secara otomatis (contoh: EMP-005)
        public string NextNik() => DataStore.NextCode("EMP-", DataStore.GetKaryawan().Select(k => k.Nik));

        // Menambahkan karyawan baru ke Supabase dan cache memori
        public OperationResult Add(string nama, string divisi, string jabatan, string kontak)
        {
            if (string.IsNullOrWhiteSpace(nama)) return OperationResult.Fail("Nama Karyawan wajib diisi!");
            if (KontakDipakaiKaryawanLain(IsiDefault(kontak, "-"), null)) return OperationResult.Fail(PesanKontakSama);

            try
            {
                var item = new KaryawanItem
                {
                    Nik = NextNik(),
                    Nama = nama.Trim(),
                    Divisi = IsiDefault(divisi, "Umum"),
                    Jabatan = IsiDefault(jabatan, "Staff"),
                    Kontak = IsiDefault(kontak, "-")
                };

                DataStore.Insert("karyawan", new
                {
                    nik = item.Nik,
                    nama = item.Nama,
                    divisi = item.Divisi,
                    jabatan = item.Jabatan,
                    kontak = item.Kontak
                });

                DataStore.GetKaryawan().Add(item);
                return OperationResult.Ok("Data karyawan berhasil ditambahkan!");
            }
            catch
            {
                return OperationResult.Fail("Gagal menyimpan data karyawan ke server. Periksa koneksi internet Anda.");
            }
        }

        // Memperbarui data karyawan di Supabase dan menyinkronkannya di cache memori
        public OperationResult Update(KaryawanItem karyawan, string nama, string divisi, string jabatan, string kontak)
        {
            if (karyawan == null) return OperationResult.Fail("Klik baris karyawan pada tabel terlebih dahulu sebelum mengubah!");
            if (string.IsNullOrWhiteSpace(nama)) return OperationResult.Fail("Nama Karyawan tidak boleh kosong!");

            // Hanya diperiksa bila kontak benar-benar diubah, agar data lama tetap bisa diedit (sama seperti trigger database)
            string kontakBaru = IsiDefault(kontak, "-");
            if (NormalisasiKontak(kontakBaru) != NormalisasiKontak(karyawan.Kontak)
                && KontakDipakaiKaryawanLain(kontakBaru, karyawan.Nik))
                return OperationResult.Fail(PesanKontakSama);

            try
            {
                nama = nama.Trim();
                divisi = IsiDefault(divisi, karyawan.Divisi);
                jabatan = IsiDefault(jabatan, "Staff");
                kontak = IsiDefault(kontak, "-");

                DataStore.Update("karyawan?nik=eq." + Uri.EscapeDataString(karyawan.Nik), new
                {
                    nama = nama,
                    divisi = divisi,
                    jabatan = jabatan,
                    kontak = kontak
                });

                var alokasiTerkait = DataStore.GetAlokasi().Where(a => a.NikKaryawan == karyawan.Nik).ToList();
                if ((karyawan.Nama != nama || karyawan.Divisi != divisi) && alokasiTerkait.Count > 0)
                {
                    DataStore.Update("alokasi?nik_karyawan=eq." + Uri.EscapeDataString(karyawan.Nik), new
                    {
                        nama_karyawan = nama,
                        divisi = divisi
                    });

                    foreach (var alokasi in alokasiTerkait)
                    {
                        alokasi.NamaKaryawan = nama;
                        alokasi.Divisi = divisi;
                    }
                }

                karyawan.Nama = nama;
                karyawan.Divisi = divisi;
                karyawan.Jabatan = jabatan;
                karyawan.Kontak = kontak;
                return OperationResult.Ok("Data karyawan berhasil diperbarui!");
            }
            catch
            {
                return OperationResult.Fail("Gagal memperbarui data karyawan di server. Periksa koneksi internet Anda.");
            }
        }

        // Menghapus data karyawan jika tidak sedang meminjam aset aktif
        public OperationResult Delete(KaryawanItem karyawan)
        {
            if (karyawan == null) return OperationResult.Fail("Klik baris karyawan pada tabel terlebih dahulu sebelum menghapus!");

            try
            {
                if (DataStore.GetAlokasi().Any(a => a.NikKaryawan == karyawan.Nik && a.IsActive))
                    return OperationResult.Fail("Karyawan masih memiliki pinjaman aset aktif!");

                DataStore.Delete("karyawan?nik=eq." + Uri.EscapeDataString(karyawan.Nik));
                DataStore.GetKaryawan().Remove(karyawan);
                DataStore.GetAlokasi().RemoveAll(a => a.NikKaryawan == karyawan.Nik);
                return OperationResult.Ok("Data karyawan berhasil dihapus!");
            }
            catch
            {
                return OperationResult.Fail("Gagal menghapus data karyawan di server. Periksa koneksi internet Anda.");
            }
        }

        private const string PesanKontakSama = "Nomor telepon/email kontak sudah digunakan karyawan lain!";

        // Menyeragamkan kontak (tanpa spasi, titik, kurung, strip, dan huruf kecil semua); harus sama dengan trigger enforce_karyawan_kontak_unique di database
        public static string NormalisasiKontak(string kontak)
            => Regex.Replace(kontak ?? "", @"[\s().\-]", "").ToLowerInvariant();

        // Kontak kosong ("-") dikecualikan karena boleh sama pada banyak karyawan
        private static bool KontakDipakaiKaryawanLain(string kontak, string nikSendiri)
        {
            string normal = NormalisasiKontak(kontak);
            return normal.Length > 0
                && DataStore.GetKaryawan().Any(k => k.Nik != nikSendiri && NormalisasiKontak(k.Kontak) == normal);
        }

        private static string IsiDefault(string teks, string nilaiDefault)
            => string.IsNullOrWhiteSpace(teks) ? nilaiDefault : teks.Trim();
    }
}
