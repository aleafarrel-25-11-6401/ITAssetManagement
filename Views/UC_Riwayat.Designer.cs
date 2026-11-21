namespace ITAssetManagement.Views
{
    partial class UC_Riwayat
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
            this.lblFilter = new System.Windows.Forms.Label();
            this.cmbFilterAksi = new System.Windows.Forms.ComboBox();
            this.lblCari = new System.Windows.Forms.Label();
            this.txtCari = new System.Windows.Forms.TextBox();
            this.btnBersihkan = new System.Windows.Forms.Button();
            this.flpLegenda = new System.Windows.Forms.FlowLayoutPanel();
            this.lblInfoWarna = new System.Windows.Forms.Label();
            this.lblBadgePinjam = new System.Windows.Forms.Label();
            this.lblBadgeKembali = new System.Windows.Forms.Label();
            this.lblBadgePerbaikan = new System.Windows.Forms.Label();
            this.lblBadgeRusak = new System.Windows.Forms.Label();
            this.lblBadgeTambah = new System.Windows.Forms.Label();
            this.lblBadgeHapus = new System.Windows.Forms.Label();
            this.dgvRiwayat = new System.Windows.Forms.DataGridView();
            this.flpLegenda.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRiwayat)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblFilter.Location = new System.Drawing.Point(20, 22);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(83, 21);
            this.lblFilter.TabIndex = 0;
            this.lblFilter.Text = "Filter Aksi:";
            // 
            // cmbFilterAksi
            // 
            this.cmbFilterAksi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterAksi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbFilterAksi.FormattingEnabled = true;
            this.cmbFilterAksi.Items.AddRange(new object[] {
            "Semua",
            "Peminjaman",
            "Pengembalian",
            "Tambah Aset",
            "Hapus Aset"});
            this.cmbFilterAksi.Location = new System.Drawing.Point(108, 18);
            this.cmbFilterAksi.Name = "cmbFilterAksi";
            this.cmbFilterAksi.Size = new System.Drawing.Size(155, 29);
            this.cmbFilterAksi.TabIndex = 1;
            this.cmbFilterAksi.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);
            // 
            // lblCari
            // 
            this.lblCari.AutoSize = true;
            this.lblCari.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCari.Location = new System.Drawing.Point(280, 22);
            this.lblCari.Name = "lblCari";
            this.lblCari.Size = new System.Drawing.Size(102, 21);
            this.lblCari.TabIndex = 2;
            this.lblCari.Text = "Cari Riwayat:";
            // 
            // txtCari
            // 
            this.txtCari.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCari.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtCari.Location = new System.Drawing.Point(388, 18);
            this.txtCari.Name = "txtCari";
            this.txtCari.Size = new System.Drawing.Size(305, 29);
            this.txtCari.TabIndex = 3;
            this.txtCari.TextChanged += new System.EventHandler(this.Filter_Changed);
            // 
            // btnBersihkan
            // 
            this.btnBersihkan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBersihkan.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnBersihkan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBersihkan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBersihkan.ForeColor = System.Drawing.Color.White;
            this.btnBersihkan.Location = new System.Drawing.Point(705, 16);
            this.btnBersihkan.Name = "btnBersihkan";
            this.btnBersihkan.Size = new System.Drawing.Size(168, 34);
            this.btnBersihkan.TabIndex = 4;
            this.btnBersihkan.Text = "Bersihkan Riwayat";
            this.btnBersihkan.UseVisualStyleBackColor = false;
            this.btnBersihkan.Click += new System.EventHandler(this.btnBersihkan_Click);
            // 
            // flpLegenda
            // 
            this.flpLegenda.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpLegenda.Controls.Add(this.lblInfoWarna);
            this.flpLegenda.Controls.Add(this.lblBadgePinjam);
            this.flpLegenda.Controls.Add(this.lblBadgeKembali);
            this.flpLegenda.Controls.Add(this.lblBadgePerbaikan);
            this.flpLegenda.Controls.Add(this.lblBadgeRusak);
            this.flpLegenda.Controls.Add(this.lblBadgeTambah);
            this.flpLegenda.Controls.Add(this.lblBadgeHapus);
            this.flpLegenda.Location = new System.Drawing.Point(20, 54);
            this.flpLegenda.Margin = new System.Windows.Forms.Padding(0);
            this.flpLegenda.Name = "flpLegenda";
            this.flpLegenda.Size = new System.Drawing.Size(853, 28);
            this.flpLegenda.TabIndex = 5;
            // 
            // lblInfoWarna
            // 
            this.lblInfoWarna.AutoSize = true;
            this.lblInfoWarna.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblInfoWarna.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblInfoWarna.Margin = new System.Windows.Forms.Padding(0, 4, 8, 2);
            this.lblInfoWarna.Name = "lblInfoWarna";
            this.lblInfoWarna.Size = new System.Drawing.Size(66, 19);
            this.lblInfoWarna.TabIndex = 0;
            this.lblInfoWarna.Text = "Indikator:";
            // 
            // lblBadgePinjam
            // 
            this.lblBadgePinjam.AutoSize = true;
            this.lblBadgePinjam.BackColor = System.Drawing.Color.FromArgb(254, 243, 199);
            this.lblBadgePinjam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBadgePinjam.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblBadgePinjam.ForeColor = System.Drawing.Color.FromArgb(146, 64, 14);
            this.lblBadgePinjam.Margin = new System.Windows.Forms.Padding(0, 1, 6, 2);
            this.lblBadgePinjam.Name = "lblBadgePinjam";
            this.lblBadgePinjam.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.lblBadgePinjam.TabIndex = 1;
            this.lblBadgePinjam.Text = "Pinjam";
            // 
            // lblBadgeKembali
            // 
            this.lblBadgeKembali.AutoSize = true;
            this.lblBadgeKembali.BackColor = System.Drawing.Color.FromArgb(209, 250, 229);
            this.lblBadgeKembali.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBadgeKembali.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblBadgeKembali.ForeColor = System.Drawing.Color.FromArgb(6, 95, 70);
            this.lblBadgeKembali.Margin = new System.Windows.Forms.Padding(0, 1, 6, 2);
            this.lblBadgeKembali.Name = "lblBadgeKembali";
            this.lblBadgeKembali.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.lblBadgeKembali.TabIndex = 2;
            this.lblBadgeKembali.Text = "Kembali Baik";
            // 
            // lblBadgePerbaikan
            // 
            this.lblBadgePerbaikan.AutoSize = true;
            this.lblBadgePerbaikan.BackColor = System.Drawing.Color.FromArgb(255, 237, 213);
            this.lblBadgePerbaikan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBadgePerbaikan.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblBadgePerbaikan.ForeColor = System.Drawing.Color.FromArgb(154, 52, 18);
            this.lblBadgePerbaikan.Margin = new System.Windows.Forms.Padding(0, 1, 6, 2);
            this.lblBadgePerbaikan.Name = "lblBadgePerbaikan";
            this.lblBadgePerbaikan.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.lblBadgePerbaikan.TabIndex = 3;
            this.lblBadgePerbaikan.Text = "Kembali Perbaikan";
            // 
            // lblBadgeRusak
            // 
            this.lblBadgeRusak.AutoSize = true;
            this.lblBadgeRusak.BackColor = System.Drawing.Color.FromArgb(254, 226, 226);
            this.lblBadgeRusak.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBadgeRusak.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblBadgeRusak.ForeColor = System.Drawing.Color.FromArgb(153, 27, 27);
            this.lblBadgeRusak.Margin = new System.Windows.Forms.Padding(0, 1, 6, 2);
            this.lblBadgeRusak.Name = "lblBadgeRusak";
            this.lblBadgeRusak.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.lblBadgeRusak.TabIndex = 4;
            this.lblBadgeRusak.Text = "Kembali Rusak";
            // 
            // lblBadgeTambah
            // 
            this.lblBadgeTambah.AutoSize = true;
            this.lblBadgeTambah.BackColor = System.Drawing.Color.FromArgb(219, 234, 254);
            this.lblBadgeTambah.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBadgeTambah.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblBadgeTambah.ForeColor = System.Drawing.Color.FromArgb(30, 64, 175);
            this.lblBadgeTambah.Margin = new System.Windows.Forms.Padding(0, 1, 6, 2);
            this.lblBadgeTambah.Name = "lblBadgeTambah";
            this.lblBadgeTambah.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.lblBadgeTambah.TabIndex = 5;
            this.lblBadgeTambah.Text = "Tambah Aset";
            // 
            // lblBadgeHapus
            // 
            this.lblBadgeHapus.AutoSize = true;
            this.lblBadgeHapus.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblBadgeHapus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBadgeHapus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblBadgeHapus.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblBadgeHapus.Margin = new System.Windows.Forms.Padding(0, 1, 0, 2);
            this.lblBadgeHapus.Name = "lblBadgeHapus";
            this.lblBadgeHapus.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.lblBadgeHapus.TabIndex = 6;
            this.lblBadgeHapus.Text = "Hapus Aset";
            // 
            // dgvRiwayat
            // 
            this.dgvRiwayat.AllowUserToAddRows = false;
            this.dgvRiwayat.AllowUserToDeleteRows = false;
            this.dgvRiwayat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRiwayat.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRiwayat.BackgroundColor = System.Drawing.Color.White;
            this.dgvRiwayat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRiwayat.Location = new System.Drawing.Point(20, 88);
            this.dgvRiwayat.Name = "dgvRiwayat";
            this.dgvRiwayat.ReadOnly = true;
            this.dgvRiwayat.RowHeadersVisible = false;
            this.dgvRiwayat.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRiwayat.Size = new System.Drawing.Size(853, 477);
            this.dgvRiwayat.TabIndex = 9;
            // 
            // UC_Riwayat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Controls.Add(this.dgvRiwayat);
            this.Controls.Add(this.flpLegenda);
            this.Controls.Add(this.btnBersihkan);
            this.Controls.Add(this.txtCari);
            this.Controls.Add(this.lblCari);
            this.Controls.Add(this.cmbFilterAksi);
            this.Controls.Add(this.lblFilter);
            this.Name = "UC_Riwayat";
            this.Size = new System.Drawing.Size(893, 585);
            this.flpLegenda.ResumeLayout(false);
            this.flpLegenda.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRiwayat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cmbFilterAksi;
        private System.Windows.Forms.Label lblCari;
        private System.Windows.Forms.TextBox txtCari;
        private System.Windows.Forms.Button btnBersihkan;
        private System.Windows.Forms.FlowLayoutPanel flpLegenda;
        private System.Windows.Forms.Label lblInfoWarna;
        private System.Windows.Forms.Label lblBadgePinjam;
        private System.Windows.Forms.Label lblBadgeKembali;
        private System.Windows.Forms.Label lblBadgePerbaikan;
        private System.Windows.Forms.Label lblBadgeRusak;
        private System.Windows.Forms.Label lblBadgeTambah;
        private System.Windows.Forms.Label lblBadgeHapus;
        private System.Windows.Forms.DataGridView dgvRiwayat;
    }
}
