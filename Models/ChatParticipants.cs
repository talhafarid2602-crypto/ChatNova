using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNova.Models
{
    /// <summary>
    /// Maps exactly to ChatParticipants table in newChatNovaDB
    /// Handles both Private and Group chat members
    /// </summary>
    public class ChatParticipant
    {
        // ===========================================================
        // EXACT MATCH TO ChatParticipants TABLE COLUMNS
        // ===========================================================

        public int ChatParticipantId { get; set; }
        public int ChatId { get; set; }            // FK → Chats
        public int UserId { get; set; }            // FK → IUsers
        public DateTime JoinedAt { get; set; }

        // ===========================================================
        // UI / JOIN FIELDS (NOT stored in DB)
        // ===========================================================

        public string Username { get; set; }       // loaded via JOIN
        public bool IsOnline { get; set; }         // from session/signalR

        // ===========================================================
        // CONSTRUCTORS
        // ===========================================================

        public ChatParticipant()
        {
            JoinedAt = DateTime.Now;
        }

        public ChatParticipant(int chatId, int userId)
        {
            ChatId = chatId;
            UserId = userId;
            JoinedAt = DateTime.Now;
        }

        // ===========================================================
        // VALIDATION
        // ===========================================================

        public bool IsValid()
        {
            return ChatId > 0 && UserId > 0;
        }

        public override string ToString()
        {
            return $"Participant[User#{UserId}] in Chat#{ChatId} " +
                   $"joined {JoinedAt:MMM dd, yyyy}";
        }
    }
}
