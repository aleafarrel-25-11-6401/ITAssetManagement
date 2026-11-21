namespace ITAssetManagement.Views
{
    partial class UC_Profil
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
            this.grpInfo = new System.Windows.Forms.GroupBox();
            this.tlpInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lblUsernameAktif = new System.Windows.Forms.Label();
            this.lblNamaAktif = new System.Windows.Forms.Label();
            this.lblRoleAktif = new System.Windows.Forms.Label();
            this.txtUsernameAktif = new System.Windows.Forms.TextBox();
            this.txtNamaAktif = new System.Windows.Forms.TextBox();
            this.txtRoleAktif = new System.Windows.Forms.TextBox();
            this.grpUbah = new System.Windows.Forms.GroupBox();
            this.tlpUbah = new System.Windows.Forms.TableLayoutPanel();
            this.lblUsernameBaru = new System.Windows.Forms.Label();
            this.lblNamaBaru = new System.Windows.Forms.Label();
            this.lblPasswordLama = new System.Windows.Forms.Label();
            this.txtUsernameBaru = new System.Windows.Forms.TextBox();
            this.txtNamaBaru = new System.Windows.Forms.TextBox();
            this.txtPasswordLama = new System.Windows.Forms.TextBox();
            this.lblPasswordBaru = new System.Windows.Forms.Label();
            this.lblKonfirmasiPassword = new System.Windows.Forms.Label();
            this.txtPasswordBaru = new System.Windows.Forms.TextBox();
            this.txtKonfirmasiPassword = new System.Windows.Forms.TextBox();
            this.lblPetunjuk = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSimpan = new System.Windows.Forms.Button();
            this.grpInfo.SuspendLayout();
            this.tlpInfo.SuspendLayout();
            this.grpUbah.SuspendLayout();
            this.tlpUbah.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpInfo
            // 
            this.grpInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInfo.BackColor = System.Drawing.Color.White;
            this.grpInfo.Controls.Add(this.tlpInfo);
            this.grpInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpInfo.Location = new System.Drawing.Point(20, 15);
            this.grpInfo.Name = "grpInfo";
            this.grpInfo.Size = new System.Drawing.Size(853, 110);
            this.grpInfo.TabIndex = 0;
            this.grpInfo.TabStop = false;
            this.grpInfo.Text = "Informasi Akun Saat Ini";
            // 
            // tlpInfo
            // 
            this.tlpInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpInfo.ColumnCount = 3;
            this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.tlpInfo.Controls.Add(this.lblUsernameAktif, 0, 0);
            this.tlpInfo.Controls.Add(this.lblNamaAktif, 1, 0);
            this.tlpInfo.Controls.Add(this.lblRoleAktif, 2, 0);
            this.tlpInfo.Controls.Add(this.txtUsernameAktif, 0, 1);
            this.tlpInfo.Controls.Add(this.txtNamaAktif, 1, 1);
            this.tlpInfo.Controls.Add(this.txtRoleAktif, 2, 1);
            this.tlpInfo.Location = new System.Drawing.Point(16, 28);
            this.tlpInfo.Name = "tlpInfo";
            this.tlpInfo.RowCount = 2;
            this.tlpInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpInfo.Size = new System.Drawing.Size(821, 62);
            this.tlpInfo.TabIndex = 0;
            // 
            // lblUsernameAktif
            // 
            this.lblUsernameAktif.AutoSize = true;
            this.lblUsernameAktif.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblUsernameAktif.Location = new System.Drawing.Point(3, 0);
            this.lblUsernameAktif.Name = "lblUsernameAktif";
            this.lblUsernameAktif.Size = new System.Drawing.Size(116, 20);
            this.lblUsernameAktif.TabIndex = 0;
            this.lblUsernameAktif.Text = "Username Login";
            // 
            // lblNamaAktif
            // 
            this.lblNamaAktif.AutoSize = true;
            this.lblNamaAktif.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNamaAktif.Location = new System.Drawing.Point(265, 0);
            this.lblNamaAktif.Name = "lblNamaAktif";
            this.lblNamaAktif.Size = new System.Drawing.Size(176, 20);
            this.lblNamaAktif.TabIndex = 1;
            this.lblNamaAktif.Text = "Nama Pengguna (Tampil)";
            // 
            // lblRoleAktif
            // 
            this.lblRoleAktif.AutoSize = true;
            this.lblRoleAktif.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRoleAktif.Location = new System.Drawing.Point(593, 0);
            this.lblRoleAktif.Name = "lblRoleAktif";
            this.lblRoleAktif.Size = new System.Drawing.Size(120, 20);
            this.lblRoleAktif.TabIndex = 2;
            this.lblRoleAktif.Text = "Role / Hak Akses";
            // 
            // txtUsernameAktif
            // 
            this.txtUsernameAktif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUsernameAktif.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtUsernameAktif.Location = new System.Drawing.Point(3, 27);
            this.txtUsernameAktif.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.txtUsernameAktif.Name = "txtUsernameAktif";
            this.txtUsernameAktif.ReadOnly = true;
            this.txtUsernameAktif.Size = new System.Drawing.Size(247, 29);
            this.txtUsernameAktif.TabIndex = 3;
            // 
            // txtNamaAktif
            // 
            this.txtNamaAktif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNamaAktif.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNamaAktif.Location = new System.Drawing.Point(265, 27);
            this.txtNamaAktif.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.txtNamaAktif.Name = "txtNamaAktif";
            this.txtNamaAktif.ReadOnly = true;
            this.txtNamaAktif.Size = new System.Drawing.Size(313, 29);
            this.txtNamaAktif.TabIndex = 4;
            // 
            // txtRoleAktif
            // 
            this.txtRoleAktif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRoleAktif.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtRoleAktif.Location = new System.Drawing.Point(593, 27);
            this.txtRoleAktif.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.txtRoleAktif.Name = "txtRoleAktif";
            this.txtRoleAktif.ReadOnly = true;
            this.txtRoleAktif.Size = new System.Drawing.Size(216, 29);
            this.txtRoleAktif.TabIndex = 5;
            // 
            // grpUbah
            // 
            this.grpUbah.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpUbah.BackColor = System.Drawing.Color.White;
            this.grpUbah.Controls.Add(this.tlpUbah);
            this.grpUbah.Controls.Add(this.lblPetunjuk);
            this.grpUbah.Controls.Add(this.btnReset);
            this.grpUbah.Controls.Add(this.btnSimpan);
            this.grpUbah.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpUbah.Location = new System.Drawing.Point(20, 140);
            this.grpUbah.Name = "grpUbah";
            this.grpUbah.Size = new System.Drawing.Size(853, 255);
            this.grpUbah.TabIndex = 1;
            this.grpUbah.TabStop = false;
            this.grpUbah.Text = "Form Ubah Username, Nama Pengguna & Password";
            // 
            // tlpUbah
            // 
            this.tlpUbah.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpUbah.ColumnCount = 3;
            this.tlpUbah.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.tlpUbah.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.tlpUbah.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.tlpUbah.Controls.Add(this.lblUsernameBaru, 0, 0);
            this.tlpUbah.Controls.Add(this.lblNamaBaru, 1, 0);
            this.tlpUbah.Controls.Add(this.lblPasswordLama, 2, 0);
            this.tlpUbah.Controls.Add(this.txtUsernameBaru, 0, 1);
            this.tlpUbah.Controls.Add(this.txtNamaBaru, 1, 1);
            this.tlpUbah.Controls.Add(this.txtPasswordLama, 2, 1);
            this.tlpUbah.Controls.Add(this.lblPasswordBaru, 0, 2);
            this.tlpUbah.Controls.Add(this.lblKonfirmasiPassword, 1, 2);
            this.tlpUbah.Controls.Add(this.txtPasswordBaru, 0, 3);
            this.tlpUbah.Controls.Add(this.txtKonfirmasiPassword, 1, 3);
            this.tlpUbah.Location = new System.Drawing.Point(16, 28);
            this.tlpUbah.Name = "tlpUbah";
            this.tlpUbah.RowCount = 4;
            this.tlpUbah.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpUbah.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpUbah.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpUbah.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpUbah.Size = new System.Drawing.Size(821, 130);
            this.tlpUbah.TabIndex = 0;
            // 
            // lblUsernameBaru
            // 
            this.lblUsernameBaru.AutoSize = true;
            this.lblUsernameBaru.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblUsernameBaru.Location = new System.Drawing.Point(3, 0);
            this.lblUsernameBaru.Name = "lblUsernameBaru";
            this.lblUsernameBaru.Size = new System.Drawing.Size(220, 20);
            this.lblUsernameBaru.TabIndex = 0;
            this.lblUsernameBaru.Text = "Username Baru (min. 3 karakter)";
            // 
            // lblNamaBaru
            // 
            this.lblNamaBaru.AutoSize = true;
            this.lblNamaBaru.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNamaBaru.Location = new System.Drawing.Point(265, 0);
            this.lblNamaBaru.Name = "lblNamaBaru";
            this.lblNamaBaru.Size = new System.Drawing.Size(220, 20);
            this.lblNamaBaru.TabIndex = 1;
            this.lblNamaBaru.Text = "Nama Pengguna (Nama Tampil)";
            // 
            // lblPasswordLama
            // 
            this.lblPasswordLama.AutoSize = true;
            this.lblPasswordLama.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPasswordLama.Location = new System.Drawing.Point(560, 0);
            this.lblPasswordLama.Name = "lblPasswordLama";
            this.lblPasswordLama.Size = new System.Drawing.Size(175, 20);
            this.lblPasswordLama.TabIndex = 2;
            this.lblPasswordLama.Text = "Password Saat Ini (Wajib)";
            // 
            // txtUsernameBaru
            // 
            this.txtUsernameBaru.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUsernameBaru.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtUsernameBaru.Location = new System.Drawing.Point(3, 27);
            this.txtUsernameBaru.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.txtUsernameBaru.MaxLength = 50;
            this.txtUsernameBaru.Name = "txtUsernameBaru";
            this.txtUsernameBaru.Size = new System.Drawing.Size(247, 29);
            this.txtUsernameBaru.TabIndex = 3;
            // 
            // txtNamaBaru
            // 
            this.txtNamaBaru.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNamaBaru.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNamaBaru.Location = new System.Drawing.Point(265, 27);
            this.txtNamaBaru.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.txtNamaBaru.MaxLength = 100;
            this.txtNamaBaru.Name = "txtNamaBaru";
            this.txtNamaBaru.Size = new System.Drawing.Size(280, 29);
            this.txtNamaBaru.TabIndex = 4;
            // 
            // txtPasswordLama
            // 
            this.txtPasswordLama.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPasswordLama.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtPasswordLama.Location = new System.Drawing.Point(560, 27);
            this.txtPasswordLama.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.txtPasswordLama.Name = "txtPasswordLama";
            this.txtPasswordLama.Size = new System.Drawing.Size(249, 29);
            this.txtPasswordLama.TabIndex = 5;
            this.txtPasswordLama.UseSystemPasswordChar = true;
            // 
            // lblPasswordBaru
            // 
            this.lblPasswordBaru.AutoSize = true;
            this.lblPasswordBaru.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPasswordBaru.Location = new System.Drawing.Point(3, 64);
            this.lblPasswordBaru.Name = "lblPasswordBaru";
            this.lblPasswordBaru.Size = new System.Drawing.Size(177, 20);
            this.lblPasswordBaru.TabIndex = 6;
            this.lblPasswordBaru.Text = "Password Baru (Opsional)";
            // 
            // lblKonfirmasiPassword
            // 
            this.lblKonfirmasiPassword.AutoSize = true;
            this.lblKonfirmasiPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblKonfirmasiPassword.Location = new System.Drawing.Point(265, 64);
            this.lblKonfirmasiPassword.Name = "lblKonfirmasiPassword";
            this.lblKonfirmasiPassword.Size = new System.Drawing.Size(179, 20);
            this.lblKonfirmasiPassword.TabIndex = 7;
            this.lblKonfirmasiPassword.Text = "Konfirmasi Password Baru";
            // 
            // txtPasswordBaru
            // 
            this.txtPasswordBaru.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPasswordBaru.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtPasswordBaru.Location = new System.Drawing.Point(3, 91);
            this.txtPasswordBaru.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.txtPasswordBaru.Name = "txtPasswordBaru";
            this.txtPasswordBaru.Size = new System.Drawing.Size(247, 29);
            this.txtPasswordBaru.TabIndex = 8;
            this.txtPasswordBaru.UseSystemPasswordChar = true;
            // 
            // txtKonfirmasiPassword
            // 
            this.txtKonfirmasiPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKonfirmasiPassword.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtKonfirmasiPassword.Location = new System.Drawing.Point(265, 91);
            this.txtKonfirmasiPassword.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.txtKonfirmasiPassword.Name = "txtKonfirmasiPassword";
            this.txtKonfirmasiPassword.Size = new System.Drawing.Size(280, 29);
            this.txtKonfirmasiPassword.TabIndex = 9;
            this.txtKonfirmasiPassword.UseSystemPasswordChar = true;
            // 
            // lblPetunjuk
            // 
            this.lblPetunjuk.AutoSize = true;
            this.lblPetunjuk.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPetunjuk.ForeColor = System.Drawing.Color.DimGray;
            this.lblPetunjuk.Location = new System.Drawing.Point(19, 165);
            this.lblPetunjuk.Name = "lblPetunjuk";
            this.lblPetunjuk.Size = new System.Drawing.Size(661, 20);
            this.lblPetunjuk.TabIndex = 1;
            this.lblPetunjuk.Text = "* Anda dapat mengubah username (case-sensitive), nama pengguna, dan/atau password" +
    " sekaligus.";
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReset.Location = new System.Drawing.Point(206, 196);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(120, 40);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "Reset Form";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSimpan
            // 
            this.btnSimpan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.btnSimpan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimpan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSimpan.ForeColor = System.Drawing.Color.White;
            this.btnSimpan.Location = new System.Drawing.Point(19, 196);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(175, 40);
            this.btnSimpan.TabIndex = 2;
            this.btnSimpan.Text = "Simpan Perubahan";
            this.btnSimpan.UseVisualStyleBackColor = false;
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // UC_Profil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.grpUbah);
            this.Controls.Add(this.grpInfo);
            this.Name = "UC_Profil";
            this.Size = new System.Drawing.Size(893, 585);
            this.grpInfo.ResumeLayout(false);
            this.tlpInfo.ResumeLayout(false);
            this.tlpInfo.PerformLayout();
            this.grpUbah.ResumeLayout(false);
            this.grpUbah.PerformLayout();
            this.tlpUbah.ResumeLayout(false);
            this.tlpUbah.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpInfo;
        private System.Windows.Forms.TableLayoutPanel tlpInfo;
        private System.Windows.Forms.Label lblUsernameAktif;
        private System.Windows.Forms.Label lblNamaAktif;
        private System.Windows.Forms.Label lblRoleAktif;
        private System.Windows.Forms.TextBox txtUsernameAktif;
        private System.Windows.Forms.TextBox txtNamaAktif;
        private System.Windows.Forms.TextBox txtRoleAktif;
        private System.Windows.Forms.GroupBox grpUbah;
        private System.Windows.Forms.TableLayoutPanel tlpUbah;
        private System.Windows.Forms.Label lblUsernameBaru;
        private System.Windows.Forms.Label lblNamaBaru;
        private System.Windows.Forms.Label lblPasswordLama;
        private System.Windows.Forms.TextBox txtUsernameBaru;
        private System.Windows.Forms.TextBox txtNamaBaru;
        private System.Windows.Forms.TextBox txtPasswordLama;
        private System.Windows.Forms.Label lblPasswordBaru;
        private System.Windows.Forms.Label lblKonfirmasiPassword;
        private System.Windows.Forms.TextBox txtPasswordBaru;
        private System.Windows.Forms.TextBox txtKonfirmasiPassword;
        private System.Windows.Forms.Label lblPetunjuk;
        private System.Windows.Forms.Button btnSimpan;
        private System.Windows.Forms.Button btnReset;
    }
}
