using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCrypt.Net;

namespace ChatNova.Helpers
{
    /// <summary>
    /// Handles password hashing and verification using BCrypt
    /// NEVER store plain text passwords
    /// </summary>
    public static class PasswordHelper
    {
        // BCrypt work factor — higher = slower = more secure
        private const int WorkFactor = 12;

        /// <summary>
        /// Hash plain text password before storing in DB
        /// </summary>
        public static string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword, WorkFactor);
        }

        /// <summary>
        /// Verify login — compare plain input vs stored hash
        /// </summary>
        public static bool VerifyPassword(string plainPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, storedHash);
        }
    }
}