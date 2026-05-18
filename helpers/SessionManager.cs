using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNova.Helpers
{
    /// <summary>
    /// Holds logged-in user data across all forms
    /// Set on login — cleared on logout
    /// </summary>
    public static class SessionManager
    {
        public static int CurrentUserId { get; private set; }
        public static string CurrentUsername { get; private set; }
        public static string CurrentEmail { get; private set; }
        public static string CurrentPhone { get; private set; }
        public static bool IsLoggedIn { get; private set; }

        /// <summary>Call this after successful login</summary>
        public static void Login(Models.IUser user)
        {
            CurrentUserId = user.UserId;
            CurrentUsername = user.Username;
            CurrentEmail = user.Email;
            CurrentPhone = user.PhoneNumber;
            IsLoggedIn = true;
        }

        /// <summary>Call this on logout</summary>
        public static void Logout()
        {
            CurrentUserId = 0;
            CurrentUsername = null;
            CurrentEmail = null;
            CurrentPhone = null;
            IsLoggedIn = false;
        }
    }
}