namespace ITAssetManagement.Views
{
    partial class UC_Dashboard
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
            this.tlpCards = new System.Windows.Forms.TableLayoutPanel();
            this.cardTotal = new System.Windows.Forms.Panel();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblTotalTitle = new System.Windows.Forms.Label();
            this.cardTersedia = new System.Windows.Forms.Panel();
            this.lblTersediaValue = new System.Windows.Forms.Label();
            this.lblTersediaTitle = new System.Windows.Forms.Label();
            this.cardDipinjam = new System.Windows.Forms.Panel();
            this.lblDipinjamValue = new System.Windows.Forms.Label();
            this.lblDipinjamTitle = new System.Windows.Forms.Label();
            this.cardPerbaikan = new System.Windows.Forms.Panel();
            this.lblPerbaikanValue = new System.Windows.Forms.Label();
            this.lblPerbaikanTitle = new System.Windows.Forms.Label();
            this.tlpTables = new System.Windows.Forms.TableLayoutPanel();
            this.pnlAlokasi = new System.Windows.Forms.Panel();
            this.dgvAlokasiAktif = new System.Windows.Forms.DataGridView();
            this.lblSubAlokasi = new System.Windows.Forms.Label();
            this.pnlRiwayat = new System.Windows.Forms.Panel();
            this.dgvRiwayatTerbaru = new System.Windows.Forms.DataGridView();
            this.lblSubRiwayat = new System.Windows.Forms.Label();
            this.tlpCards.SuspendLayout();
            this.cardTotal.SuspendLayout();
            this.cardTersedia.SuspendLayout();
            this.cardDipinjam.SuspendLayout();
            this.cardPerbaikan.SuspendLayout();
            this.tlpTables.SuspendLayout();
            this.pnlAlokasi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlokasiAktif)).BeginInit();
            this.pnlRiwayat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRiwayatTerbaru)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpCards
            // 
            this.tlpCards.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpCards.ColumnCount = 4;
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpCards.Controls.Add(this.cardTotal, 0, 0);
            this.tlpCards.Controls.Add(this.cardTersedia, 1, 0);
            this.tlpCards.Controls.Add(this.cardDipinjam, 2, 0);
            this.tlpCards.Controls.Add(this.cardPerbaikan, 3, 0);
            this.tlpCards.Location = new System.Drawing.Point(16, 16);
            this.tlpCards.Name = "tlpCards";
            this.tlpCards.RowCount = 1;
            this.tlpCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCards.Size = new System.Drawing.Size(861, 100);
            this.tlpCards.TabIndex = 0;
            // 
            // cardTotal
            // 
            this.cardTotal.BackColor = System.Drawing.Color.FromArgb(79, 70, 229);
            this.cardTotal.Controls.Add(this.lblTotalValue);
            this.cardTotal.Controls.Add(this.lblTotalTitle);
            this.cardTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTotal.Location = new System.Drawing.Point(4, 4);
            this.cardTotal.Margin = new System.Windows.Forms.Padding(4);
            this.cardTotal.Name = "cardTotal";
            this.cardTotal.Size = new System.Drawing.Size(207, 92);
            this.cardTotal.TabIndex = 0;
            // 
            // lblTotalTitle
            // 
            this.lblTotalTitle.AutoSize = true;
            this.lblTotalTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTotalTitle.ForeColor = System.Drawing.Color.White;
            this.lblTotalTitle.Location = new System.Drawing.Point(14, 12);
            this.lblTotalTitle.Name = "lblTotalTitle";
            this.lblTotalTitle.Size = new System.Drawing.Size(81, 21);
            this.lblTotalTitle.TabIndex = 0;
            this.lblTotalTitle.Text = "Total Aset";
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTotalValue.ForeColor = System.Drawing.Color.White;
            this.lblTotalValue.Location = new System.Drawing.Point(14, 38);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(40, 45);
            this.lblTotalValue.TabIndex = 1;
            this.lblTotalValue.Text = "0";
            // 
            // cardTersedia
            // 
            this.cardTersedia.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cardTersedia.Controls.Add(this.lblTersediaValue);
            this.cardTersedia.Controls.Add(this.lblTersediaTitle);
            this.cardTersedia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTersedia.Location = new System.Drawing.Point(219, 4);
            this.cardTersedia.Margin = new System.Windows.Forms.Padding(4);
            this.cardTersedia.Name = "cardTersedia";
            this.cardTersedia.Size = new System.Drawing.Size(207, 92);
            this.cardTersedia.TabIndex = 1;
            // 
            // lblTersediaTitle
            // 
            this.lblTersediaTitle.AutoSize = true;
            this.lblTersediaTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTersediaTitle.ForeColor = System.Drawing.Color.White;
            this.lblTersediaTitle.Location = new System.Drawing.Point(14, 12);
            this.lblTersediaTitle.Name = "lblTersediaTitle";
            this.lblTersediaTitle.Size = new System.Drawing.Size(105, 21);
            this.lblTersediaTitle.TabIndex = 0;
            this.lblTersediaTitle.Text = "Aset Tersedia";
            // 
            // lblTersediaValue
            // 
            this.lblTersediaValue.AutoSize = true;
            this.lblTersediaValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTersediaValue.ForeColor = System.Drawing.Color.White;
            this.lblTersediaValue.Location = new System.Drawing.Point(14, 38);
            this.lblTersediaValue.Name = "lblTersediaValue";
            this.lblTersediaValue.Size = new System.Drawing.Size(40, 45);
            this.lblTersediaValue.TabIndex = 1;
            this.lblTersediaValue.Text = "0";
            // 
            // cardDipinjam
            // 
            this.cardDipinjam.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.cardDipinjam.Controls.Add(this.lblDipinjamValue);
            this.cardDipinjam.Controls.Add(this.lblDipinjamTitle);
            this.cardDipinjam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardDipinjam.Location = new System.Drawing.Point(434, 4);
            this.cardDipinjam.Margin = new System.Windows.Forms.Padding(4);
            this.cardDipinjam.Name = "cardDipinjam";
            this.cardDipinjam.Size = new System.Drawing.Size(207, 92);
            this.cardDipinjam.TabIndex = 2;
            // 
            // lblDipinjamTitle
            // 
            this.lblDipinjamTitle.AutoSize = true;
            this.lblDipinjamTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDipinjamTitle.ForeColor = System.Drawing.Color.White;
            this.lblDipinjamTitle.Location = new System.Drawing.Point(14, 12);
            this.lblDipinjamTitle.Name = "lblDipinjamTitle";
            this.lblDipinjamTitle.Size = new System.Drawing.Size(134, 21);
            this.lblDipinjamTitle.TabIndex = 0;
            this.lblDipinjamTitle.Text = "Sedang Dipinjam";
            // 
            // lblDipinjamValue
            // 
            this.lblDipinjamValue.AutoSize = true;
            this.lblDipinjamValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblDipinjamValue.ForeColor = System.Drawing.Color.White;
            this.lblDipinjamValue.Location = new System.Drawing.Point(14, 38);
            this.lblDipinjamValue.Name = "lblDipinjamValue";
            this.lblDipinjamValue.Size = new System.Drawing.Size(40, 45);
            this.lblDipinjamValue.TabIndex = 1;
            this.lblDipinjamValue.Text = "0";
            // 
            // cardPerbaikan
            // 
            this.cardPerbaikan.BackColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.cardPerbaikan.Controls.Add(this.lblPerbaikanValue);
            this.cardPerbaikan.Controls.Add(this.lblPerbaikanTitle);
            this.cardPerbaikan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardPerbaikan.Location = new System.Drawing.Point(649, 4);
            this.cardPerbaikan.Margin = new System.Windows.Forms.Padding(4);
            this.cardPerbaikan.Name = "cardPerbaikan";
            this.cardPerbaikan.Size = new System.Drawing.Size(208, 92);
            this.cardPerbaikan.TabIndex = 3;
            // 
            // lblPerbaikanTitle
            // 
            this.lblPerbaikanTitle.AutoSize = true;
            this.lblPerbaikanTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblPerbaikanTitle.ForeColor = System.Drawing.Color.White;
            this.lblPerbaikanTitle.Location = new System.Drawing.Point(14, 12);
            this.lblPerbaikanTitle.Name = "lblPerbaikanTitle";
            this.lblPerbaikanTitle.Size = new System.Drawing.Size(142, 21);
            this.lblPerbaikanTitle.TabIndex = 0;
            this.lblPerbaikanTitle.Text = "Rusak / Perbaikan";
            // 
            // lblPerbaikanValue
            // 
            this.lblPerbaikanValue.AutoSize = true;
            this.lblPerbaikanValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblPerbaikanValue.ForeColor = System.Drawing.Color.White;
            this.lblPerbaikanValue.Location = new System.Drawing.Point(14, 38);
            this.lblPerbaikanValue.Name = "lblPerbaikanValue";
            this.lblPerbaikanValue.Size = new System.Drawing.Size(40, 45);
            this.lblPerbaikanValue.TabIndex = 1;
            this.lblPerbaikanValue.Text = "0";
            // 
            // tlpTables
            // 
            this.tlpTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpTables.ColumnCount = 1;
            this.tlpTables.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTables.Controls.Add(this.pnlAlokasi, 0, 0);
            this.tlpTables.Controls.Add(this.pnlRiwayat, 0, 1);
            this.tlpTables.Location = new System.Drawing.Point(16, 126);
            this.tlpTables.Name = "tlpTables";
            this.tlpTables.RowCount = 2;
            this.tlpTables.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTables.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTables.Size = new System.Drawing.Size(861, 443);
            this.tlpTables.TabIndex = 1;
            // 
            // pnlAlokasi
            // 
            this.pnlAlokasi.Controls.Add(this.dgvAlokasiAktif);
            this.pnlAlokasi.Controls.Add(this.lblSubAlokasi);
            this.pnlAlokasi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAlokasi.Location = new System.Drawing.Point(4, 4);
            this.pnlAlokasi.Margin = new System.Windows.Forms.Padding(4);
            this.pnlAlokasi.Name = "pnlAlokasi";
            this.pnlAlokasi.Size = new System.Drawing.Size(853, 213);
            this.pnlAlokasi.TabIndex = 0;
            // 
            // lblSubAlokasi
            // 
            this.lblSubAlokasi.AutoSize = true;
            this.lblSubAlokasi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSubAlokasi.Location = new System.Drawing.Point(0, 4);
            this.lblSubAlokasi.Name = "lblSubAlokasi";
            this.lblSubAlokasi.Size = new System.Drawing.Size(256, 23);
            this.lblSubAlokasi.TabIndex = 0;
            this.lblSubAlokasi.Text = "Aset yang Sedang Dipinjam";
            // 
            // dgvAlokasiAktif
            // 
            this.dgvAlokasiAktif.AllowUserToAddRows = false;
            this.dgvAlokasiAktif.AllowUserToDeleteRows = false;
            this.dgvAlokasiAktif.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAlokasiAktif.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAlokasiAktif.BackgroundColor = System.Drawing.Color.White;
            this.dgvAlokasiAktif.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAlokasiAktif.Location = new System.Drawing.Point(0, 32);
            this.dgvAlokasiAktif.Name = "dgvAlokasiAktif";
            this.dgvAlokasiAktif.ReadOnly = true;
            this.dgvAlokasiAktif.RowHeadersVisible = false;
            this.dgvAlokasiAktif.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAlokasiAktif.Size = new System.Drawing.Size(853, 178);
            this.dgvAlokasiAktif.TabIndex = 1;
            // 
            // pnlRiwayat
            // 
            this.pnlRiwayat.Controls.Add(this.dgvRiwayatTerbaru);
            this.pnlRiwayat.Controls.Add(this.lblSubRiwayat);
            this.pnlRiwayat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRiwayat.Location = new System.Drawing.Point(4, 225);
            this.pnlRiwayat.Margin = new System.Windows.Forms.Padding(4);
            this.pnlRiwayat.Name = "pnlRiwayat";
            this.pnlRiwayat.Size = new System.Drawing.Size(853, 214);
            this.pnlRiwayat.TabIndex = 1;
            // 
            // lblSubRiwayat
            // 
            this.lblSubRiwayat.AutoSize = true;
            this.lblSubRiwayat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSubRiwayat.Location = new System.Drawing.Point(0, 4);
            this.lblSubRiwayat.Name = "lblSubRiwayat";
            this.lblSubRiwayat.Size = new System.Drawing.Size(225, 23);
            this.lblSubRiwayat.TabIndex = 0;
            this.lblSubRiwayat.Text = "Aktivitas Riwayat Terakhir";
            // 
            // dgvRiwayatTerbaru
            // 
            this.dgvRiwayatTerbaru.AllowUserToAddRows = false;
            this.dgvRiwayatTerbaru.AllowUserToDeleteRows = false;
            this.dgvRiwayatTerbaru.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRiwayatTerbaru.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRiwayatTerbaru.BackgroundColor = System.Drawing.Color.White;
            this.dgvRiwayatTerbaru.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRiwayatTerbaru.Location = new System.Drawing.Point(0, 32);
            this.dgvRiwayatTerbaru.Name = "dgvRiwayatTerbaru";
            this.dgvRiwayatTerbaru.ReadOnly = true;
            this.dgvRiwayatTerbaru.RowHeadersVisible = false;
            this.dgvRiwayatTerbaru.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRiwayatTerbaru.Size = new System.Drawing.Size(853, 179);
            this.dgvRiwayatTerbaru.TabIndex = 1;
            // 
            // UC_Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Controls.Add(this.tlpTables);
            this.Controls.Add(this.tlpCards);
            this.Name = "UC_Dashboard";
            this.Size = new System.Drawing.Size(893, 585);
            this.tlpCards.ResumeLayout(false);
            this.cardTotal.ResumeLayout(false);
            this.cardTotal.PerformLayout();
            this.cardTersedia.ResumeLayout(false);
            this.cardTersedia.PerformLayout();
            this.cardDipinjam.ResumeLayout(false);
            this.cardDipinjam.PerformLayout();
            this.cardPerbaikan.ResumeLayout(false);
            this.cardPerbaikan.PerformLayout();
            this.tlpTables.ResumeLayout(false);
            this.pnlAlokasi.ResumeLayout(false);
            this.pnlAlokasi.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlokasiAktif)).EndInit();
            this.pnlRiwayat.ResumeLayout(false);
            this.pnlRiwayat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRiwayatTerbaru)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpCards;
        private System.Windows.Forms.Panel cardTotal;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label lblTotalTitle;
        private System.Windows.Forms.Panel cardTersedia;
        private System.Windows.Forms.Label lblTersediaValue;
        private System.Windows.Forms.Label lblTersediaTitle;
        private System.Windows.Forms.Panel cardDipinjam;
        private System.Windows.Forms.Label lblDipinjamValue;
        private System.Windows.Forms.Label lblDipinjamTitle;
        private System.Windows.Forms.Panel cardPerbaikan;
        private System.Windows.Forms.Label lblPerbaikanValue;
        private System.Windows.Forms.Label lblPerbaikanTitle;
        private System.Windows.Forms.TableLayoutPanel tlpTables;
        private System.Windows.Forms.Panel pnlAlokasi;
        private System.Windows.Forms.Label lblSubAlokasi;
        private System.Windows.Forms.DataGridView dgvAlokasiAktif;
        private System.Windows.Forms.Panel pnlRiwayat;
        private System.Windows.Forms.Label lblSubRiwayat;
        private System.Windows.Forms.DataGridView dgvRiwayatTerbaru;
    }
}
