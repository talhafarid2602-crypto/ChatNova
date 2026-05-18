using ChatNova.Data;
using ChatNova.Helpers;
using ChatNova.Models; // assuming Conversation, Message, User models are here
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNova.Services
{
    /// <summary>
    /// Create chats, load chat list, add participants
    /// </summary>
    public class ChatService
    {
        // -------------------------------------------------------
        // CREATE PRIVATE CHAT
        // -------------------------------------------------------
        public int CreatePrivateChat(int userId1, int userId2)
        {
            // Check if private chat already exists between these two
            string checkQuery = @"
                SELECT cp1.ChatId
                FROM ChatParticipants cp1
                INNER JOIN ChatParticipants cp2
                    ON cp1.ChatId = cp2.ChatId
                INNER JOIN Chats c
                    ON c.ChatId = cp1.ChatId
                WHERE cp1.UserId  = @User1
                  AND cp2.UserId  = @User2
                  AND c.ChatType  = 'Private'";

            SqlParameter[] checkParams =
            {
                new SqlParameter("@User1", userId1),
                new SqlParameter("@User2", userId2)
            };

            object existing = DatabaseConnection.ExecuteScalar(checkQuery, checkParams);
            if (existing != null) return Convert.ToInt32(existing);

            // Create new Private chat
            string insertChat = @"
                INSERT INTO Chats (ChatType) VALUES ('Private');
                SELECT SCOPE_IDENTITY();";

            int chatId = Convert.ToInt32(
                DatabaseConnection.ExecuteScalar(insertChat));

            // Add both participants
            AddParticipant(chatId, userId1);
            AddParticipant(chatId, userId2);

            return chatId;
        }

        // -------------------------------------------------------
        // CREATE GROUP CHAT
        // -------------------------------------------------------
        public int CreateGroupChat(string groupName, string description,
                                   int createdBy, List<int> memberIds)
        {
            // 1. Create Chat row
            string insertChat = @"
                INSERT INTO Chats (ChatType) VALUES ('Group');
                SELECT SCOPE_IDENTITY();";

            int chatId = Convert.ToInt32(
                DatabaseConnection.ExecuteScalar(insertChat));

            // 2. Create Group row
            string insertGroup = @"
                INSERT INTO Groups (ChatId, GroupName, GroupDescription, CreatedBy)
                VALUES (@ChatId, @GroupName, @Description, @CreatedBy)";

            SqlParameter[] groupParams =
            {
                new SqlParameter("@ChatId",      chatId),
                new SqlParameter("@GroupName",   groupName),
                new SqlParameter("@Description", (object)description ?? DBNull.Value),
                new SqlParameter("@CreatedBy",   createdBy)
            };

            DatabaseConnection.ExecuteNonQuery(insertGroup, groupParams);

            // 3. Add creator + all members
            AddParticipant(chatId, createdBy);
            foreach (int memberId in memberIds)
            {
                if (memberId != createdBy)
                    AddParticipant(chatId, memberId);
            }

            return chatId;
        }

        // -------------------------------------------------------
        // GET ALL CHATS FOR CURRENT USER
        // -------------------------------------------------------
        public List<Chat> GetUserChats(int userId)
        {
            string query = @"
                SELECT
                    c.ChatId,
                    c.ChatType,
                    c.CreatedAt,
                    g.GroupName,
                    -- Other user name for private chats
                    (SELECT u.Username
                     FROM ChatParticipants cp2
                     INNER JOIN IUsers u ON cp2.UserId = u.UserId
                     WHERE cp2.ChatId = c.ChatId
                       AND cp2.UserId != @UserId) AS OtherPersonName,
                    -- Last message
                    (SELECT TOP 1 MessageText
                     FROM IMessages
                     WHERE ChatId    = c.ChatId
                       AND IsDeleted = 0
                     ORDER BY SentAt DESC) AS LastMessage,
                    -- Last message time
                    (SELECT TOP 1 SentAt
                     FROM IMessages
                     WHERE ChatId = c.ChatId
                     ORDER BY SentAt DESC) AS LastMessageTime,
                    -- Unread count
                    (SELECT COUNT(1)
                     FROM IMessages m
                     INNER JOIN IMessageStatus ms
                         ON m.MessageId = ms.MessageId
                     WHERE m.ChatId    = c.ChatId
                       AND ms.UserId   = @UserId
                       AND ms.Status  != 'Seen') AS UnreadCount
                FROM Chats c
                INNER JOIN ChatParticipants cp ON c.ChatId = cp.ChatId
                LEFT JOIN Groups g ON c.ChatId = g.ChatId
                WHERE cp.UserId = @UserId
                ORDER BY LastMessageTime DESC";

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", userId)
            };

            var chats = new List<Chat>();
            var table = DatabaseConnection.ExecuteDataTable(query, parameters);

            foreach (System.Data.DataRow row in table.Rows)
            {
                // Decrypt last message preview
                string encryptedLast = row["LastMessage"] as string;
                string decryptedLast = string.IsNullOrEmpty(encryptedLast)
                    ? null
                    : EncryptionHelper.Decrypt(encryptedLast);

                chats.Add(new Chat
                {
                    ChatId = (int)row["ChatId"],
                    ChatType = row["ChatType"].ToString(),
                    CreatedAt = (DateTime)row["CreatedAt"],
                    GroupName = row["GroupName"] as string,
                    OtherPersonName = row["OtherPersonName"] as string,
                    LastMessage = decryptedLast,
                    LastMessageTime = row["LastMessageTime"] as DateTime?,
                    UnreadCount = Convert.ToInt32(row["UnreadCount"])
                });
            }

            return chats;
        }

        // -------------------------------------------------------
        // ADD PARTICIPANT TO CHAT
        // -------------------------------------------------------
        public void AddParticipant(int chatId, int userId)
        {
            string query = @"
                IF NOT EXISTS (
                    SELECT 1 FROM ChatParticipants
                    WHERE ChatId = @ChatId AND UserId = @UserId)
                INSERT INTO ChatParticipants (ChatId, UserId)
                VALUES (@ChatId, @UserId)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ChatId",  chatId),
                new SqlParameter("@UserId",  userId)
            };

            DatabaseConnection.ExecuteNonQuery(query, parameters);
        }

        // -------------------------------------------------------
        // REMOVE PARTICIPANT FROM GROUP
        // -------------------------------------------------------
        public bool RemoveParticipant(int chatId, int userId)
        {
            string query = @"
                DELETE FROM ChatParticipants
                WHERE ChatId = @ChatId AND UserId = @UserId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ChatId", chatId),
                new SqlParameter("@UserId", userId)
            };

            return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}