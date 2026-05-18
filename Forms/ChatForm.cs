using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ChatNova.Helpers;
using ChatNova.Models;
using ChatNova.Services;

namespace ChatNova.Forms
{
    public partial class ChatForm : Form
    {
        // ── Services ──────────────────────────────────────────────
        private readonly MessageService _messageService;
        private readonly ChatService _chatService;
        private readonly ContactService _contactService;
        private readonly NotificationService _notificationService;

        // ── State ─────────────────────────────────────────────────
        private readonly int _currentUserId;
        private int _currentChatId = 0;
        private List<Chat> _userChats = new List<Chat>();

        // ── Polling ───────────────────────────────────────────────
        private readonly Timer _pollTimer;
        private int _lastMessageId = 0;

        // ── Constructor ───────────────────────────────────────────
        public ChatForm(int currentUserId)
        {
            InitializeComponent();

            _currentUserId = currentUserId;
            _messageService = new MessageService();
            _chatService = new ChatService();
            _contactService = new ContactService();
            _notificationService = new NotificationService();

            _pollTimer = new Timer { Interval = 3000 };
            _pollTimer.Tick += PollTimer_Tick;
        }

        // =========================================================
        // FORM LOAD
        // =========================================================
        private void ChatForm_Load(object sender, EventArgs e)
        {
            lblCurrentUser.Text = "Logged in as: @" + SessionManager.CurrentUsername;
            txtMessage.Enabled = false;
            btnSend.Enabled = false;

            LoadChatList();
            LoadNotificationCount();
        }

        // =========================================================
        // LOAD CHAT LIST INTO lstConversations
        // =========================================================
        private void LoadChatList()
        {
            try
            {
                _userChats = _chatService.GetUserChats(_currentUserId);
                lstConversations.Items.Clear();

                foreach (var chat in _userChats)
                {
                    string preview = chat.GetMessagePreview();
                    string badge = chat.UnreadCount > 0
                                        ? $" ({chat.UnreadCount})"
                                        : "";
                    string display = $"{chat.GetDisplayName()}{badge}\n{preview}";
                    lstConversations.Items.Add(display);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading chats: " + ex.Message);
            }
        }

        // =========================================================
        // CHAT SELECTED
        // =========================================================
        private void lstConversations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstConversations.SelectedIndex < 0) return;

            _pollTimer.Stop();

            var selectedChat = _userChats[lstConversations.SelectedIndex];
            _currentChatId = selectedChat.ChatId;
            lblContactName.Text = selectedChat.GetDisplayName();

            txtMessage.Enabled = true;
            btnSend.Enabled = true;

            LoadAllMessages();
            _pollTimer.Start();
        }

        // =========================================================
        // LOAD ALL MESSAGES FOR SELECTED CHAT
        // =========================================================
        private void LoadAllMessages()
        {
            try
            {
                pnlMessages.Controls.Clear();
                _pollTimer.Stop();
                _lastMessageId = 0;

                var messages = _messageService.GetMessages(_currentChatId);

                foreach (var msg in messages)
                    AddMessageBubble(msg);
                if (messages.Count > 0)
                _lastMessageId = messages[messages.Count - 1].MessageId;

                // Mark all as Seen
                foreach (var msg in messages)
                {
                    if (!msg.IsMyMessage)
                        _messageService.UpdateMessageStatus(
                            msg.MessageId, _currentUserId,
                            IMessageStatus.STATUS_SEEN);
                }

                ScrollToBottom();
                LoadChatList(); // refresh unread counts
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading messages: " + ex.Message);
            }
        }

        // =========================================================
        // SEND MESSAGE
        // =========================================================
        private void btnSend_Click(object sender, EventArgs e)
        {
            string plainText = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(plainText)) return;

            if (_currentChatId <= 0)
            {
                MessageBox.Show("Please select a chat first.");
                return;
            }

