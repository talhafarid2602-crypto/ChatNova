using ChatNova.Data;
using ChatNova.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatNova.Models;
namespace ChatNova.Services
{
    /// <summary>
    /// Send, Load, Edit, Delete messages
    /// All MessageText is AES encrypted before INSERT
    /// All MessageText is AES decrypted after SELECT
    /// </summary>
    public class MessageService
    {
        // -------------------------------------------------------
        // SEND MESSAGE
        // -------------------------------------------------------
        public bool SendMessage(int chatId, int senderId, string plainText)
        {
            // 1. Check sender is not blocked by anyone in this chat
            if (IsBlockedInChat(chatId, senderId))
                throw new Exception("You are blocked and cannot send messages.");

            // 2. Encrypt before storing
            string encrypted = EncryptionHelper.Encrypt(plainText);

            string query = @"
                INSERT INTO IMessages (ChatId, SenderId, MessageText)
                VALUES (@ChatId, @SenderId, @MessageText);
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ChatId",      chatId),
                new SqlParameter("@SenderId",    senderId),
                new SqlParameter("@MessageText", encrypted)
            };

            object result = DatabaseConnection.ExecuteScalar(query, parameters);
            int newMessageId = Convert.ToInt32(result);

            // 3. Insert Sent status for all participants except sender
            InsertMessageStatusForParticipants(chatId, newMessageId, senderId);

            return newMessageId > 0;
        }

        // -------------------------------------------------------
        // LOAD MESSAGES FOR CHAT
        // -------------------------------------------------------
        public List<IMessage> GetMessages(int chatId)
        {
            string query = @"
                SELECT m.MessageId, m.ChatId, m.SenderId,
                       m.MessageText, m.SentAt, m.EditedAt, m.IsDeleted,
                       u.Username AS SenderName
                FROM IMessages m
                INNER JOIN IUsers u ON m.SenderId = u.UserId
                WHERE m.ChatId = @ChatId
                ORDER BY m.SentAt ASC";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ChatId", chatId)
            };

            var messages = new List<IMessage>();

            var table = DatabaseConnection.ExecuteDataTable(query, parameters);

            foreach (System.Data.DataRow row in table.Rows)
            {
                bool isDeleted = (bool)row["IsDeleted"];

                // Decrypt message text
                string decryptedText = isDeleted
                    ? "🚫 This message was deleted"
                    : EncryptionHelper.Decrypt(row["MessageText"].ToString());

                messages.Add(new IMessage
                {
                    MessageId = (int)row["MessageId"],
                    ChatId = (int)row["ChatId"],
                    SenderId = (int)row["SenderId"],
                    MessageText = decryptedText,
                    SentAt = (DateTime)row["SentAt"],
                    EditedAt = row["EditedAt"] as DateTime?,
                    IsDeleted = isDeleted,
                    SenderName = row["SenderName"].ToString(),
                    IsMyMessage = (int)row["SenderId"] == SessionManager.CurrentUserId
                });
            }

            return messages;
        }

        // -------------------------------------------------------
        // EDIT MESSAGE
        // -------------------------------------------------------
        public bool EditMessage(int messageId, int senderId, string newPlainText)
        {
            string encrypted = EncryptionHelper.Encrypt(newPlainText);

            string query = @"
                UPDATE IMessages
                SET MessageText = @MessageText,
                    EditedAt    = GETDATE()
                WHERE MessageId = @MessageId
                  AND SenderId  = @SenderId
                  AND IsDeleted = 0";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MessageText", encrypted),
                new SqlParameter("@MessageId",   messageId),
                new SqlParameter("@SenderId",    senderId)
            };

            return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // -------------------------------------------------------
        // DELETE MESSAGE (soft delete)
        // -------------------------------------------------------
        public bool DeleteMessage(int messageId, int senderId)
        {
            string query = @"
                UPDATE IMessages
                SET IsDeleted = 1
                WHERE MessageId = @MessageId
                  AND SenderId  = @SenderId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MessageId", messageId),
                new SqlParameter("@SenderId",  senderId)
            };

            return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // -------------------------------------------------------
        // UPDATE MESSAGE STATUS (Delivered / Seen)
        // -------------------------------------------------------
        public bool UpdateMessageStatus(int messageId, int userId, string status)
        {
            string query = @"
                UPDATE IMessageStatus
                SET Status    = @Status,
                    UpdatedAt = GETDATE()
                WHERE MessageId = @MessageId
                  AND UserId    = @UserId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Status",    status),
                new SqlParameter("@MessageId", messageId),
                new SqlParameter("@UserId",    userId)
            };

            return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // -------------------------------------------------------
        // PRIVATE: Insert Sent status for all chat participants
        // -------------------------------------------------------
        private void InsertMessageStatusForParticipants(
            int chatId, int messageId, int senderId)
        {
            string query = @"
                INSERT INTO IMessageStatus (MessageId, UserId, Status)
                SELECT @MessageId, UserId, 'Sent'
                FROM ChatParticipants
                WHERE ChatId = @ChatId
                  AND UserId != @SenderId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MessageId", messageId),
                new SqlParameter("@ChatId",    chatId),
                new SqlParameter("@SenderId",  senderId)
            };

            DatabaseConnection.ExecuteNonQuery(query, parameters);
        }

        // -------------------------------------------------------
        // PRIVATE: Check if sender is blocked by anyone in chat
        // -------------------------------------------------------
        private bool IsBlockedInChat(int chatId, int senderId)
        {
            string query = @"
                SELECT COUNT(1)
                FROM BlockedContacts bc
                INNER JOIN ChatParticipants cp
                    ON bc.UserId = cp.UserId
                WHERE cp.ChatId        = @ChatId
                  AND bc.BlockedUserId = @SenderId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ChatId",   chatId),
                new SqlParameter("@SenderId", senderId)
            };

            object result = DatabaseConnection.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result) > 0;
        }
    }
}