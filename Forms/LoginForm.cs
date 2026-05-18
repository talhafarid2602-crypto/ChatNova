using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatNova.Forms;
using ChatNova.Helpers;
using ChatNova.Services;

namespace ChatNova
{
    public partial class LoginForm : Form
    {
        // -------------------------------------------------------
        // SERVICE — only AuthService needed for Login
        // -------------------------------------------------------
        private readonly AuthService _authService;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        // -------------------------------------------------------
        // LOGIN CLICK
        // -------------------------------------------------------
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // Validate — ignore placeholder text
            if (string.IsNullOrWhiteSpace(username)
                || txtUsername.ForeColor == Color.Gray)
            {
                ShowError("Please enter your username.");
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password)
                || txtPassword.ForeColor == Color.Gray)
            {
                ShowError("Please enter your password.");
                txtPassword.Focus();
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Logging in...";

            try
            {
                // Run in background — prevents UI freeze
                var user = await Task.Run(() =>
                    _authService.Login(username, password)
                );

                // SessionManager.Login() is called inside AuthService
                // so CurrentUserId is ready here
                ChatForm chatForm = new ChatForm(SessionManager.CurrentUserId);
                chatForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                txtPassword.Clear();
                txtPassword.Focus();
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Login";
            }
        }

        // -------------------------------------------------------
        // GO TO SIGNUP
        // -------------------------------------------------------
        private void lnkSignUp_LinkClicked(object sender,
            LinkLabelLinkClickedEventArgs e)
        {
            new SignupForm().Show();
            this.Hide();
        }

        // -------------------------------------------------------
        // ERROR DISPLAY
        // -------------------------------------------------------
        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        // -------------------------------------------------------
        // PLACEHOLDER — USERNAME
        // -------------------------------------------------------
        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.ForeColor == Color.Gray)
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Username";
                txtUsername.ForeColor = Color.Gray;
            }
        }

        // -------------------------------------------------------
        // PLACEHOLDER — PASSWORD
        // -------------------------------------------------------
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.ForeColor == Color.Gray)
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Password";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }
        }
    }
}
