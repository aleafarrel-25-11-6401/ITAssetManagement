namespace ITAssetManagement.Views
{
    partial class UC_Asset
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
            this.grpForm = new System.Windows.Forms.GroupBox();
            this.tlpKondisi = new System.Windows.Forms.TableLayoutPanel();
            this.lblKondisi = new System.Windows.Forms.Label();
            this.lblPerbaikan = new System.Windows.Forms.Label();
            this.lblRusak = new System.Windows.Forms.Label();
            this.lblStatusStok = new System.Windows.Forms.Label();
            this.cmbKondisi = new System.Windows.Forms.ComboBox();
            this.nudPerbaikan = new System.Windows.Forms.NumericUpDown();
            this.nudRusak = new System.Windows.Forms.NumericUpDown();
            this.lblRincianStok = new System.Windows.Forms.Label();
            this.tlpInputs = new System.Windows.Forms.TableLayoutPanel();
            this.lblKode = new System.Windows.Forms.Label();
            this.lblNama = new System.Windows.Forms.Label();
            this.lblKategori = new System.Windows.Forms.Label();
            this.lblJumlah = new System.Windows.Forms.Label();
            this.txtKodeAset = new System.Windows.Forms.TextBox();
            this.txtNamaAset = new System.Windows.Forms.TextBox();
            this.cmbKategori = new System.Windows.Forms.ComboBox();
            this.nudJumlah = new System.Windows.Forms.NumericUpDown();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnUbah = new System.Windows.Forms.Button();
            this.btnTambah = new System.Windows.Forms.Button();
            this.lblCari = new System.Windows.Forms.Label();
            this.txtCari = new System.Windows.Forms.TextBox();
            this.dgvAset = new System.Windows.Forms.DataGridView();
            this.grpForm.SuspendLayout();
            this.tlpKondisi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPerbaikan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRusak)).BeginInit();
            this.tlpInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudJumlah)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAset)).BeginInit();
            this.SuspendLayout();
            // 
            // grpForm
            // 
            this.grpForm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpForm.BackColor = System.Drawing.Color.White;
            this.grpForm.Controls.Add(this.tlpKondisi);
            this.grpForm.Controls.Add(this.tlpInputs);
            this.grpForm.Controls.Add(this.btnReset);
            this.grpForm.Controls.Add(this.btnHapus);
            this.grpForm.Controls.Add(this.btnUbah);
            this.grpForm.Controls.Add(this.btnTambah);
            this.grpForm.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpForm.Location = new System.Drawing.Point(20, 15);
            this.grpForm.Name = "grpForm";
            this.grpForm.Size = new System.Drawing.Size(853, 200);
            this.grpForm.TabIndex = 0;
            this.grpForm.TabStop = false;
            this.grpForm.Text = "Form Data Aset IT";
            // 
            // tlpKondisi
            // 
            this.tlpKondisi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpKondisi.ColumnCount = 4;
            this.tlpKondisi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tlpKondisi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tlpKondisi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tlpKondisi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44F));
            this.tlpKondisi.Controls.Add(this.lblKondisi, 0, 0);
            this.tlpKondisi.Controls.Add(this.lblPerbaikan, 1, 0);
            this.tlpKondisi.Controls.Add(this.lblRusak, 2, 0);
            this.tlpKondisi.Controls.Add(this.lblStatusStok, 3, 0);
            this.tlpKondisi.Controls.Add(this.cmbKondisi, 0, 1);
            this.tlpKondisi.Controls.Add(this.nudPerbaikan, 1, 1);
            this.tlpKondisi.Controls.Add(this.nudRusak, 2, 1);
            this.tlpKondisi.Controls.Add(this.lblRincianStok, 3, 1);
            this.tlpKondisi.Location = new System.Drawing.Point(16, 86);
            this.tlpKondisi.Name = "tlpKondisi";
            this.tlpKondisi.RowCount = 2;
            this.tlpKondisi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpKondisi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpKondisi.Size = new System.Drawing.Size(821, 56);
            this.tlpKondisi.TabIndex = 1;
            // 
            // lblKondisi
            // 
            this.lblKondisi.AutoSize = true;
            this.lblKondisi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblKondisi.Location = new System.Drawing.Point(3, 0);
            this.lblKondisi.Name = "lblKondisi";
            this.lblKondisi.Size = new System.Drawing.Size(102, 20);
            this.lblKondisi.TabIndex = 0;
            this.lblKondisi.Text = "Preset Kondisi";
            // 
            // lblPerbaikan
            // 
            this.lblPerbaikan.AutoSize = true;
            this.lblPerbaikan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPerbaikan.Location = new System.Drawing.Point(200, 0);
            this.lblPerbaikan.Name = "lblPerbaikan";
            this.lblPerbaikan.Size = new System.Drawing.Size(114, 20);
            this.lblPerbaikan.TabIndex = 1;
            this.lblPerbaikan.Text = "Perbaikan (Unit)";
            // 
            // lblRusak
            // 
            this.lblRusak.AutoSize = true;
            this.lblRusak.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRusak.Location = new System.Drawing.Point(331, 0);
            this.lblRusak.Name = "lblRusak";
            this.lblRusak.Size = new System.Drawing.Size(88, 20);
            this.lblRusak.TabIndex = 2;
            this.lblRusak.Text = "Rusak (Unit)";
            // 
            // lblStatusStok
            // 
            this.lblStatusStok.AutoSize = true;
            this.lblStatusStok.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatusStok.Location = new System.Drawing.Point(462, 0);
            this.lblStatusStok.Name = "lblStatusStok";
            this.lblStatusStok.Size = new System.Drawing.Size(113, 20);
            this.lblStatusStok.TabIndex = 3;
            this.lblStatusStok.Text = "Status Stok Unit";
            // 
            // cmbKondisi
            // 
            this.cmbKondisi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbKondisi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKondisi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbKondisi.FormattingEnabled = true;
            this.cmbKondisi.Items.AddRange(new object[] {
            "Baik",
            "Perbaikan",
            "Rusak"});
            this.cmbKondisi.Location = new System.Drawing.Point(3, 25);
            this.cmbKondisi.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.cmbKondisi.Name = "cmbKondisi";
            this.cmbKondisi.Size = new System.Drawing.Size(184, 29);
            this.cmbKondisi.TabIndex = 4;
            this.cmbKondisi.SelectedIndexChanged += new System.EventHandler(this.cmbKondisi_SelectedIndexChanged);
            // 
            // nudPerbaikan
            // 
            this.nudPerbaikan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudPerbaikan.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.nudPerbaikan.Location = new System.Drawing.Point(200, 25);
            this.nudPerbaikan.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.nudPerbaikan.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudPerbaikan.Name = "nudPerbaikan";
            this.nudPerbaikan.Size = new System.Drawing.Size(118, 29);
            this.nudPerbaikan.TabIndex = 5;
            this.nudPerbaikan.ValueChanged += new System.EventHandler(this.KondisiUnit_Changed);
            // 
            // nudRusak
            // 
            this.nudRusak.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudRusak.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.nudRusak.Location = new System.Drawing.Point(331, 25);
            this.nudRusak.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.nudRusak.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudRusak.Name = "nudRusak";
            this.nudRusak.Size = new System.Drawing.Size(118, 29);
            this.nudRusak.TabIndex = 6;
            this.nudRusak.ValueChanged += new System.EventHandler(this.KondisiUnit_Changed);
            // 
            // lblRincianStok
            // 
            this.lblRincianStok.AutoEllipsis = true;
            this.lblRincianStok.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.lblRincianStok.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRincianStok.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRincianStok.Font = new System.Drawing.Font("Segoe UI", 8.75F, System.Drawing.FontStyle.Bold);
            this.lblRincianStok.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblRincianStok.Location = new System.Drawing.Point(462, 25);
            this.lblRincianStok.Margin = new System.Windows.Forms.Padding(3, 3, 0, 4);
            this.lblRincianStok.Name = "lblRincianStok";
            this.lblRincianStok.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblRincianStok.Size = new System.Drawing.Size(359, 27);
            this.lblRincianStok.TabIndex = 7;
            this.lblRincianStok.Text = "Baik: 1 | Perbaikan: 0 | Rusak: 0";
            this.lblRincianStok.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tlpInputs
            // 
            this.tlpInputs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpInputs.ColumnCount = 4;
            this.tlpInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tlpInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.tlpInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tlpInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tlpInputs.Controls.Add(this.lblKode, 0, 0);
            this.tlpInputs.Controls.Add(this.lblNama, 1, 0);
            this.tlpInputs.Controls.Add(this.lblKategori, 2, 0);
            this.tlpInputs.Controls.Add(this.lblJumlah, 3, 0);
            this.tlpInputs.Controls.Add(this.txtKodeAset, 0, 1);
            this.tlpInputs.Controls.Add(this.txtNamaAset, 1, 1);
            this.tlpInputs.Controls.Add(this.cmbKategori, 2, 1);
            this.tlpInputs.Controls.Add(this.nudJumlah, 3, 1);
            this.tlpInputs.Location = new System.Drawing.Point(16, 26);
            this.tlpInputs.Name = "tlpInputs";
            this.tlpInputs.RowCount = 2;
            this.tlpInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpInputs.Size = new System.Drawing.Size(821, 56);
            this.tlpInputs.TabIndex = 0;
            // 
            // lblKode
            // 
            this.lblKode.AutoSize = true;
            this.lblKode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblKode.Location = new System.Drawing.Point(3, 0);
            this.lblKode.Name = "lblKode";
            this.lblKode.Size = new System.Drawing.Size(77, 20);
            this.lblKode.TabIndex = 0;
            this.lblKode.Text = "Kode Aset";
            // 
            // lblNama
            // 
            this.lblNama.AutoSize = true;
            this.lblNama.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNama.Location = new System.Drawing.Point(150, 0);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(125, 20);
            this.lblNama.TabIndex = 1;
            this.lblNama.Text = "Nama / Tipe Aset";
            // 
            // lblKategori
            // 
            this.lblKategori.AutoSize = true;
            this.lblKategori.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblKategori.Location = new System.Drawing.Point(494, 0);
            this.lblKategori.Name = "lblKategori";
            this.lblKategori.Size = new System.Drawing.Size(66, 20);
            this.lblKategori.TabIndex = 2;
            this.lblKategori.Text = "Kategori";
            // 
            // lblJumlah
            // 
            this.lblJumlah.AutoSize = true;
            this.lblJumlah.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblJumlah.Location = new System.Drawing.Point(691, 0);
            this.lblJumlah.Name = "lblJumlah";
            this.lblJumlah.Size = new System.Drawing.Size(86, 20);
            this.lblJumlah.TabIndex = 3;
            this.lblJumlah.Text = "Jumlah Unit";
            // 
            // txtKodeAset
            // 
            this.txtKodeAset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKodeAset.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtKodeAset.Location = new System.Drawing.Point(3, 25);
            this.txtKodeAset.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtKodeAset.Name = "txtKodeAset";
            this.txtKodeAset.ReadOnly = true;
            this.txtKodeAset.Size = new System.Drawing.Size(134, 29);
            this.txtKodeAset.TabIndex = 5;
            // 
            // txtNamaAset
            // 
            this.txtNamaAset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNamaAset.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNamaAset.Location = new System.Drawing.Point(150, 25);
            this.txtNamaAset.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtNamaAset.MaxLength = 100;
            this.txtNamaAset.Name = "txtNamaAset";
            this.txtNamaAset.Size = new System.Drawing.Size(331, 29);
            this.txtNamaAset.TabIndex = 6;
            // 
            // cmbKategori
            // 
            this.cmbKategori.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbKategori.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKategori.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbKategori.FormattingEnabled = true;
            this.cmbKategori.Items.AddRange(new object[] {
            "Laptop",
            "Monitor",
            "Periferal",
            "Jaringan",
            "Lainnya"});
            this.cmbKategori.Location = new System.Drawing.Point(494, 25);
            this.cmbKategori.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.cmbKategori.Name = "cmbKategori";
            this.cmbKategori.Size = new System.Drawing.Size(184, 29);
            this.cmbKategori.TabIndex = 7;
            // 
            // nudJumlah
            // 
            this.nudJumlah.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudJumlah.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.nudJumlah.Location = new System.Drawing.Point(691, 25);
            this.nudJumlah.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.nudJumlah.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudJumlah.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudJumlah.Name = "nudJumlah";
            this.nudJumlah.Size = new System.Drawing.Size(130, 29);
            this.nudJumlah.TabIndex = 8;
            this.nudJumlah.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudJumlah.ValueChanged += new System.EventHandler(this.KondisiUnit_Changed);
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReset.Location = new System.Drawing.Point(432, 152);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(110, 36);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset Form";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnHapus
            // 
            this.btnHapus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnHapus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHapus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnHapus.ForeColor = System.Drawing.Color.White;
            this.btnHapus.Location = new System.Drawing.Point(310, 152);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(110, 36);
            this.btnHapus.TabIndex = 4;
            this.btnHapus.Text = "Hapus";
            this.btnHapus.UseVisualStyleBackColor = false;
            this.btnHapus.Click += new System.EventHandler(this.btnHapus_Click);
            // 
            // btnUbah
            // 
            this.btnUbah.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.btnUbah.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUbah.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnUbah.ForeColor = System.Drawing.Color.White;
            this.btnUbah.Location = new System.Drawing.Point(168, 152);
            this.btnUbah.Name = "btnUbah";
            this.btnUbah.Size = new System.Drawing.Size(130, 36);
            this.btnUbah.TabIndex = 3;
            this.btnUbah.Text = "Simpan Ubah";
            this.btnUbah.UseVisualStyleBackColor = false;
            this.btnUbah.Click += new System.EventHandler(this.btnUbah_Click);
            // 
            // btnTambah
            // 
            this.btnTambah.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.btnTambah.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTambah.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTambah.ForeColor = System.Drawing.Color.White;
            this.btnTambah.Location = new System.Drawing.Point(19, 152);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(138, 36);
            this.btnTambah.TabIndex = 2;
            this.btnTambah.Text = "+ Tambah Aset";
            this.btnTambah.UseVisualStyleBackColor = false;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // lblCari
            // 
            this.lblCari.AutoSize = true;
            this.lblCari.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCari.Location = new System.Drawing.Point(20, 229);
            this.lblCari.Name = "lblCari";
            this.lblCari.Size = new System.Drawing.Size(75, 21);
            this.lblCari.TabIndex = 1;
            this.lblCari.Text = "Cari Aset:";
            // 
            // txtCari
            // 
            this.txtCari.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCari.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtCari.Location = new System.Drawing.Point(102, 225);
            this.txtCari.Name = "txtCari";
            this.txtCari.Size = new System.Drawing.Size(320, 29);
            this.txtCari.TabIndex = 2;
            this.txtCari.TextChanged += new System.EventHandler(this.txtCari_TextChanged);
            // 
            // dgvAset
            // 
            this.dgvAset.AllowUserToAddRows = false;
            this.dgvAset.AllowUserToDeleteRows = false;
            this.dgvAset.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAset.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAset.BackgroundColor = System.Drawing.Color.White;
            this.dgvAset.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAset.Location = new System.Drawing.Point(20, 263);
            this.dgvAset.Name = "dgvAset";
            this.dgvAset.ReadOnly = true;
            this.dgvAset.RowHeadersVisible = false;
            this.dgvAset.RowHeadersWidth = 51;
            this.dgvAset.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAset.Size = new System.Drawing.Size(853, 302);
            this.dgvAset.TabIndex = 3;
            this.dgvAset.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAset_CellClick);
            // 
            // UC_Asset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.dgvAset);
            this.Controls.Add(this.txtCari);
            this.Controls.Add(this.lblCari);
            this.Controls.Add(this.grpForm);
            this.Name = "UC_Asset";
            this.Size = new System.Drawing.Size(893, 585);
            this.grpForm.ResumeLayout(false);
            this.tlpKondisi.ResumeLayout(false);
            this.tlpKondisi.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPerbaikan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRusak)).EndInit();
            this.tlpInputs.ResumeLayout(false);
            this.tlpInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudJumlah)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpForm;
        private System.Windows.Forms.TableLayoutPanel tlpInputs;
        private System.Windows.Forms.Label lblKode;
        private System.Windows.Forms.TextBox txtKodeAset;
        private System.Windows.Forms.Label lblNama;
        private System.Windows.Forms.TextBox txtNamaAset;
        private System.Windows.Forms.Label lblKategori;
        private System.Windows.Forms.ComboBox cmbKategori;
        private System.Windows.Forms.Label lblJumlah;
        private System.Windows.Forms.NumericUpDown nudJumlah;
        private System.Windows.Forms.TableLayoutPanel tlpKondisi;
        private System.Windows.Forms.Label lblKondisi;
        private System.Windows.Forms.Label lblPerbaikan;
        private System.Windows.Forms.Label lblRusak;
        private System.Windows.Forms.Label lblStatusStok;
        private System.Windows.Forms.ComboBox cmbKondisi;
        private System.Windows.Forms.NumericUpDown nudPerbaikan;
        private System.Windows.Forms.NumericUpDown nudRusak;
        private System.Windows.Forms.Label lblRincianStok;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.Button btnUbah;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblCari;
        private System.Windows.Forms.TextBox txtCari;
        private System.Windows.Forms.DataGridView dgvAset;
    }
}
