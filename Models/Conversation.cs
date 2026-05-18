using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNova.Models
{
    /// <summary>
    /// Maps exactly to Chats table in newChatNovaDB
    /// ChatType is either 'Private' or 'Group'
    /// </summary>
    public class Chat
    {
        // ===========================================================
        // EXACT MATCH TO Chats TABLE COLUMNS
        // ===========================================================

        public int ChatId { get; set; }

        /// <summary>
        /// Either "Private" or "Group" — enforced as constants below
        /// </summary>
        public string ChatType { get; set; }

        public DateTime CreatedAt { get; set; }

        // ===========================================================
        // CHAT TYPE CONSTANTS — never use raw strings
        // ===========================================================

        public const string TYPE_PRIVATE = "Private";
        public const string TYPE_GROUP = "Group";

        // ===========================================================
        // UI / JOIN FIELDS (NOT stored in DB)
        // ===========================================================

        /// <summary>Other user's name — for Private chats only</summary>
        public string OtherPersonName { get; set; }

        /// <summary>Group name — loaded from Groups table via JOIN</summary>
        public string GroupName { get; set; }

        /// <summary>Last message preview — calculated from IMessages</summary>
        public string LastMessage { get; set; }

        /// <summary>Time of last message — calculated from IMessages</summary>
        public DateTime? LastMessageTime { get; set; }

        /// <summary>Unread count — calculated from IMessageStatus</summary>
        public int UnreadCount { get; set; }

        // ===========================================================
        // CONSTRUCTORS
        // ===========================================================

        public Chat()
        {
            ChatType = TYPE_PRIVATE;
            CreatedAt = DateTime.Now;
            UnreadCount = 0;
        }

        public Chat(string chatType)
        {
            ChatType = chatType == TYPE_GROUP ? TYPE_GROUP : TYPE_PRIVATE;
            CreatedAt = DateTime.Now;
            UnreadCount = 0;
        }

        // ===========================================================
        // BUSINESS LOGIC HELPERS
        // ===========================================================

        public bool IsGroup() => ChatType == TYPE_GROUP;
        public bool IsPrivate() => ChatType == TYPE_PRIVATE;

        /// <summary>
        /// Returns correct display name for UI chat list
        /// Private → other person's name
        /// Group   → group name from Groups table
        /// </summary>
        public string GetDisplayName()
        {
            if (IsGroup())
                return string.IsNullOrWhiteSpace(GroupName)
                    ? "Unnamed Group"
                    : GroupName;

            return string.IsNullOrWhiteSpace(OtherPersonName)
                ? "Unknown User"
                : OtherPersonName;
        }

        /// <summary>
        /// Safe decrypted message preview for UI list
        /// Pass in already-decrypted text from EncryptionService
        /// </summary>
        public string GetMessagePreview(string decryptedLastMessage = null)
        {
            string text = decryptedLastMessage ?? LastMessage;

            if (string.IsNullOrWhiteSpace(text))
                return "No messages yet";

            return text.Length > 40
                ? text.Substring(0, 40) + "..."
                : text;
        }

        public string GetFormattedTime()
        {
            if (!LastMessageTime.HasValue) return "";

            return LastMessageTime.Value.Date == DateTime.Today
                ? LastMessageTime.Value.ToString("hh:mm tt")
                : LastMessageTime.Value.ToString("MMM dd");
        }

        // ===========================================================
        // VALIDATION
        // ===========================================================

        public bool IsValid()
        {
            return ChatType == TYPE_PRIVATE || ChatType == TYPE_GROUP;
        }

        public override string ToString()
        {
            return $"Chat[{ChatId}] {ChatType} — {GetDisplayName()}";
        }
    }
}