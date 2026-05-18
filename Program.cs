using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatNova.Data;
using ChatNova.Forms;
namespace ChatNova
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // STEP 1: Validate DB connection before app starts
                if (!DatabaseConnection.TestConnection())
                {
                    MessageBox.Show(
                        "Database connection failed.\n\n" +
                        "Please verify:\n" +
                        "- SQL Server is running\n" +
                        "- Connection string is correct\n" +
                        "- ChatNovaDB database exists",
                        "ChatNova - Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // STEP 2: Show Login first
                Application.Run(new LoginForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unexpected error:\n\n" + ex.Message,
                    "ChatNova - Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}