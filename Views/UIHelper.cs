using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ITAssetManagement.Models;

namespace ITAssetManagement.Views
{
    // Class pembantu tampilan untuk mengatur DataGridView, pewarnaan baris, debounce pencarian, dan indikator loading
    public static class UIHelper
    {
        private static readonly Font _gridFont = new Font("Segoe UI", 9F, FontStyle.Regular);
        private static readonly Font _gridBoldFont = new Font("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font _emptyTitleFont = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        private static readonly Font _emptySubFont = new Font("Segoe UI", 9F, FontStyle.Regular);
        // Variabel komponen untuk label loading dan timer jeda ketikan pencarian
        private static Label _loadingBadge;
        private static Timer _debounceTimer;
        private static Action _debounceAction;

        // Menunda eksekusi pencarian selama 300ms sampai pengguna berhenti mengetik
        public static void Debounce(Action action, int delayMs = 300)
        {
            _debounceAction = action;

            if (_debounceTimer == null)
            {
                _debounceTimer = new Timer();
                _debounceTimer.Tick += DebounceTimer_Tick;
            }

            _debounceTimer.Stop();
            _debounceTimer.Interval = delayMs;
            _debounceTimer.Start();
        }

        // Membatalkan timer pencarian yang masih tertunda saat pengguna berpindah halaman
        public static void CancelDebounce()
        {
            _debounceAction = null;
            if (_debounceTimer != null)
            {
                _debounceTimer.Stop();
            }
        }

        // Menjalankan fungsi pencarian ketika waktu tunggu timer selesai
        private static void DebounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            try
            {
                _debounceAction?.Invoke();
            }
            catch
            {
                // Abaikan jika kontrol halaman sudah ditutup saat timer selesai
            }
        }

        // Menampilkan label status "Memproses..." dan kursor tunggu di bagian atas jendela aktif
        public static void ShowLoading(string message = "Memproses data...")
        {
            try
            {
                Application.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;

                Form form = Form.ActiveForm;
                if (form == null || form.IsDisposed) return;

                if (_loadingBadge == null || _loadingBadge.IsDisposed)
                {
                    _loadingBadge = new Label
                    {
                        AutoSize = false,
                        Size = new Size(230, 32),
                        BackColor = Color.FromArgb(72, 68, 211),
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        BorderStyle = BorderStyle.FixedSingle
                    };
                }

                if (_loadingBadge.Parent != form)
                {
                    form.Controls.Add(_loadingBadge);
                }

                _loadingBadge.Text = "⏳  " + message;
                _loadingBadge.Location = new Point((form.ClientSize.Width - _loadingBadge.Width) / 2, 12);
                _loadingBadge.Visible = true;
                _loadingBadge.BringToFront();
                _loadingBadge.Refresh();
                form.Update();
            }
            catch
            {
                // Abaikan jika jendela sedang proses ditutup
            }
        }

        // Menyembunyikan label loading dan mengembalikan kursor mouse normal
        public static void HideLoading()
        {
            try
            {
                if (_loadingBadge != null && !_loadingBadge.IsDisposed)
                {
                    _loadingBadge.Visible = false;
                    _loadingBadge.Parent?.Update();
                }
            }
            catch
            {
                // Abaikan jika form sudah ditutup
            }
            finally
            {
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
            }
        }

        // Mengisi tabel DataGridView serta merapikan judul kolom, wrap teks catatan/keterangan, tinggi baris otomatis, pewarnaan baris, dan empty state
        public static void BindGrid(DataGridView grid, object data, string emptyMessage = null)
        {
            if (grid == null || grid.IsDisposed) return;

            grid.DefaultCellStyle.Font = _gridFont;
            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grid.DefaultCellStyle.Padding = new Padding(4, 4, 4, 4);
            grid.RowTemplate.Height = 28;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grid.Tag = emptyMessage;

            if (grid.BindingContext == null)
            {
                grid.BindingContext = new BindingContext();
            }

            grid.DataBindingComplete -= Grid_DataBindingComplete;
            grid.DataBindingComplete += Grid_DataBindingComplete;
            grid.Paint -= Grid_Paint;
            grid.Paint += Grid_Paint;
            grid.Resize -= Grid_Resize;
            grid.Resize += Grid_Resize;

            grid.DataSource = null;
            grid.DataSource = data;

            ApplyColumnFormatting(grid);
            ColorizeRows(grid);
            if (grid.Rows.Count > 0)
            {
                grid.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            }
            ClearSelection(grid);
            grid.Invalidate();
        }

