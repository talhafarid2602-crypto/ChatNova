using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNova.Models
{
    /// <summary>
    /// Maps exactly to BlockedContacts table in newChatNovaDB
    /// </summary>
    public class BlockedContact
    {
        // ===========================================================
        // EXACT MATCH TO BlockedContacts TABLE COLUMNS
        // ===========================================================

        public int BlockId { get; set; }
        public int UserId { get; set; }            // FK → IUsers (who blocked)
        public int BlockedUserId { get; set; }     // FK → IUsers (who is blocked)
        public DateTime BlockedAt { get; set; }

        // ===========================================================
        // UI / JOIN FIELDS (NOT stored in DB)
        // ===========================================================

        public string BlockedUsername { get; set; }    // loaded via JOIN

        // ===========================================================
        // CONSTRUCTORS
        // ===========================================================

        public BlockedContact()
        {
            BlockedAt = DateTime.Now;
        }

        public BlockedContact(int userId, int blockedUserId)
        {
            UserId = userId;
            BlockedUserId = blockedUserId;
            BlockedAt = DateTime.Now;
        }

        // ===========================================================
        // VALIDATION
        // ===========================================================

        public bool IsValid()
        {
            return UserId > 0
                && BlockedUserId > 0
                && UserId != BlockedUserId;    // can't block yourself
        }

        public override string ToString()
        {
            return $"Blocked[{BlockId}] " +
                   $"User#{UserId} blocked {BlockedUsername ?? $"User#{BlockedUserId}"} " +
                   $"at {BlockedAt:MMM dd, yyyy}";
        }
    }
}