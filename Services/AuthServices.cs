using ChatNova.Data;
using ChatNova.Helpers;
using ChatNova.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNova.Services
{
    /// <summary>
    /// Handles Register + Login
    /// Uses BCrypt for password — stores hash only
    /// </summary>
    public class AuthService
    {
        // -------------------------------------------------------
        // REGISTER
        // -------------------------------------------------------
        public bool Register(string username, string plainPassword,
                     string email, string phoneNumber)
        {
            // Convert empty strings to null explicitly
            if (string.IsNullOrWhiteSpace(email)) email = null;
            if (string.IsNullOrWhiteSpace(phoneNumber)) phoneNumber = null;

            // Hash password
            string hashedPassword = PasswordHelper.HashPassword(plainPassword);

            string query = @"
        INSERT INTO IUsers 
            (Username, PasswordHash, Email, PhoneNumber)
        VALUES 
            (@Username, @PasswordHash, @Email, @PhoneNumber)";

            SqlParameter[] parameters =
            {
        new SqlParameter("@Username",     username),
        new SqlParameter("@PasswordHash", hashedPassword),
        new SqlParameter("@Email",        (object)email       ?? DBNull.Value),
        new SqlParameter("@PhoneNumber",  (object)phoneNumber ?? DBNull.Value)
    };

            try
            {
                int rows = DatabaseConnection.ExecuteNonQuery(query, parameters);
                return rows > 0;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                    throw new Exception("Username, email or phone already exists.");
                throw;
            }
        }

        // -------------------------------------------------------
        // LOGIN
        // -------------------------------------------------------
        public IUser Login(string usernameOrEmail, string plainPassword)
        {
            string query = @"
                SELECT UserId, Username, PasswordHash,
                       Email, PhoneNumber, IsActive
                FROM IUsers
                WHERE (Username = @Input OR Email = @Input)
                  AND IsActive = 1";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Input", usernameOrEmail)
            };

            using (var reader = DatabaseConnection.ExecuteReader(query, parameters))
            {
                if (!reader.Read())
                    throw new Exception("User not found.");

                string storedHash = reader["PasswordHash"].ToString();

                // Verify BCrypt hash
                if (!PasswordHelper.VerifyPassword(plainPassword, storedHash))
                    throw new Exception("Incorrect password.");

                // Build user object
                IUser user = new IUser
                {
                    UserId = (int)reader["UserId"],
                    Username = reader["Username"].ToString(),
                    PasswordHash = storedHash,
                    Email = reader["Email"] as string,
                    PhoneNumber = reader["PhoneNumber"] as string,
                    IsActive = (bool)reader["IsActive"]
                };

                // Save to session
                SessionManager.Login(user);
                return user;
            }
        }

        // -------------------------------------------------------
        // LOGOUT
        // -------------------------------------------------------
        public void Logout()
        {
            SessionManager.Logout();
        }

        // -------------------------------------------------------
        // CHECK IF USERNAME EXISTS
        // -------------------------------------------------------
        public bool UsernameExists(string username)
        {
            string query = "SELECT COUNT(1) FROM IUsers WHERE Username = @Username";
            SqlParameter[] parameters =
            {
                new SqlParameter("@Username", username)
            };
            object result = DatabaseConnection.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result) > 0;
        }
    }
}