        // Menerapkan judul kolom, format tanggal, WrapMode teks, dan proporsi lebar kolom
        private static void ApplyColumnFormatting(DataGridView grid)
        {
            if (grid == null || grid.IsDisposed) return;

            foreach (DataGridViewColumn col in grid.Columns)
            {
                col.HeaderText = FormatHeader(col.Name);
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                if (col.ValueType == typeof(DateTime))
                {
                    col.DefaultCellStyle.Format = (col.Name == "Waktu") ? "dd/MM/yyyy HH:mm" : "dd/MM/yyyy";
                }

                ConfigureColumnLayout(col);
            }
        }

        // Mengatur proporsi lebar kolom agar kolom angka/kode ringkas dan kolom Catatan/Keterangan mendapat ruang lebih lebar
        private static void ConfigureColumnLayout(DataGridViewColumn col)
        {
            if (col == null) return;

            if (col.Name == "Jumlah" || col.Name == "Tersedia")
            {
                col.FillWeight = 48F;
                col.MinimumWidth = 55;
            }
            else if (col.Name == "KodeAset" || col.Name == "IdAlokasi" || col.Name == "NikKaryawan" || col.Name == "Nik" ||
                     col.Name == "Status" || col.Name == "Kategori")
            {
                col.FillWeight = 68F;
                col.MinimumWidth = 75;
            }
            else if (col.Name == "Waktu" || col.Name == "TanggalPinjam" || col.Name == "JenisAksi")
            {
                col.FillWeight = 82F;
                col.MinimumWidth = 90;
            }
            else if (col.Name == "Catatan" || col.Name == "Keterangan")
            {
                col.FillWeight = 175F;
                col.MinimumWidth = 150;
            }
            else
            {
                col.FillWeight = 100F;
                col.MinimumWidth = 90;
            }
        }

        private static void Grid_Resize(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid == null || grid.IsDisposed) return;

            if (grid.Rows.Count == 0)
            {
                grid.Invalidate();
            }
            else
            {
                grid.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            }
        }

        // Menggambar tampilan Empty State di tengah tabel ketika DataGridView tidak memiliki baris data
        private static void Grid_Paint(object sender, PaintEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid == null || grid.IsDisposed || grid.Rows.Count > 0) return;

            int headerHeight = grid.ColumnHeadersVisible ? grid.ColumnHeadersHeight : 0;
            Rectangle area = new Rectangle(0, headerHeight, grid.ClientSize.Width, Math.Max(0, grid.ClientSize.Height - headerHeight));
            if (area.Width < 100 || area.Height < 50) return;

