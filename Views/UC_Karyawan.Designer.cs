namespace ITAssetManagement.Views
{
    partial class UC_Karyawan
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
            this.tlpInputs = new System.Windows.Forms.TableLayoutPanel();
            this.lblNik = new System.Windows.Forms.Label();
            this.lblNama = new System.Windows.Forms.Label();
            this.lblDivisi = new System.Windows.Forms.Label();
            this.lblJabatan = new System.Windows.Forms.Label();
            this.lblKontak = new System.Windows.Forms.Label();
            this.txtNik = new System.Windows.Forms.TextBox();
            this.txtNama = new System.Windows.Forms.TextBox();
            this.cmbDivisi = new System.Windows.Forms.ComboBox();
            this.txtJabatan = new System.Windows.Forms.TextBox();
            this.txtKontak = new System.Windows.Forms.TextBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnUbah = new System.Windows.Forms.Button();
            this.btnTambah = new System.Windows.Forms.Button();
            this.lblCari = new System.Windows.Forms.Label();
            this.txtCari = new System.Windows.Forms.TextBox();
            this.dgvKaryawan = new System.Windows.Forms.DataGridView();
            this.grpForm.SuspendLayout();
            this.tlpInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKaryawan)).BeginInit();
            this.SuspendLayout();
            // 
            // grpForm
            // 
            this.grpForm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpForm.BackColor = System.Drawing.Color.White;
            this.grpForm.Controls.Add(this.tlpInputs);
            this.grpForm.Controls.Add(this.btnReset);
            this.grpForm.Controls.Add(this.btnHapus);
            this.grpForm.Controls.Add(this.btnUbah);
            this.grpForm.Controls.Add(this.btnTambah);
            this.grpForm.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpForm.Location = new System.Drawing.Point(20, 15);
            this.grpForm.Name = "grpForm";
            this.grpForm.Size = new System.Drawing.Size(853, 170);
            this.grpForm.TabIndex = 0;
            this.grpForm.TabStop = false;
            this.grpForm.Text = "Form Data Karyawan (Penerima Aset)";
            // 
            // tlpInputs
            // 
            this.tlpInputs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpInputs.ColumnCount = 5;
            this.tlpInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27F));
            this.tlpInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19F));
            this.tlpInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19F));
            this.tlpInputs.Controls.Add(this.lblNik, 0, 0);
            this.tlpInputs.Controls.Add(this.lblNama, 1, 0);
            this.tlpInputs.Controls.Add(this.lblDivisi, 2, 0);
            this.tlpInputs.Controls.Add(this.lblJabatan, 3, 0);
            this.tlpInputs.Controls.Add(this.lblKontak, 4, 0);
            this.tlpInputs.Controls.Add(this.txtNik, 0, 1);
            this.tlpInputs.Controls.Add(this.txtNama, 1, 1);
            this.tlpInputs.Controls.Add(this.cmbDivisi, 2, 1);
            this.tlpInputs.Controls.Add(this.txtJabatan, 3, 1);
            this.tlpInputs.Controls.Add(this.txtKontak, 4, 1);
            this.tlpInputs.Location = new System.Drawing.Point(16, 28);
            this.tlpInputs.Name = "tlpInputs";
            this.tlpInputs.RowCount = 2;
            this.tlpInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpInputs.Size = new System.Drawing.Size(821, 62);
            this.tlpInputs.TabIndex = 0;
            // 
            // lblNik
            // 
            this.lblNik.AutoSize = true;
            this.lblNik.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNik.Location = new System.Drawing.Point(3, 0);
            this.lblNik.Name = "lblNik";
            this.lblNik.Size = new System.Drawing.Size(92, 20);
            this.lblNik.TabIndex = 0;
            this.lblNik.Text = "ID Karyawan";
            // 
            // lblNama
            // 
            this.lblNama.AutoSize = true;
            this.lblNama.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNama.Location = new System.Drawing.Point(126, 0);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(109, 20);
            this.lblNama.TabIndex = 1;
            this.lblNama.Text = "Nama Lengkap";
            // 
            // lblDivisi
            // 
            this.lblDivisi.AutoSize = true;
            this.lblDivisi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDivisi.Location = new System.Drawing.Point(347, 0);
            this.lblDivisi.Name = "lblDivisi";
            this.lblDivisi.Size = new System.Drawing.Size(45, 20);
            this.lblDivisi.TabIndex = 2;
            this.lblDivisi.Text = "Divisi";
            // 
            // lblJabatan
            // 
            this.lblJabatan.AutoSize = true;
            this.lblJabatan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblJabatan.Location = new System.Drawing.Point(511, 0);
            this.lblJabatan.Name = "lblJabatan";
            this.lblJabatan.Size = new System.Drawing.Size(60, 20);
            this.lblJabatan.TabIndex = 3;
            this.lblJabatan.Text = "Jabatan";
            // 
            // lblKontak
            // 
            this.lblKontak.AutoSize = true;
            this.lblKontak.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblKontak.Location = new System.Drawing.Point(666, 0);
            this.lblKontak.Name = "lblKontak";
            this.lblKontak.Size = new System.Drawing.Size(106, 20);
            this.lblKontak.TabIndex = 4;
            this.lblKontak.Text = "No. HP / Email";
            // 
            // txtNik
            // 
            this.txtNik.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNik.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNik.Location = new System.Drawing.Point(3, 27);
            this.txtNik.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtNik.Name = "txtNik";
            this.txtNik.ReadOnly = true;
            this.txtNik.Size = new System.Drawing.Size(110, 29);
            this.txtNik.TabIndex = 5;
            // 
            // txtNama
            // 
            this.txtNama.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNama.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNama.Location = new System.Drawing.Point(126, 27);
            this.txtNama.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtNama.MaxLength = 100;
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(208, 29);
            this.txtNama.TabIndex = 6;
            // 
            // cmbDivisi
            // 
            this.cmbDivisi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDivisi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDivisi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbDivisi.FormattingEnabled = true;
            this.cmbDivisi.Items.AddRange(new object[] {
            "Software Dev",
            "UI/UX Design",
            "IT Support",
            "Finance",
            "HRD",
            "Marketing"});
            this.cmbDivisi.Location = new System.Drawing.Point(347, 27);
            this.cmbDivisi.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.cmbDivisi.Name = "cmbDivisi";
            this.cmbDivisi.Size = new System.Drawing.Size(151, 29);
            this.cmbDivisi.TabIndex = 7;
            // 
            // txtJabatan
            // 
            this.txtJabatan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJabatan.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtJabatan.Location = new System.Drawing.Point(511, 27);
            this.txtJabatan.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtJabatan.MaxLength = 100;
            this.txtJabatan.Name = "txtJabatan";
            this.txtJabatan.Size = new System.Drawing.Size(142, 29);
            this.txtJabatan.TabIndex = 8;
            // 
            // txtKontak
            // 
            this.txtKontak.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKontak.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtKontak.Location = new System.Drawing.Point(666, 27);
            this.txtKontak.MaxLength = 50;
            this.txtKontak.Name = "txtKontak";
            this.txtKontak.Size = new System.Drawing.Size(152, 29);
            this.txtKontak.TabIndex = 9;
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReset.Location = new System.Drawing.Point(461, 108);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(110, 38);
            this.btnReset.TabIndex = 4;
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
            this.btnHapus.Location = new System.Drawing.Point(342, 108);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(110, 38);
            this.btnHapus.TabIndex = 3;
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
            this.btnUbah.Location = new System.Drawing.Point(203, 108);
            this.btnUbah.Name = "btnUbah";
            this.btnUbah.Size = new System.Drawing.Size(130, 38);
            this.btnUbah.TabIndex = 2;
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
            this.btnTambah.Location = new System.Drawing.Point(19, 108);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(175, 38);
            this.btnTambah.TabIndex = 1;
            this.btnTambah.Text = "+ Tambah Karyawan";
            this.btnTambah.UseVisualStyleBackColor = false;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // lblCari
            // 
            this.lblCari.AutoSize = true;
            this.lblCari.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCari.Location = new System.Drawing.Point(20, 203);
            this.lblCari.Name = "lblCari";
            this.lblCari.Size = new System.Drawing.Size(113, 21);
            this.lblCari.TabIndex = 1;
            this.lblCari.Text = "Cari Karyawan:";
            // 
            // txtCari
            // 
            this.txtCari.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCari.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtCari.Location = new System.Drawing.Point(138, 199);
            this.txtCari.Name = "txtCari";
            this.txtCari.Size = new System.Drawing.Size(320, 29);
            this.txtCari.TabIndex = 2;
            this.txtCari.TextChanged += new System.EventHandler(this.txtCari_TextChanged);
            // 
            // dgvKaryawan
            // 
            this.dgvKaryawan.AllowUserToAddRows = false;
            this.dgvKaryawan.AllowUserToDeleteRows = false;
            this.dgvKaryawan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvKaryawan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvKaryawan.BackgroundColor = System.Drawing.Color.White;
            this.dgvKaryawan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKaryawan.Location = new System.Drawing.Point(20, 238);
            this.dgvKaryawan.Name = "dgvKaryawan";
            this.dgvKaryawan.ReadOnly = true;
            this.dgvKaryawan.RowHeadersVisible = false;
            this.dgvKaryawan.RowHeadersWidth = 51;
            this.dgvKaryawan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvKaryawan.Size = new System.Drawing.Size(853, 327);
            this.dgvKaryawan.TabIndex = 3;
            this.dgvKaryawan.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKaryawan_CellClick);
            // 
            // UC_Karyawan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.dgvKaryawan);
            this.Controls.Add(this.txtCari);
            this.Controls.Add(this.lblCari);
            this.Controls.Add(this.grpForm);
            this.Name = "UC_Karyawan";
            this.Size = new System.Drawing.Size(893, 585);
            this.grpForm.ResumeLayout(false);
            this.tlpInputs.ResumeLayout(false);
            this.tlpInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKaryawan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpForm;
        private System.Windows.Forms.TableLayoutPanel tlpInputs;
        private System.Windows.Forms.Label lblNik;
        private System.Windows.Forms.TextBox txtNik;
        private System.Windows.Forms.Label lblNama;
        private System.Windows.Forms.TextBox txtNama;
        private System.Windows.Forms.Label lblDivisi;
        private System.Windows.Forms.ComboBox cmbDivisi;
        private System.Windows.Forms.Label lblJabatan;
        private System.Windows.Forms.TextBox txtJabatan;
        private System.Windows.Forms.Label lblKontak;
        private System.Windows.Forms.TextBox txtKontak;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.Button btnUbah;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblCari;
        private System.Windows.Forms.TextBox txtCari;
        private System.Windows.Forms.DataGridView dgvKaryawan;
    }
}