            try
            {
                // Check if blocked before sending
                if (_contactService.IsBlocked(_currentUserId, GetOtherUserId()))
                {
                    MessageBox.Show(
                        "You have blocked this user. Unblock to send messages.",
                        "Blocked",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                bool sent = _messageService.SendMessage(
                                _currentChatId, _currentUserId, plainText);

                if (sent)
                {
                    _lastMessageId = 0;
                    LoadAllMessages();
                    txtMessage.Clear();
                    txtMessage.Focus();

                    // Notify other participants
                    _notificationService.CreateNotification(
                        GetOtherUserId(),
                        "New Message",
                        $"@{SessionManager.CurrentUsername}: {plainText.Substring(0, Math.Min(50, plainText.Length))}..."
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send: " + ex.Message);
            }
        }

        // =========================================================
        // ENTER KEY SENDS
        // =========================================================
        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                e.SuppressKeyPress = true;
                btnSend_Click(sender, e);
            }
        }

        // =========================================================
        // POLL FOR NEW MESSAGES EVERY 3s
        // =========================================================
        private void PollTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var messages = _messageService.GetMessages(_currentChatId);
                bool hasNew = false;

                foreach (var msg in messages)
                {
                    if (msg.MessageId > _lastMessageId)
                    {
                        
                            AddMessageBubble(msg);
                            hasNew = true;
                        
                        _lastMessageId = msg.MessageId;
                        // Mark as Seen immediately
                        if (!msg.IsMyMessage)
                            _messageService.UpdateMessageStatus(
                                msg.MessageId, _currentUserId,
                                IMessageStatus.STATUS_SEEN);
                    }
                }

                if (hasNew)
                {
                    ScrollToBottom();
                    LoadChatList();
                    LoadNotificationCount();
                }
            }
            catch { /* silent — polling should never crash app */ }
        }

        // =========================================================
        // SEARCH USERS / CHATS
        // =========================================================
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string term = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(term)||term =="Search users...")
            {
                LoadChatList();
                return;
            }

            try
            {
                // Search contacts by username/email/phone
                var results = _contactService.SearchUsers(term);

                if (results.Count == 0)
                {
                    MessageBox.Show("No users found for: " + term);
                    return;
                }

                // Show results — let user pick one to start chat
                var menu = new ContextMenuStrip();
                foreach (var user in results)
                {
                    var u = user; // capture for lambda
                    menu.Items.Add(
                        $"@{u.Username} ({u.Email ?? u.PhoneNumber})",
                        null,
                        (s, ev) => StartOrOpenChat(u)
                    );
                }
                menu.Show(btnSearch, new Point(0, btnSearch.Height));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message);
            }
        }

        // =========================================================
        // START OR OPEN PRIVATE CHAT WITH USER
        // =========================================================
        private void StartOrOpenChat(IUser user)
        {
            try
            {
                int chatId = _chatService.CreatePrivateChat(_currentUserId, user.UserId);
                LoadChatList();

                // Select that chat in the list
                for (int i = 0; i < _userChats.Count; i++)
                {
                    if (_userChats[i].ChatId == chatId)
                    {
                        lstConversations.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open chat: " + ex.Message);
            }
        }

        // =========================================================
        // NEW GROUP CHAT BUTTON
        // =========================================================
        private void btnNewGroup_Click(object sender, EventArgs e)
        {
            string groupName = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter group name:", "New Group", "");

            if (string.IsNullOrWhiteSpace(groupName)) return;

            try
            {
                // Get contacts to add as members
                var contacts = _contactService.GetContacts(_currentUserId);
                if (contacts.Count == 0)
                {
                    MessageBox.Show("Add contacts first before creating a group.");
                    return;
                }

                var memberIds = new List<int>();
                foreach (var c in contacts)
                    memberIds.Add(c.ContactUserId);

                int chatId = _chatService.CreateGroupChat(
                    groupName, null, _currentUserId, memberIds);

                MessageBox.Show($"Group '{groupName}' created!");
                LoadChatList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating group: " + ex.Message);
            }
        }

        // =========================================================
        // BLOCK / UNBLOCK CURRENT CHAT USER
        // =========================================================
        private void btnBlock_Click(object sender, EventArgs e)
        {
            if (_currentChatId <= 0) return;

            int otherUserId = GetOtherUserId();
            if (otherUserId <= 0) return;

            try
            {
                bool isBlocked = _contactService.IsBlocked(_currentUserId, otherUserId);

                if (isBlocked)
                {
                    _contactService.UnblockUser(_currentUserId, otherUserId);
                    MessageBox.Show("User unblocked.");
                }
                else
                {
                    var confirm = MessageBox.Show(
                        "Block this user?", "Block",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (confirm == DialogResult.Yes)
                    {
                        _contactService.BlockUser(_currentUserId, otherUserId);
                        MessageBox.Show("User blocked.");
                        txtMessage.Enabled = false;
                        btnSend.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // =========================================================
        // DELETE MESSAGE (right-click context menu)
        // =========================================================
        private void DeleteMessage_Click(object sender, EventArgs e, int messageId)
        {
            var confirm = MessageBox.Show(
                "Delete this message?", "Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _messageService.DeleteMessage(messageId, _currentUserId);
                    LoadAllMessages();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting: " + ex.Message);
                }
            }
        }

        // =========================================================
        // NOTIFICATIONS COUNT
        // =========================================================
        private void LoadNotificationCount()
        {
            try
            {
                int count = _notificationService.GetUnreadCount(_currentUserId);
                lblCurrentUser.Text = $"@{SessionManager.CurrentUsername}" +
                    (count > 0 ? $"  🔔 {count}" : "");
            }
            catch { }
        }

        // =========================================================
        // LOGOUT
        // =========================================================
        private void btnLogout_Click(object sender, EventArgs e)
        {
            _pollTimer.Stop();

            var confirm = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                SessionManager.Logout();
                new LoginForm().Show();
                this.Close();
            }
        }

        // =========================================================
        // BUILD MESSAGE BUBBLE
        // =========================================================
        private void AddMessageBubble(IMessage msg)
        {
            var bubble = new Panel
            {
                AutoSize = true,
                MaximumSize = new Size(400, 0),
                Padding = new Padding(10),
                BackColor = msg.IsMyMessage
                    ? Color.FromArgb(0, 120, 215)
                    : Color.FromArgb(240, 240, 240)
            };

            // Message text
            var lblText = new Label
            {
                Text = msg.IsDeleted
                                ? "🚫 This message was deleted"
                                : msg.MessageText,
                AutoSize = true,
                MaximumSize = new Size(380, 0),
                ForeColor = msg.IsMyMessage ? Color.White : Color.Black,
                Font = new Font("Segoe UI", 10),
                Cursor = msg.IsMyMessage && !msg.IsDeleted
                                ? Cursors.Hand
                                : Cursors.Default
            };

            // Time + edited label
            string timeText = msg.GetFormattedTime() +
                              (msg.IsEdited() ? "  ✏️" : "") +
                              "  " + GetStatusIcon(msg.MessageId);

            var lblTime = new Label
            {
                Text = timeText,
                AutoSize = true,
                ForeColor = msg.IsMyMessage ? Color.LightBlue : Color.Gray,
                Font = new Font("Segoe UI", 7)
            };

            bubble.Controls.Add(lblText);
            bubble.Controls.Add(lblTime);
            lblTime.Top = lblText.Bottom + 2;

            // Right-click to delete/edit own messages
            if (msg.IsMyMessage && !msg.IsDeleted)
            {
                int msgId = msg.MessageId;
                string plain = msg.MessageText;

                var ctx = new ContextMenuStrip();

                ctx.Items.Add("✏️ Edit", null, (s, ev) =>
                {
                    string newText = Microsoft.VisualBasic.Interaction.InputBox(
                        "Edit message:", "Edit", plain);

                    if (!string.IsNullOrWhiteSpace(newText))
                    {
                        _messageService.EditMessage(msgId, _currentUserId, newText);
                        LoadAllMessages();
                    }
                });

                ctx.Items.Add("🗑️ Delete", null, (s, ev) =>
                    DeleteMessage_Click(s, ev, msgId));

                bubble.ContextMenuStrip = ctx;
                lblText.ContextMenuStrip = ctx;
            }

            var wrapper = new Panel
            {
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = msg.IsMyMessage
                    ? new Padding(100, 2, 5, 2)
                    : new Padding(5, 2, 100, 2)
            };

            wrapper.Controls.Add(bubble);
            pnlMessages.Controls.Add(wrapper);
            wrapper.BringToFront();
        }

        // =========================================================
        // HELPERS
        // =========================================================

        /// <summary>Get other user ID in a private chat</summary>
        private int GetOtherUserId()
        {
            if (_currentChatId <= 0) return 0;

            var chat = _userChats.Find(c => c.ChatId == _currentChatId);
            if (chat == null || chat.IsGroup()) return 0;

            // Load from DB via participants
            string query = $@"
                SELECT UserId FROM ChatParticipants
                WHERE ChatId = {_currentChatId}
                  AND UserId != {_currentUserId}";

            var table = ChatNova.Data.DatabaseConnection.ExecuteDataTable(query);
            if (table.Rows.Count > 0)
                return Convert.ToInt32(table.Rows[0]["UserId"]);

            return 0;
        }

        /// <summary>Get tick icon for message status</summary>
        private string GetStatusIcon(int messageId)
        {
            try
            {
                string query = @"
                    SELECT Status FROM IMessageStatus
                    WHERE MessageId = @MessageId AND UserId != @SenderId";

                var param = new System.Data.SqlClient.SqlParameter[]
                {
                    new System.Data.SqlClient.SqlParameter("@MessageId", messageId),
                    new System.Data.SqlClient.SqlParameter("@SenderId",  _currentUserId)
                };

                var table = ChatNova.Data.DatabaseConnection.ExecuteDataTable(query, param);

                if (table.Rows.Count == 0) return "✓";

                bool allSeen = true;
                bool allDelivered = true;

                foreach (System.Data.DataRow row in table.Rows)
                {
                    string s = row["Status"].ToString();
                    if (s != IMessageStatus.STATUS_SEEN) allSeen = false;
                    if (s != IMessageStatus.STATUS_DELIVERED
                     && s != IMessageStatus.STATUS_SEEN) allDelivered = false;
                }

                if (allSeen) return "✓✓"; // show blue in UI
                if (allDelivered) return "✓✓";
                return "✓";
            }
            catch { return "✓"; }
        }

        private void ScrollToBottom()
        {
            pnlMessages.AutoScrollPosition =
                new Point(0, pnlMessages.DisplayRectangle.Height);
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _pollTimer.Stop();
            _pollTimer.Dispose();
        }
    }
}