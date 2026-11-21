namespace ITAssetManagement.Views
{
    partial class UC_Alokasi
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.grpPinjam = new System.Windows.Forms.GroupBox();
            this.tlpPinjam = new System.Windows.Forms.TableLayoutPanel();
            this.lblAset = new System.Windows.Forms.Label();
            this.lblJumlahPinjam = new System.Windows.Forms.Label();
            this.lblKaryawan = new System.Windows.Forms.Label();
            this.lblTanggal = new System.Windows.Forms.Label();
            this.cmbAset = new System.Windows.Forms.ComboBox();
            this.nudJumlahPinjam = new System.Windows.Forms.NumericUpDown();
            this.cmbKaryawan = new System.Windows.Forms.ComboBox();
            this.dtpTanggal = new System.Windows.Forms.DateTimePicker();
            this.lblKeterangan = new System.Windows.Forms.Label();
            this.txtKeterangan = new System.Windows.Forms.TextBox();
            this.btnAlokasi = new System.Windows.Forms.Button();
            this.pnlDaftar = new System.Windows.Forms.Panel();
            this.dgvAlokasi = new System.Windows.Forms.DataGridView();
            this.btnKembalikan = new System.Windows.Forms.Button();
            this.nudJumlahKondisi = new System.Windows.Forms.NumericUpDown();
            this.lblJumlahKondisi = new System.Windows.Forms.Label();
            this.cmbKondisiKembali = new System.Windows.Forms.ComboBox();
            this.lblKondisiKembali = new System.Windows.Forms.Label();
            this.lblSubTabel = new System.Windows.Forms.Label();
            this.grpPinjam.SuspendLayout();
            this.tlpPinjam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudJumlahPinjam)).BeginInit();
            this.pnlDaftar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlokasi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudJumlahKondisi)).BeginInit();
            this.SuspendLayout();
            // 
            // grpPinjam
            // 
            this.grpPinjam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPinjam.BackColor = System.Drawing.Color.White;
            this.grpPinjam.Controls.Add(this.tlpPinjam);
            this.grpPinjam.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpPinjam.Location = new System.Drawing.Point(20, 15);
            this.grpPinjam.Name = "grpPinjam";
            this.grpPinjam.Size = new System.Drawing.Size(853, 175);
            this.grpPinjam.TabIndex = 0;
            this.grpPinjam.TabStop = false;
            this.grpPinjam.Text = "Form Peminjaman / Alokasi Aset ke Karyawan";
            // 
            // tlpPinjam
            // 
            this.tlpPinjam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpPinjam.ColumnCount = 4;
            this.tlpPinjam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tlpPinjam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tlpPinjam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.tlpPinjam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpPinjam.Controls.Add(this.lblAset, 0, 0);
            this.tlpPinjam.Controls.Add(this.lblJumlahPinjam, 1, 0);
            this.tlpPinjam.Controls.Add(this.lblKaryawan, 2, 0);
            this.tlpPinjam.Controls.Add(this.lblTanggal, 3, 0);
            this.tlpPinjam.Controls.Add(this.cmbAset, 0, 1);
            this.tlpPinjam.Controls.Add(this.nudJumlahPinjam, 1, 1);
            this.tlpPinjam.Controls.Add(this.cmbKaryawan, 2, 1);
            this.tlpPinjam.Controls.Add(this.dtpTanggal, 3, 1);
            this.tlpPinjam.Controls.Add(this.lblKeterangan, 0, 2);
            this.tlpPinjam.Controls.Add(this.txtKeterangan, 0, 3);
            this.tlpPinjam.Controls.Add(this.btnAlokasi, 3, 3);
            this.tlpPinjam.Location = new System.Drawing.Point(16, 28);
            this.tlpPinjam.Name = "tlpPinjam";
            this.tlpPinjam.RowCount = 4;
            this.tlpPinjam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpPinjam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tlpPinjam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpPinjam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tlpPinjam.Size = new System.Drawing.Size(821, 130);
            this.tlpPinjam.TabIndex = 0;
            // 
            // lblAset
            // 
            this.lblAset.AutoSize = true;
            this.lblAset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAset.Location = new System.Drawing.Point(3, 0);
            this.lblAset.Name = "lblAset";
            this.lblAset.Size = new System.Drawing.Size(186, 20);
            this.lblAset.TabIndex = 0;
            this.lblAset.Text = "Pilih Aset (Status: Tersedia)";
            // 
            // lblJumlahPinjam
            // 
            this.lblJumlahPinjam.AutoSize = true;
            this.lblJumlahPinjam.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblJumlahPinjam.Location = new System.Drawing.Point(282, 0);
            this.lblJumlahPinjam.Name = "lblJumlahPinjam";
            this.lblJumlahPinjam.Size = new System.Drawing.Size(104, 20);
            this.lblJumlahPinjam.TabIndex = 1;
            this.lblJumlahPinjam.Text = "Jumlah Pinjam";
            // 
            // lblKaryawan
            // 
            this.lblKaryawan.AutoSize = true;
            this.lblKaryawan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblKaryawan.Location = new System.Drawing.Point(429, 0);
            this.lblKaryawan.Name = "lblKaryawan";
            this.lblKaryawan.Size = new System.Drawing.Size(170, 20);
            this.lblKaryawan.TabIndex = 2;
            this.lblKaryawan.Text = "Pilih Karyawan Penerima";
            // 
            // lblTanggal
            // 
            this.lblTanggal.AutoSize = true;
            this.lblTanggal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTanggal.Location = new System.Drawing.Point(658, 0);
            this.lblTanggal.Name = "lblTanggal";
            this.lblTanggal.Size = new System.Drawing.Size(110, 20);
            this.lblTanggal.TabIndex = 3;
            this.lblTanggal.Text = "Tanggal Pinjam";
            // 
            // cmbAset
            // 
            this.cmbAset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAset.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbAset.FormattingEnabled = true;
            this.cmbAset.Location = new System.Drawing.Point(3, 25);
            this.cmbAset.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.cmbAset.Name = "cmbAset";
            this.cmbAset.Size = new System.Drawing.Size(264, 29);
            this.cmbAset.TabIndex = 4;
            this.cmbAset.SelectedIndexChanged += new System.EventHandler(this.cmbAset_SelectedIndexChanged);
            // 
            // nudJumlahPinjam
            // 
            this.nudJumlahPinjam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudJumlahPinjam.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.nudJumlahPinjam.Location = new System.Drawing.Point(282, 25);
            this.nudJumlahPinjam.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.nudJumlahPinjam.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudJumlahPinjam.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudJumlahPinjam.Name = "nudJumlahPinjam";
            this.nudJumlahPinjam.Size = new System.Drawing.Size(132, 29);
            this.nudJumlahPinjam.TabIndex = 5;
            this.nudJumlahPinjam.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmbKaryawan
            // 
            this.cmbKaryawan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbKaryawan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKaryawan.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbKaryawan.FormattingEnabled = true;
            this.cmbKaryawan.Location = new System.Drawing.Point(429, 25);
            this.cmbKaryawan.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.cmbKaryawan.Name = "cmbKaryawan";
            this.cmbKaryawan.Size = new System.Drawing.Size(214, 29);
            this.cmbKaryawan.TabIndex = 6;
            // 
            // dtpTanggal
            // 
            this.dtpTanggal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpTanggal.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpTanggal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTanggal.Location = new System.Drawing.Point(658, 25);
            this.dtpTanggal.Name = "dtpTanggal";
            this.dtpTanggal.Size = new System.Drawing.Size(160, 29);
            this.dtpTanggal.TabIndex = 7;
            // 
            // lblKeterangan
            // 
            this.lblKeterangan.AutoSize = true;
            this.tlpPinjam.SetColumnSpan(this.lblKeterangan, 3);
            this.lblKeterangan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblKeterangan.Location = new System.Drawing.Point(3, 60);
            this.lblKeterangan.Name = "lblKeterangan";
            this.lblKeterangan.Size = new System.Drawing.Size(166, 20);
            this.lblKeterangan.TabIndex = 8;
            this.lblKeterangan.Text = "Keperluan / Keterangan";
            // 
            // txtKeterangan
            // 
            this.tlpPinjam.SetColumnSpan(this.txtKeterangan, 3);
            this.txtKeterangan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKeterangan.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtKeterangan.Location = new System.Drawing.Point(3, 86);
            this.txtKeterangan.Margin = new System.Windows.Forms.Padding(3, 4, 12, 3);
            this.txtKeterangan.MaxLength = 255;
            this.txtKeterangan.Name = "txtKeterangan";
            this.txtKeterangan.Size = new System.Drawing.Size(640, 29);
            this.txtKeterangan.TabIndex = 9;
            // 
            // btnAlokasi
            // 
            this.btnAlokasi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.btnAlokasi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAlokasi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlokasi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnAlokasi.ForeColor = System.Drawing.Color.White;
            this.btnAlokasi.Location = new System.Drawing.Point(658, 84);
            this.btnAlokasi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 4);
            this.btnAlokasi.Name = "btnAlokasi";
            this.btnAlokasi.Size = new System.Drawing.Size(160, 42);
            this.btnAlokasi.TabIndex = 10;
            this.btnAlokasi.Text = "Alokasikan Aset";
            this.btnAlokasi.UseVisualStyleBackColor = false;
            this.btnAlokasi.Click += new System.EventHandler(this.btnAlokasi_Click);
            // 
            // pnlDaftar
            // 
            this.pnlDaftar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDaftar.Controls.Add(this.dgvAlokasi);
            this.pnlDaftar.Controls.Add(this.btnKembalikan);
            this.pnlDaftar.Controls.Add(this.nudJumlahKondisi);
            this.pnlDaftar.Controls.Add(this.lblJumlahKondisi);
            this.pnlDaftar.Controls.Add(this.cmbKondisiKembali);
            this.pnlDaftar.Controls.Add(this.lblKondisiKembali);
            this.pnlDaftar.Controls.Add(this.lblSubTabel);
            this.pnlDaftar.Location = new System.Drawing.Point(20, 200);
            this.pnlDaftar.Name = "pnlDaftar";
            this.pnlDaftar.Size = new System.Drawing.Size(853, 365);
            this.pnlDaftar.TabIndex = 1;
            // 
            // dgvAlokasi
            // 
            this.dgvAlokasi.AllowUserToAddRows = false;
            this.dgvAlokasi.AllowUserToDeleteRows = false;
            this.dgvAlokasi.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAlokasi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAlokasi.BackgroundColor = System.Drawing.Color.White;
            this.dgvAlokasi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAlokasi.Location = new System.Drawing.Point(0, 44);
            this.dgvAlokasi.Name = "dgvAlokasi";
            this.dgvAlokasi.ReadOnly = true;
            this.dgvAlokasi.RowHeadersVisible = false;
            this.dgvAlokasi.RowHeadersWidth = 51;
            this.dgvAlokasi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAlokasi.Size = new System.Drawing.Size(853, 321);
            this.dgvAlokasi.TabIndex = 4;
            this.dgvAlokasi.SelectionChanged += new System.EventHandler(this.KondisiKembali_Changed);
            // 
            // btnKembalikan
            // 
            this.btnKembalikan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKembalikan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnKembalikan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKembalikan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnKembalikan.ForeColor = System.Drawing.Color.White;
            this.btnKembalikan.Location = new System.Drawing.Point(673, 1);
            this.btnKembalikan.Name = "btnKembalikan";
            this.btnKembalikan.Size = new System.Drawing.Size(180, 36);
            this.btnKembalikan.TabIndex = 3;
            this.btnKembalikan.Text = "Kembalikan Aset";
            this.btnKembalikan.UseVisualStyleBackColor = false;
            this.btnKembalikan.Click += new System.EventHandler(this.btnKembalikan_Click);
            // 
            // nudJumlahKondisi
            // 
            this.nudJumlahKondisi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudJumlahKondisi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudJumlahKondisi.Location = new System.Drawing.Point(600, 6);
            this.nudJumlahKondisi.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudJumlahKondisi.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudJumlahKondisi.Name = "nudJumlahKondisi";
            this.nudJumlahKondisi.Size = new System.Drawing.Size(60, 27);
            this.nudJumlahKondisi.TabIndex = 6;
            this.nudJumlahKondisi.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblJumlahKondisi
            // 
            this.lblJumlahKondisi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblJumlahKondisi.AutoSize = true;
            this.lblJumlahKondisi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblJumlahKondisi.Location = new System.Drawing.Point(560, 10);
            this.lblJumlahKondisi.Name = "lblJumlahKondisi";
            this.lblJumlahKondisi.Size = new System.Drawing.Size(39, 20);
            this.lblJumlahKondisi.TabIndex = 5;
            this.lblJumlahKondisi.Text = "Unit:";
            // 
            // cmbKondisiKembali
            // 
            this.cmbKondisiKembali.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbKondisiKembali.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKondisiKembali.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbKondisiKembali.FormattingEnabled = true;
            this.cmbKondisiKembali.Items.AddRange(new object[] {
            "Baik",
            "Perbaikan",
            "Rusak"});
            this.cmbKondisiKembali.Location = new System.Drawing.Point(452, 6);
            this.cmbKondisiKembali.Name = "cmbKondisiKembali";
            this.cmbKondisiKembali.Size = new System.Drawing.Size(100, 28);
            this.cmbKondisiKembali.TabIndex = 2;
            this.cmbKondisiKembali.SelectedIndexChanged += new System.EventHandler(this.KondisiKembali_Changed);
            // 
            // lblKondisiKembali
            // 
            this.lblKondisiKembali.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKondisiKembali.AutoSize = true;
            this.lblKondisiKembali.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblKondisiKembali.Location = new System.Drawing.Point(318, 10);
            this.lblKondisiKembali.Name = "lblKondisiKembali";
            this.lblKondisiKembali.Size = new System.Drawing.Size(120, 20);
            this.lblKondisiKembali.TabIndex = 1;
            this.lblKondisiKembali.Text = "Kondisi Kembali:";
            // 
            // lblSubTabel
            // 
            this.lblSubTabel.AutoSize = true;
            this.lblSubTabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSubTabel.Location = new System.Drawing.Point(0, 8);
            this.lblSubTabel.Name = "lblSubTabel";
            this.lblSubTabel.Size = new System.Drawing.Size(305, 23);
            this.lblSubTabel.TabIndex = 0;
            this.lblSubTabel.Text = "Daftar Aset Sedang Dipinjam (Aktif)";
            // 
            // UC_Alokasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.pnlDaftar);
            this.Controls.Add(this.grpPinjam);
            this.Name = "UC_Alokasi";
            this.Size = new System.Drawing.Size(893, 585);
            this.grpPinjam.ResumeLayout(false);
            this.tlpPinjam.ResumeLayout(false);
            this.tlpPinjam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudJumlahPinjam)).EndInit();
            this.pnlDaftar.ResumeLayout(false);
            this.pnlDaftar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlokasi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudJumlahKondisi)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPinjam;
        private System.Windows.Forms.TableLayoutPanel tlpPinjam;
        private System.Windows.Forms.Label lblAset;
        private System.Windows.Forms.ComboBox cmbAset;
        private System.Windows.Forms.Label lblJumlahPinjam;
        private System.Windows.Forms.NumericUpDown nudJumlahPinjam;
        private System.Windows.Forms.Label lblKaryawan;
        private System.Windows.Forms.ComboBox cmbKaryawan;
        private System.Windows.Forms.Label lblTanggal;
        private System.Windows.Forms.DateTimePicker dtpTanggal;
        private System.Windows.Forms.Label lblKeterangan;
        private System.Windows.Forms.TextBox txtKeterangan;
        private System.Windows.Forms.Button btnAlokasi;
        private System.Windows.Forms.Panel pnlDaftar;
        private System.Windows.Forms.Label lblSubTabel;
        private System.Windows.Forms.Label lblKondisiKembali;
        private System.Windows.Forms.ComboBox cmbKondisiKembali;
        private System.Windows.Forms.Label lblJumlahKondisi;
        private System.Windows.Forms.NumericUpDown nudJumlahKondisi;
        private System.Windows.Forms.Button btnKembalikan;
        private System.Windows.Forms.DataGridView dgvAlokasi;
    }
}