            string pesanUtama = "Belum Ada Data";
            string pesanDetail = (grid.Tag as string) ?? GetDefaultEmptyMessage(grid.Name);
            if (pesanDetail.StartsWith("Tidak ditemukan", StringComparison.OrdinalIgnoreCase) ||
                pesanDetail.StartsWith("Tidak ada catatan riwayat yang cocok", StringComparison.OrdinalIgnoreCase))
            {
                pesanUtama = "Data Tidak Ditemukan";
            }

            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(248, 250, 252)))
            using (Pen borderPen = new Pen(Color.FromArgb(226, 232, 240), 1))
            using (SolidBrush titleBrush = new SolidBrush(Color.FromArgb(71, 85, 105)))
            using (SolidBrush subBrush = new SolidBrush(Color.FromArgb(100, 116, 139)))
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                int boxWidth = Math.Min(area.Width - 40, 460);
                int boxHeight = Math.Min(area.Height - 20, 82);
                Rectangle boxRect = new Rectangle(
                    area.X + (area.Width - boxWidth) / 2,
                    area.Y + (area.Height - boxHeight) / 2,
                    boxWidth,
                    boxHeight);

                e.Graphics.FillRectangle(bgBrush, boxRect);
                e.Graphics.DrawRectangle(borderPen, boxRect);

                Rectangle titleRect = new Rectangle(boxRect.X + 12, boxRect.Y + 12, boxRect.Width - 24, 26);
                Rectangle subRect = new Rectangle(boxRect.X + 12, boxRect.Y + 38, boxRect.Width - 24, boxRect.Height - 44);

                e.Graphics.DrawString(pesanUtama, _emptyTitleFont, titleBrush, titleRect, sf);
                e.Graphics.DrawString(pesanDetail, _emptySubFont, subBrush, subRect, sf);
            }
        }

        // Menentukan pesan Empty State bawaan sesuai nama kontrol DataGridView
        public static string GetDefaultEmptyMessage(string gridName)
        {
            if (gridName == "dgvAlokasi" || gridName == "dgvAlokasiAktif")
                return "Tidak ada aset yang sedang dipinjam saat ini.";
            if (gridName == "dgvRiwayat" || gridName == "dgvRiwayatTerbaru")
                return "Belum ada catatan riwayat aktivitas keluar-masuk aset.";
            if (gridName == "dgvAset")
                return "Belum ada data aset IT yang terdaftar.";
            if (gridName == "dgvKaryawan")
                return "Belum ada data karyawan yang terdaftar.";
            return "Data masih kosong.";
        }

        private static void Grid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null && !grid.IsDisposed)
            {
                ApplyColumnFormatting(grid);
                ColorizeRows(grid);
                if (grid.Rows.Count > 0)
                {
                    grid.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
                }
            }
        }

        // Menerapkan warna latar, warna seleksi, dan warna teks agar baris tetap mudah dibaca saat dipilih
        private static void ApplyRowColors(DataGridViewRow row, Color back, Color selBack, Color fore)
        {
            row.DefaultCellStyle.BackColor = back;
            row.DefaultCellStyle.SelectionBackColor = selBack;
            row.DefaultCellStyle.ForeColor = fore;
            row.DefaultCellStyle.SelectionForeColor = fore;
        }

        // Memberi warna baris otomatis pada tabel Riwayat (berdasarkan JenisAksi/kondisi) dan tabel Aset (berdasarkan Kondisi/Status)
        private static void ColorizeRows(DataGridView grid)
        {
            if (grid == null || grid.IsDisposed) return;

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.DataBoundItem is RiwayatItem riwayat)
                {
                    if (riwayat.JenisAksi == RiwayatAction.Peminjaman)
                    {
                        ApplyRowColors(row, Color.FromArgb(254, 243, 199), Color.FromArgb(253, 230, 138), Color.FromArgb(146, 64, 14));
                    }
                    else if (riwayat.JenisAksi == RiwayatAction.Pengembalian)
                    {
                        string catatan = riwayat.Catatan ?? "";
                        if (catatan.StartsWith("Kondisi kembali: Rusak", StringComparison.OrdinalIgnoreCase))
                        {
                            ApplyRowColors(row, Color.FromArgb(254, 226, 226), Color.FromArgb(254, 202, 202), Color.FromArgb(153, 27, 27));
                        }
                        else if (catatan.StartsWith("Kondisi kembali: Perbaikan", StringComparison.OrdinalIgnoreCase))
                        {
                            ApplyRowColors(row, Color.FromArgb(255, 237, 213), Color.FromArgb(254, 215, 170), Color.FromArgb(154, 52, 18));
                        }
                        else
                        {
                            ApplyRowColors(row, Color.FromArgb(209, 250, 229), Color.FromArgb(167, 243, 208), Color.FromArgb(6, 95, 70));
                        }
                    }
                    else if (riwayat.JenisAksi == RiwayatAction.TambahAset)
                    {
                        ApplyRowColors(row, Color.FromArgb(219, 234, 254), Color.FromArgb(191, 219, 254), Color.FromArgb(30, 64, 175));
                    }
                    else
                    {
                        // Abu-abu Slate untuk Hapus Aset atau aksi arsip lainnya
                        ApplyRowColors(row, Color.FromArgb(241, 245, 249), Color.FromArgb(226, 232, 240), Color.FromArgb(71, 85, 105));
                    }

                    if (grid.Columns.Contains("JenisAksi"))
                    {
                        row.Cells["JenisAksi"].Style.Font = _gridBoldFont;
                    }
                }
                else if (row.DataBoundItem is AssetItem asset)
                {
                    if (asset.Rusak > 0 || asset.Kondisi == AssetCondition.Rusak)
                    {
                        ApplyRowColors(row, Color.FromArgb(254, 226, 226), Color.FromArgb(254, 202, 202), Color.FromArgb(153, 27, 27));
                    }
                    else if (asset.Perbaikan > 0 || asset.Kondisi == AssetCondition.Perbaikan || asset.Bermasalah > 0)
                    {
                        ApplyRowColors(row, Color.FromArgb(254, 243, 199), Color.FromArgb(253, 230, 138), Color.FromArgb(146, 64, 14));
                    }
                    else if (asset.IsBorrowed)
                    {
                        ApplyRowColors(row, Color.FromArgb(239, 246, 255), Color.FromArgb(219, 234, 254), Color.FromArgb(30, 64, 175));
                    }
                }
            }
        }

        // Menghilangkan baris terpilih otomatis pada tabel DataGridView
        public static void ClearSelection(DataGridView grid)
        {
            if (grid == null || grid.IsDisposed) return;

            grid.CurrentCell = null;
            grid.ClearSelection();
        }

        // Memberi spasi pada judul kolom dari nama properti (contoh: "KodeAset" menjadi "Kode Aset")
        private static string FormatHeader(string name)
        {
            if (string.IsNullOrEmpty(name)) return "";

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                if (i > 0 && char.IsUpper(name[i]) && char.IsLower(name[i - 1]))
                {
                    sb.Append(' ');
                }
                sb.Append(name[i]);
            }
            return sb.ToString();
        }
    }
}
