using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNova.Models
{
    /// <summary>
    /// Maps exactly to Groups table in newChatNovaDB
    /// </summary>
    public class Group
    {
        // ===========================================================
        // EXACT MATCH TO Groups TABLE COLUMNS
        // ===========================================================

        public int GroupId { get; set; }
        public int ChatId { get; set; }            // FK → Chats
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }  // NULL allowed
        public int CreatedBy { get; set; }         // FK → IUsers
        public DateTime CreatedAt { get; set; }

        // ===========================================================
        // UI / JOIN FIELDS (NOT stored in DB)
        // ===========================================================

        public string CreatedByUsername { get; set; }  // loaded via JOIN
        public int MemberCount { get; set; }           // calculated

        // ===========================================================
        // CONSTRUCTORS
        // ===========================================================

        public Group()
        {
            CreatedAt = DateTime.Now;
        }

        public Group(int chatId, string groupName, int createdBy)
        {
            ChatId = chatId;
            GroupName = groupName;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
        }

        // ===========================================================
        // VALIDATION
        // ===========================================================

        public bool IsValid()
        {
            return ChatId > 0
                && CreatedBy > 0
                && !string.IsNullOrWhiteSpace(GroupName);
        }

        public override string ToString()
        {
            return $"Group[{GroupId}] {GroupName} | " +
                   $"Members: {MemberCount} | " +
                   $"CreatedBy: {CreatedByUsername ?? CreatedBy.ToString()}";
        }
    }
}