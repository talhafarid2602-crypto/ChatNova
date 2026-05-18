using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNova.Models
{
    /// <summary>
    /// Maps exactly to IMessages table in newChatNovaDB
    /// </summary>
    public class IMessage
    {
        // ===========================================================
        // EXACT MATCH TO IMessages TABLE COLUMNS
        // ===========================================================

        public int MessageId { get; set; }

        public int ChatId { get; set; }            // was ConversationID — FIXED

        public int SenderId { get; set; }

        /// <summary>
        /// Stored ENCRYPTED in DB — never display raw
        /// Always decrypt before showing in UI
        /// </summary>
        public string MessageText { get; set; }    // was Content — FIXED

        public DateTime SentAt { get; set; }

        public DateTime? EditedAt { get; set; }    // was missing — ADDED

        public bool IsDeleted { get; set; }        // DEFAULT 0

        // ===========================================================
        // UI / JOIN FIELDS (not stored in DB)
        // ===========================================================

        public string SenderName { get; set; }     // loaded via JOIN
        public bool IsMyMessage { get; set; }      // set in UI layer

        // ===========================================================
        // CONSTRUCTORS
        // ===========================================================

        public IMessage()
        {
            IsDeleted = false;
            SentAt = DateTime.Now;
        }

        public IMessage(int chatId, int senderId, string encryptedMessageText)
        {
            ChatId = chatId;
            SenderId = senderId;
            MessageText = encryptedMessageText;
            IsDeleted = false;
            SentAt = DateTime.Now;
        }

        // ===========================================================
        // UI HELPERS
        // ===========================================================

        public string GetFormattedTime()
        {
            return SentAt.Date == DateTime.Today
                ? SentAt.ToString("hh:mm tt")
                : SentAt.ToString("MMM dd, hh:mm tt");
        }

        public string GetPreview(string decryptedText)
        {
            if (IsDeleted) return "🚫 This message was deleted";
            if (string.IsNullOrWhiteSpace(decryptedText)) return "";
            return decryptedText.Length > 40
                ? decryptedText.Substring(0, 40) + "..."
                : decryptedText;
        }

        public bool IsEdited() => EditedAt.HasValue;

        // ===========================================================
        // VALIDATION
        // ===========================================================

        public bool IsValid()
        {
            return ChatId > 0
                && SenderId > 0
                && !string.IsNullOrWhiteSpace(MessageText);
        }

        public override string ToString() =>
            $"[{SentAt:HH:mm}] Msg#{MessageId} by User#{SenderId} in Chat#{ChatId}";
    }
}