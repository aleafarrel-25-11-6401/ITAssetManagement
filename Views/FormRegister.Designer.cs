namespace ITAssetManagement.Views
{
    partial class FormRegister
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
            this.lblJudul = new System.Windows.Forms.Label();
            this.lblInfoRole = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblNamaLengkap = new System.Windows.Forms.Label();
            this.txtNamaLengkap = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblKonfirmasi = new System.Windows.Forms.Label();
            this.txtKonfirmasi = new System.Windows.Forms.TextBox();
            this.btnDaftar = new System.Windows.Forms.Button();
            this.btnKembali = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblJudul
            // 
            this.lblJudul.AutoSize = true;
            this.lblJudul.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblJudul.Location = new System.Drawing.Point(260, 70);
            this.lblJudul.Name = "lblJudul";
            this.lblJudul.Size = new System.Drawing.Size(178, 28);
            this.lblJudul.TabIndex = 1;
            this.lblJudul.Text = "Daftar Akun Baru";
            // 
            // lblInfoRole
            // 
            this.lblInfoRole.AutoSize = true;
            this.lblInfoRole.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblInfoRole.ForeColor = System.Drawing.Color.DimGray;
            this.lblInfoRole.Location = new System.Drawing.Point(261, 98);
            this.lblInfoRole.Name = "lblInfoRole";
            this.lblInfoRole.Size = new System.Drawing.Size(287, 19);
            this.lblInfoRole.TabIndex = 2;
            this.lblInfoRole.Text = "Akun baru otomatis terdaftar sebagai Staff IT.";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblUsername.Location = new System.Drawing.Point(260, 124);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(227, 20);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Username Login (min. 3 karakter)";
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.txtUsername.Location = new System.Drawing.Point(260, 146);
            this.txtUsername.MaxLength = 50;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(274, 28);
            this.txtUsername.TabIndex = 4;
            // 
            // lblNamaLengkap
            // 
            this.lblNamaLengkap.AutoSize = true;
            this.lblNamaLengkap.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNamaLengkap.Location = new System.Drawing.Point(260, 182);
            this.lblNamaLengkap.Name = "lblNamaLengkap";
            this.lblNamaLengkap.Size = new System.Drawing.Size(236, 20);
            this.lblNamaLengkap.TabIndex = 5;
            this.lblNamaLengkap.Text = "Nama Pengguna (Nama Tampilan)";
            // 
            // txtNamaLengkap
            // 
            this.txtNamaLengkap.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.txtNamaLengkap.Location = new System.Drawing.Point(260, 204);
            this.txtNamaLengkap.MaxLength = 100;
            this.txtNamaLengkap.Name = "txtNamaLengkap";
            this.txtNamaLengkap.Size = new System.Drawing.Size(274, 28);
            this.txtNamaLengkap.TabIndex = 6;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPassword.Location = new System.Drawing.Point(260, 240);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(181, 20);
            this.lblPassword.TabIndex = 7;
            this.lblPassword.Text = "Password (min. 6 karakter)";
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.txtPassword.Location = new System.Drawing.Point(260, 262);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(274, 28);
            this.txtPassword.TabIndex = 8;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblKonfirmasi
            // 
            this.lblKonfirmasi.AutoSize = true;
            this.lblKonfirmasi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblKonfirmasi.Location = new System.Drawing.Point(260, 298);
            this.lblKonfirmasi.Name = "lblKonfirmasi";
            this.lblKonfirmasi.Size = new System.Drawing.Size(145, 20);
            this.lblKonfirmasi.TabIndex = 9;
            this.lblKonfirmasi.Text = "Konfirmasi Password";
            // 
            // txtKonfirmasi
            // 
            this.txtKonfirmasi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.txtKonfirmasi.Location = new System.Drawing.Point(260, 320);
            this.txtKonfirmasi.Name = "txtKonfirmasi";
            this.txtKonfirmasi.Size = new System.Drawing.Size(274, 28);
            this.txtKonfirmasi.TabIndex = 10;
            this.txtKonfirmasi.UseSystemPasswordChar = true;
            // 
            // btnDaftar
            // 
            this.btnDaftar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnDaftar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDaftar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.btnDaftar.ForeColor = System.Drawing.Color.White;
            this.btnDaftar.Location = new System.Drawing.Point(260, 362);
            this.btnDaftar.Name = "btnDaftar";
            this.btnDaftar.Size = new System.Drawing.Size(274, 42);
            this.btnDaftar.TabIndex = 11;
            this.btnDaftar.Text = "Daftar Akun";
            this.btnDaftar.UseVisualStyleBackColor = false;
            this.btnDaftar.Click += new System.EventHandler(this.btnDaftar_Click);
            // 
            // btnKembali
            // 
            this.btnKembali.BackColor = System.Drawing.Color.White;
            this.btnKembali.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKembali.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnKembali.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.btnKembali.Location = new System.Drawing.Point(260, 412);
            this.btnKembali.Name = "btnKembali";
            this.btnKembali.Size = new System.Drawing.Size(274, 36);
            this.btnKembali.TabIndex = 12;
            this.btnKembali.Text = "Kembali ke Login";
            this.btnKembali.UseVisualStyleBackColor = false;
            this.btnKembali.Click += new System.EventHandler(this.btnKembali_Click);
            // 
            // FormRegister
            // 
            this.AcceptButton = this.btnDaftar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 539);
            this.Controls.Add(this.btnKembali);
            this.Controls.Add(this.btnDaftar);
            this.Controls.Add(this.txtKonfirmasi);
            this.Controls.Add(this.lblKonfirmasi);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtNamaLengkap);
            this.Controls.Add(this.lblNamaLengkap);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblInfoRole);
            this.Controls.Add(this.lblJudul);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRegister";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Daftar Akun - IT Asset Management";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblJudul;
        private System.Windows.Forms.Label lblInfoRole;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblNamaLengkap;
        private System.Windows.Forms.TextBox txtNamaLengkap;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblKonfirmasi;
        private System.Windows.Forms.TextBox txtKonfirmasi;
        private System.Windows.Forms.Button btnDaftar;
        private System.Windows.Forms.Button btnKembali;
    }
}
