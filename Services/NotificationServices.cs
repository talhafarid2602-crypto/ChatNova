using ChatNova.Data;
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
    /// Create, load, mark read notifications
    /// </summary>
    public class NotificationService
    {
        // -------------------------------------------------------
        // GET ALL NOTIFICATIONS FOR USER
        // -------------------------------------------------------
        public List<Notification> GetNotifications(int userId)
        {
            string query = @"
                SELECT NotificationId, UserId, Title,
                       Message, IsRead, CreatedAt
                FROM Notifications
                WHERE UserId = @UserId
                ORDER BY CreatedAt DESC";

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", userId)
            };

            var notifications = new List<Notification>();
            var table = DatabaseConnection.ExecuteDataTable(query, parameters);

            foreach (System.Data.DataRow row in table.Rows)
            {
                notifications.Add(new Notification
                {
                    NotificationId = (int)row["NotificationId"],
                    UserId = (int)row["UserId"],
                    Title = row["Title"].ToString(),
                    Message = row["Message"].ToString(),
                    IsRead = (bool)row["IsRead"],
                    CreatedAt = (DateTime)row["CreatedAt"]
                });
            }

            return notifications;
        }

        // -------------------------------------------------------
        // CREATE NOTIFICATION
        // -------------------------------------------------------
        public bool CreateNotification(int userId, string title, string message)
        {
            string query = @"
                INSERT INTO Notifications (UserId, Title, Message)
                VALUES (@UserId, @Title, @Message)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId",  userId),
                new SqlParameter("@Title",   title),
                new SqlParameter("@Message", message)
            };

            return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // -------------------------------------------------------
        // MARK ONE AS READ
        // -------------------------------------------------------
        public bool MarkAsRead(int notificationId)
        {
            string query = @"
                UPDATE Notifications
                SET IsRead = 1
                WHERE NotificationId = @NotificationId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@NotificationId", notificationId)
            };

            return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // -------------------------------------------------------
        // MARK ALL AS READ
        // -------------------------------------------------------
        public bool MarkAllAsRead(int userId)
        {
            string query = @"
                UPDATE Notifications
                SET IsRead = 1
                WHERE UserId = @UserId AND IsRead = 0";

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", userId)
            };

            return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // -------------------------------------------------------
        // UNREAD COUNT
        // -------------------------------------------------------
        public int GetUnreadCount(int userId)
        {
            string query = @"
                SELECT COUNT(1) FROM Notifications
                WHERE UserId = @UserId AND IsRead = 0";

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", userId)
            };

            return Convert.ToInt32(
                DatabaseConnection.ExecuteScalar(query, parameters));
        }
    }
}