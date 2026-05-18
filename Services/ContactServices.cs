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
    /// Add, remove, load contacts
    /// Also handles block / unblock
    /// </summary>
    public class ContactService
    {
        // -------------------------------------------------------
        // GET ALL CONTACTS
        // -------------------------------------------------------
        public List<Contact> GetContacts(int userId)
        {
            string query = @"
                SELECT c.ContactId, c.UserId, c.ContactUserId,
                       c.NickName,  c.CreatedAt,
                       u.Username AS ContactUsername,
                       u.Email    AS ContactEmail,
                       u.PhoneNumber AS ContactPhone,
                       CASE WHEN b.BlockId IS NOT NULL
                            THEN 1 ELSE 0
                       END AS IsBlocked
                FROM Contacts c
                INNER JOIN IUsers u ON c.ContactUserId = u.UserId
                LEFT JOIN BlockedContacts b
                    ON b.UserId        = @UserId
                    AND b.BlockedUserId = c.ContactUserId
                WHERE c.UserId = @UserId
                ORDER BY u.Username ASC";

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", userId)
            };

            var contacts = new List<Contact>();
            var table = DatabaseConnection.ExecuteDataTable(query, parameters);

            foreach (System.Data.DataRow row in table.Rows)
            {
                contacts.Add(new Contact
                {
                    ContactId = (int)row["ContactId"],
                    UserId = (int)row["UserId"],
                    ContactUserId = (int)row["ContactUserId"],
                    NickName = row["NickName"] as string,
                    CreatedAt = (DateTime)row["CreatedAt"],
                    ContactUsername = row["ContactUsername"].ToString(),
                    ContactEmail = row["ContactEmail"] as string,
                    ContactPhone = row["ContactPhone"] as string,
                    IsBlocked = Convert.ToBoolean(row["IsBlocked"])
                });
            }

            return contacts;
        }

        // -------------------------------------------------------
        // ADD CONTACT
        // -------------------------------------------------------
        public bool AddContact(int userId, int contactUserId,
                               string nickName = null)
        {
            if (userId == contactUserId)
                throw new Exception("You cannot add yourself.");

            string query = @"
                INSERT INTO Contacts (UserId, ContactUserId, NickName)
                VALUES (@UserId, @ContactUserId, @NickName)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId",        userId),
                new SqlParameter("@ContactUserId", contactUserId),
                new SqlParameter("@NickName",      (object)nickName ?? DBNull.Value)
            };

            try
            {
                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                    throw new Exception("Contact already added.");
                throw;
            }
        }

        // -------------------------------------------------------
        // REMOVE CONTACT
        // -------------------------------------------------------
        public bool RemoveContact(int userId, int contactUserId)
        {
            string query = @"
                DELETE FROM Contacts
                WHERE UserId = @UserId AND ContactUserId = @ContactUserId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId",        userId),
                new SqlParameter("@ContactUserId", contactUserId)
            };

            return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // -------------------------------------------------------
        // BLOCK USER
        // -------------------------------------------------------
        public bool BlockUser(int userId, int blockUserId)
        {
            if (userId == blockUserId)
                throw new Exception("You cannot block yourself.");

            string query = @"
                INSERT INTO BlockedContacts (UserId, BlockedUserId)
                VALUES (@UserId, @BlockedUserId)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId",        userId),
                new SqlParameter("@BlockedUserId", blockUserId)
            };

            try
            {
                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                    throw new Exception("User is already blocked.");
                throw;
            }
        }

        // -------------------------------------------------------
        // UNBLOCK USER
        // -------------------------------------------------------
        public bool UnblockUser(int userId, int blockedUserId)
        {
            string query = @"
                DELETE FROM BlockedContacts
                WHERE UserId = @UserId AND BlockedUserId = @BlockedUserId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId",        userId),
                new SqlParameter("@BlockedUserId", blockedUserId)
            };

            return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // -------------------------------------------------------
        // IS BLOCKED
        // -------------------------------------------------------
        public bool IsBlocked(int userId, int otherUserId)
        {
            string query = @"
                SELECT COUNT(1) FROM BlockedContacts
                WHERE UserId = @UserId AND BlockedUserId = @OtherUserId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId",      userId),
                new SqlParameter("@OtherUserId", otherUserId)
            };

            return Convert.ToInt32(
                DatabaseConnection.ExecuteScalar(query, parameters)) > 0;
        }

        // -------------------------------------------------------
        // SEARCH USERS (to add as contact)
        // -------------------------------------------------------
        public List<IUser> SearchUsers(string searchTerm)
        {
            string query = @"
                SELECT UserId, Username, Email, PhoneNumber
                FROM IUsers
                WHERE (Username    LIKE @Term
                   OR  Email       LIKE @Term
                   OR  PhoneNumber LIKE @Term)
                  AND IsActive = 1
                  AND UserId  != @CurrentUserId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Term",          "%" + searchTerm + "%"),
                new SqlParameter("@CurrentUserId", SessionManager.CurrentUserId)
            };

            var users = new List<IUser>();
            var table = DatabaseConnection.ExecuteDataTable(query, parameters);

            foreach (System.Data.DataRow row in table.Rows)
            {
                users.Add(new IUser
                {
                    UserId = (int)row["UserId"],
                    Username = row["Username"].ToString(),
                    Email = row["Email"] as string,
                    PhoneNumber = row["PhoneNumber"] as string
                });
            }

            return users;
        }
    }
}