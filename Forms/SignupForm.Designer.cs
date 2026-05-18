namespace ChatNova.Forms
{
    partial class SignupForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblConfirmPwd;

        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtConfirmPwd;

        private System.Windows.Forms.Button btnSignup;
        private System.Windows.Forms.Button btnBackToLogin;

        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblSuccess;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblConfirmPwd = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtConfirmPwd = new System.Windows.Forms.TextBox();
            this.btnSignup = new System.Windows.Forms.Button();
            this.btnBackToLogin = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.lblSuccess = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // ── lblTitle ──────────────────────────────────────────
            this.lblTitle.Text = "Create Account";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16,
                                          System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lblTitle.Location = new System.Drawing.Point(60, 15);
            this.lblTitle.Size = new System.Drawing.Size(260, 35);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ── lblUsername ───────────────────────────────────────
            this.lblUsername.Text = "Username *";
            this.lblUsername.Location = new System.Drawing.Point(30, 65);
            this.lblUsername.Size = new System.Drawing.Size(150, 20);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 9);

            // ── txtUsername ───────────────────────────────────────
            this.txtUsername.Location = new System.Drawing.Point(30, 85);
            this.txtUsername.Size = new System.Drawing.Size(310, 26);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 10);
            this.txtUsername.TabIndex = 0;

            // ── lblEmail ──────────────────────────────────────────
            this.lblEmail.Text = "Email (required if no phone)";
            this.lblEmail.Location = new System.Drawing.Point(30, 120);
            this.lblEmail.Size = new System.Drawing.Size(200, 20);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9);

            // ── txtEmail ──────────────────────────────────────────
            this.txtEmail.Location = new System.Drawing.Point(30, 140);
            this.txtEmail.Size = new System.Drawing.Size(310, 26);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10);
            this.txtEmail.TabIndex = 1;

            // ── lblPhone ──────────────────────────────────────────
            this.lblPhone.Text = "Phone (required if no email)";
            this.lblPhone.Location = new System.Drawing.Point(30, 175);
            this.lblPhone.Size = new System.Drawing.Size(200, 20);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 9);

            // ── txtPhone ──────────────────────────────────────────
            this.txtPhone.Location = new System.Drawing.Point(30, 195);
            this.txtPhone.Size = new System.Drawing.Size(310, 26);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 10);
            this.txtPhone.TabIndex = 2;

            // ── lblPassword ───────────────────────────────────────
            this.lblPassword.Text = "Password *";
            this.lblPassword.Location = new System.Drawing.Point(30, 230);
            this.lblPassword.Size = new System.Drawing.Size(150, 20);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 9);

            // ── txtPassword ───────────────────────────────────────
            this.txtPassword.Location = new System.Drawing.Point(30, 250);
            this.txtPassword.Size = new System.Drawing.Size(310, 26);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.TabIndex = 3;

            // ── lblConfirmPwd ─────────────────────────────────────
            this.lblConfirmPwd.Text = "Confirm Password *";
            this.lblConfirmPwd.Location = new System.Drawing.Point(30, 285);
            this.lblConfirmPwd.Size = new System.Drawing.Size(150, 20);
            this.lblConfirmPwd.Name = "lblConfirmPwd";
            this.lblConfirmPwd.Font = new System.Drawing.Font("Segoe UI", 9);

            // ── txtConfirmPwd ─────────────────────────────────────
            this.txtConfirmPwd.Location = new System.Drawing.Point(30, 305);
            this.txtConfirmPwd.Size = new System.Drawing.Size(310, 26);
            this.txtConfirmPwd.Name = "txtConfirmPwd";
            this.txtConfirmPwd.Font = new System.Drawing.Font("Segoe UI", 10);
            this.txtConfirmPwd.PasswordChar = '*';
            this.txtConfirmPwd.TabIndex = 4;

            // ── btnSignup ─────────────────────────────────────────
            this.btnSignup.Location = new System.Drawing.Point(30, 345);
            this.btnSignup.Size = new System.Drawing.Size(310, 35);
            this.btnSignup.Name = "btnSignup";
            this.btnSignup.Text = "Create Account";
            this.btnSignup.Font = new System.Drawing.Font("Segoe UI", 10,
                                           System.Drawing.FontStyle.Bold);
            this.btnSignup.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSignup.ForeColor = System.Drawing.Color.White;
            this.btnSignup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSignup.TabIndex = 5;
            this.btnSignup.Click +=
                new System.EventHandler(this.btnSignup_Click);

            // ── btnBackToLogin ────────────────────────────────────
            this.btnBackToLogin.Location = new System.Drawing.Point(30, 390);
            this.btnBackToLogin.Size = new System.Drawing.Size(310, 30);
            this.btnBackToLogin.Name = "btnBackToLogin";
            this.btnBackToLogin.Text = "Back to Login";
            this.btnBackToLogin.Font = new System.Drawing.Font("Segoe UI", 9);
            this.btnBackToLogin.BackColor = System.Drawing.Color.FromArgb(96, 96, 96);
            this.btnBackToLogin.ForeColor = System.Drawing.Color.White;
            this.btnBackToLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackToLogin.TabIndex = 6;
            this.btnBackToLogin.Click +=
                new System.EventHandler(this.btnBackToLogin_Click);

            // ── lblError ──────────────────────────────────────────
            this.lblError.Location = new System.Drawing.Point(30, 430);
            this.lblError.Size = new System.Drawing.Size(310, 20);
            this.lblError.Name = "lblError";
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 9);
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Visible = false;
            this.lblError.TabIndex = 7;

            // ── lblSuccess ────────────────────────────────────────
            this.lblSuccess.Location = new System.Drawing.Point(30, 455);
            this.lblSuccess.Size = new System.Drawing.Size(310, 20);
            this.lblSuccess.Name = "lblSuccess";
            this.lblSuccess.Font = new System.Drawing.Font("Segoe UI", 9);
            this.lblSuccess.ForeColor = System.Drawing.Color.Green;
            this.lblSuccess.Visible = false;
            this.lblSuccess.TabIndex = 8;

            // ── SignupForm ────────────────────────────────────────
            this.ClientSize = new System.Drawing.Size(380, 490);
            this.Name = "SignupForm";
            this.Text = "ChatNova - Create Account";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.AcceptButton = this.btnSignup;

            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblConfirmPwd);
            this.Controls.Add(this.txtConfirmPwd);
            this.Controls.Add(this.btnSignup);
            this.Controls.Add(this.btnBackToLogin);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblSuccess);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}