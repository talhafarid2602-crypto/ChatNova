using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNova.Models
{
    /// <summary>
    /// Maps exactly to Contacts table in newChatNovaDB
    /// </summary>
    public class Contact
    {
        // ===========================================================
        // EXACT MATCH TO Contacts TABLE COLUMNS
        // ===========================================================

        public int ContactId { get; set; }
        public int UserId { get; set; }            // FK → IUsers (owner)
        public int ContactUserId { get; set; }     // FK → IUsers (the contact)
        public string NickName { get; set; }       // NULL allowed
        public DateTime CreatedAt { get; set; }

        // ===========================================================
        // UI / JOIN FIELDS (NOT stored in DB)
        // ===========================================================

        public string ContactUsername { get; set; }    // loaded via JOIN
        public string ContactEmail { get; set; }       // loaded via JOIN
        public string ContactPhone { get; set; }       // loaded via JOIN
        public bool IsBlocked { get; set; }            // checked from BlockedContacts

        // ===========================================================
        // CONSTRUCTORS
        // ===========================================================

        public Contact()
        {
            CreatedAt = DateTime.Now;
        }

        public Contact(int userId, int contactUserId, string nickName = null)
        {
            UserId = userId;
            ContactUserId = contactUserId;
            NickName = nickName;
            CreatedAt = DateTime.Now;
        }

        // ===========================================================
        // HELPERS
        // ===========================================================

        /// <summary>
        /// Shows NickName if set, otherwise real username
        /// </summary>
        public string GetDisplayName()
        {
            return !string.IsNullOrWhiteSpace(NickName)
                ? NickName
                : ContactUsername ?? "Unknown";
        }

        // ===========================================================
        // VALIDATION
        // ===========================================================

        public bool IsValid()
        {
            return UserId > 0
                && ContactUserId > 0
                && UserId != ContactUserId;    // can't add yourself
        }

        public override string ToString()
        {
            return $"Contact[{ContactId}] " +
                   $"User#{UserId} → {GetDisplayName()}";
        }
    }
}