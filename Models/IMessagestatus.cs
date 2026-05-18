using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNova.Models
{
    /// <summary>
    /// Maps exactly to IMessageStatus table in newChatNovaDB
    /// Tracks Sent / Delivered / Seen per user per message
    /// </summary>
    public class IMessageStatus
    {
        // ===========================================================
        // EXACT MATCH TO IMessageStatus TABLE COLUMNS
        // ===========================================================

        public int MessageStatusId { get; set; }
        public int MessageId { get; set; }         // FK → IMessages
        public int UserId { get; set; }            // FK → IUsers
        public string Status { get; set; }         // Sent/Delivered/Seen
        public DateTime UpdatedAt { get; set; }

        // ===========================================================
        // STATUS CONSTANTS — never use raw strings
        // ===========================================================

        public const string STATUS_SENT = "Sent";
        public const string STATUS_DELIVERED = "Delivered";
        public const string STATUS_SEEN = "Seen";

        // ===========================================================
        // CONSTRUCTORS
        // ===========================================================

        public IMessageStatus()
        {
            Status = STATUS_SENT;
            UpdatedAt = DateTime.Now;
        }

        public IMessageStatus(int messageId, int userId, string status)
        {
            MessageId = messageId;
            UserId = userId;
            Status = status;
            UpdatedAt = DateTime.Now;
        }

        // ===========================================================
        // HELPERS
        // ===========================================================

        public bool IsSent() => Status == STATUS_SENT;
        public bool IsDelivered() => Status == STATUS_DELIVERED;
        public bool IsSeen() => Status == STATUS_SEEN;

        /// <summary>Returns tick icon for UI (like WhatsApp)</summary>
        public string GetStatusIcon()
        {
            switch (Status)
            {
                case STATUS_SENT: return "✓";
                case STATUS_DELIVERED: return "✓✓";
                case STATUS_SEEN: return "✓✓";  // blue in UI
                default: return "";
            }
        }

        public bool IsValid()
        {
            return MessageId > 0 && UserId > 0
                && (Status == STATUS_SENT
                 || Status == STATUS_DELIVERED
                 || Status == STATUS_SEEN);
        }

        public override string ToString()
        {
            return $"Status[Msg#{MessageId}] " +
                   $"User#{UserId} → {Status} at {UpdatedAt:HH:mm}";
        }
    }
}
