using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ChatNova.Services;

namespace ChatNova.Forms
{
    public partial class SignupForm : Form
    {
        // -------------------------------------------------------
        // SERVICE — only AuthService needed for Signup
        // -------------------------------------------------------
        private readonly AuthService _authService;

        public SignupForm()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        // -------------------------------------------------------
        // SIGNUP CLICK
        // -------------------------------------------------------
        private async void btnSignup_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblSuccess.Visible = false;

            // NOTE: FullName removed — not in IUsers table
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string password = txtPassword.Text;
            string confirmPwd = txtConfirmPwd.Text;

            // Validate inputs
            string error = ValidateInputs(username, email, phone, password, confirmPwd);
            if (error != null)
            {
                ShowError(error);
                return;
            }

            btnSignup.Enabled = false;
            btnSignup.Text = "Creating account...";

            try
            {
                // Run in background — prevents UI freeze
                bool success = await System.Threading.Tasks.Task.Run(() =>
                      _authService.Register(
                              username,
                              password,
                     string.IsNullOrWhiteSpace(email) ? null : email,
                     string.IsNullOrWhiteSpace(phone) ? null : phone
    )
);
                if (success)
                {
                    ShowSuccess("Account created! Redirecting to login...");

                    // Wait 2 seconds then go to login
                    await System.Threading.Tasks.Task.Delay(2000);

                    new LoginForm().Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                btnSignup.Enabled = true;
                btnSignup.Text = "Create Account";
            }
        }

        // -------------------------------------------------------
        // BACK TO LOGIN
        // -------------------------------------------------------
        private void btnBackToLogin_Click(object sender, EventArgs e)
        {
            new LoginForm().Show();
            this.Close();
        }

        // -------------------------------------------------------
        // VALIDATION — matches exactly IUsers table constraints
        // Either email OR phone must be provided (both optional
        // but at least one required for contact)
        // -------------------------------------------------------
        private string ValidateInputs(string username, string email,
                                      string phone, string password,
                                      string confirmPwd)
        {
            // Username
            if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
                return "Username must be at least 3 characters.";

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
                return "Username can only contain letters, numbers, underscores.";

            // Email + Phone — at least one required
            bool hasEmail = !string.IsNullOrWhiteSpace(email);
            bool hasPhone = !string.IsNullOrWhiteSpace(phone);

            if (!hasEmail && !hasPhone)
                return "Please enter at least an email or phone number.";

            if (hasEmail && !IsValidEmail(email))
                return "Please enter a valid email address.";

            if (hasPhone && !IsValidPhone(phone))
                return "Phone number can only contain numbers, +, -, spaces.";

            // Password
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                return "Password must be at least 6 characters.";

            if (password != confirmPwd)
                return "Passwords do not match.";

            return null; // all good
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^[\d\s\+\-]+$");
        }

        // -------------------------------------------------------
        // UI HELPERS
        // -------------------------------------------------------
        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        private void ShowSuccess(string message)
        {
            lblSuccess.Text = message;
            lblSuccess.Visible = true;
        }
    }
}
