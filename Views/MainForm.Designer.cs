namespace ITAssetManagement.Views
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnProfil = new System.Windows.Forms.Button();
            this.btnRiwayat = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnAlokasi = new System.Windows.Forms.Button();
            this.btnKaryawan = new System.Windows.Forms.Button();
            this.btnAsset = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHalaman = new System.Windows.Forms.Label();
            this.lblProfil = new System.Windows.Forms.Label();
            this.pnlHalaman = new System.Windows.Forms.Panel();
            this.pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.Controls.Add(this.btnProfil);
            this.pnlSidebar.Controls.Add(this.btnRiwayat);
            this.pnlSidebar.Controls.Add(this.btnLogout);
            this.pnlSidebar.Controls.Add(this.btnAlokasi);
            this.pnlSidebar.Controls.Add(this.btnKaryawan);
            this.pnlSidebar.Controls.Add(this.btnAsset);
            this.pnlSidebar.Controls.Add(this.btnDashboard);
            this.pnlSidebar.Controls.Add(this.pictureBox1);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(225, 645);
            this.pnlSidebar.TabIndex = 0;
            // 
            // btnProfil
            // 
            this.btnProfil.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProfil.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnProfil.Location = new System.Drawing.Point(6, 366);
            this.btnProfil.Name = "btnProfil";
            this.btnProfil.Size = new System.Drawing.Size(213, 42);
            this.btnProfil.TabIndex = 5;
            this.btnProfil.Text = "Profil Akun";
            this.btnProfil.UseVisualStyleBackColor = true;
            this.btnProfil.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnRiwayat
            // 
            this.btnRiwayat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRiwayat.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRiwayat.Location = new System.Drawing.Point(6, 318);
            this.btnRiwayat.Name = "btnRiwayat";
            this.btnRiwayat.Size = new System.Drawing.Size(213, 42);
            this.btnRiwayat.TabIndex = 4;
            this.btnRiwayat.Text = "Riwayat";
            this.btnRiwayat.UseVisualStyleBackColor = true;
            this.btnRiwayat.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(6, 596);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(213, 42);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnAlokasi
            // 
            this.btnAlokasi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAlokasi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAlokasi.Location = new System.Drawing.Point(6, 270);
            this.btnAlokasi.Name = "btnAlokasi";
            this.btnAlokasi.Size = new System.Drawing.Size(213, 42);
            this.btnAlokasi.TabIndex = 3;
            this.btnAlokasi.Text = "Alokasi";
            this.btnAlokasi.UseVisualStyleBackColor = true;
            this.btnAlokasi.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnKaryawan
            // 
            this.btnKaryawan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKaryawan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnKaryawan.Location = new System.Drawing.Point(6, 222);
            this.btnKaryawan.Name = "btnKaryawan";
            this.btnKaryawan.Size = new System.Drawing.Size(213, 42);
            this.btnKaryawan.TabIndex = 2;
            this.btnKaryawan.Text = "Karyawan";
            this.btnKaryawan.UseVisualStyleBackColor = true;
            this.btnKaryawan.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnAsset
            // 
            this.btnAsset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAsset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAsset.Location = new System.Drawing.Point(6, 174);
            this.btnAsset.Name = "btnAsset";
            this.btnAsset.Size = new System.Drawing.Size(213, 42);
            this.btnAsset.TabIndex = 1;
            this.btnAsset.Text = "Manajemen Aset";
            this.btnAsset.UseVisualStyleBackColor = true;
            this.btnAsset.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnDashboard
            // 
            this.btnDashboard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDashboard.Location = new System.Drawing.Point(6, 126);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(213, 42);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.UseVisualStyleBackColor = true;
            this.btnDashboard.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(225, 120);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblHalaman);
            this.pnlHeader.Controls.Add(this.lblProfil);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(225, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(893, 60);
            this.pnlHeader.TabIndex = 1;
            // 
            // lblHalaman
            // 
            this.lblHalaman.AutoSize = true;
            this.lblHalaman.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblHalaman.Location = new System.Drawing.Point(20, 18);
            this.lblHalaman.Name = "lblHalaman";
            this.lblHalaman.Size = new System.Drawing.Size(109, 25);
            this.lblHalaman.TabIndex = 0;
            this.lblHalaman.Text = "Dashboard";
            // 
            // lblProfil
            // 
            this.lblProfil.AutoSize = true;
            this.lblProfil.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblProfil.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProfil.Location = new System.Drawing.Point(786, 0);
            this.lblProfil.Name = "lblProfil";
            this.lblProfil.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.lblProfil.Size = new System.Drawing.Size(107, 23);
            this.lblProfil.TabIndex = 1;
            this.lblProfil.Text = "Pengguna";
            this.lblProfil.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlHalaman
            // 
            this.pnlHalaman.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHalaman.Location = new System.Drawing.Point(225, 60);
            this.pnlHalaman.Name = "pnlHalaman";
            this.pnlHalaman.Size = new System.Drawing.Size(893, 585);
            this.pnlHalaman.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 645);
            this.Controls.Add(this.pnlHalaman);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(980, 620);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IT Asset Management";
            this.pnlSidebar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlHalaman;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnAlokasi;
        private System.Windows.Forms.Button btnKaryawan;
        private System.Windows.Forms.Button btnAsset;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblHalaman;
        private System.Windows.Forms.Label lblProfil;
        private System.Windows.Forms.Button btnRiwayat;
        private System.Windows.Forms.Button btnProfil;
    }
}
