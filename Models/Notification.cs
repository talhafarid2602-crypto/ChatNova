using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNova.Models
{
    /// <summary>
    /// Maps exactly to Notifications table in newChatNovaDB
    /// </summary>
    public class Notification
    {
        // ===========================================================
        // EXACT MATCH TO Notifications TABLE COLUMNS
        // ===========================================================

        public int NotificationId { get; set; }
        public int UserId { get; set; }            // FK → IUsers
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }           // DEFAULT 0
        public DateTime CreatedAt { get; set; }

        // ===========================================================
        // CONSTRUCTORS
        // ===========================================================

        public Notification()
        {
            IsRead = false;
            CreatedAt = DateTime.Now;
        }

        public Notification(int userId, string title, string message)
        {
            UserId = userId;
            Title = title;
            Message = message;
            IsRead = false;
            CreatedAt = DateTime.Now;
        }

        // ===========================================================
        // HELPERS
        // ===========================================================

        public string GetFormattedTime()
        {
            return CreatedAt.Date == DateTime.Today
                ? CreatedAt.ToString("hh:mm tt")
                : CreatedAt.ToString("MMM dd, hh:mm tt");
        }

        // ===========================================================
        // VALIDATION
        // ===========================================================

        public bool IsValid()
        {
            return UserId > 0
                && !string.IsNullOrWhiteSpace(Title)
                && !string.IsNullOrWhiteSpace(Message);
        }

        public override string ToString()
        {
            return $"Notification[{NotificationId}] " +
                   $"→ User#{UserId} | {Title} | " +
                   $"Read: {IsRead}";
        }
    }
}