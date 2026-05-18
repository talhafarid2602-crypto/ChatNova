namespace ChatNova
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.LinkLabel lnkSignUp;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.lnkSignUp = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();

            // ── lblTitle ──────────────────────────────────────────
            this.lblTitle.Text = "ChatNova";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18,
                                          System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lblTitle.Location = new System.Drawing.Point(130, 20);
            this.lblTitle.Size = new System.Drawing.Size(160, 40);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ── txtUsername ───────────────────────────────────────
            this.txtUsername.Location = new System.Drawing.Point(80, 80);
            this.txtUsername.Size = new System.Drawing.Size(240, 26);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 10);
            this.txtUsername.Text = "Username";
            this.txtUsername.ForeColor = System.Drawing.Color.Gray;
            this.txtUsername.TabIndex = 0;
            this.txtUsername.Enter +=
                new System.EventHandler(this.txtUsername_Enter);
            this.txtUsername.Leave +=
                new System.EventHandler(this.txtUsername_Leave);

            // ── txtPassword ───────────────────────────────────────
            this.txtPassword.Location = new System.Drawing.Point(80, 120);
            this.txtPassword.Size = new System.Drawing.Size(240, 26);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10);
            this.txtPassword.Text = "Password";
            this.txtPassword.ForeColor = System.Drawing.Color.Gray;
            this.txtPassword.UseSystemPasswordChar = false;
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Enter +=
                new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.Leave +=
                new System.EventHandler(this.txtPassword_Leave);

            // ── btnLogin ──────────────────────────────────────────
            this.btnLogin.Location = new System.Drawing.Point(80, 165);
            this.btnLogin.Size = new System.Drawing.Size(240, 35);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Text = "Login";
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 10,
                                          System.Drawing.FontStyle.Bold);
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Click +=
                new System.EventHandler(this.btnLogin_Click);

            // ── lblError ──────────────────────────────────────────
            this.lblError.Location = new System.Drawing.Point(80, 210);
            this.lblError.Size = new System.Drawing.Size(240, 20);
            this.lblError.Name = "lblError";
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 9);
            this.lblError.Visible = false;
            this.lblError.TabIndex = 3;

            // ── lnkSignUp ─────────────────────────────────────────
            this.lnkSignUp.Location = new System.Drawing.Point(80, 240);
            this.lnkSignUp.Size = new System.Drawing.Size(240, 20);
            this.lnkSignUp.Name = "lnkSignUp";
            this.lnkSignUp.Text = "Don't have an account? Sign Up";
            this.lnkSignUp.Font = new System.Drawing.Font("Segoe UI", 9);
            this.lnkSignUp.TabIndex = 4;
            this.lnkSignUp.TabStop = true;
            this.lnkSignUp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkSignUp.LinkClicked +=
                new System.Windows.Forms.LinkLabelLinkClickedEventHandler(
                    this.lnkSignUp_LinkClicked);

            // ── LoginForm ─────────────────────────────────────────
            this.ClientSize = new System.Drawing.Size(400, 290);
            this.Name = "LoginForm";
            this.Text = "ChatNova - Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.AcceptButton = this.btnLogin;

            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lnkSignUp);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}