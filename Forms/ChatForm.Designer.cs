namespace ChatNova.Forms
{
    partial class ChatForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblCurrentUser;
        private System.Windows.Forms.Label lblContactName;
        private System.Windows.Forms.ListBox lstConversations;
        private System.Windows.Forms.Panel pnlMessages;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnBlock;
        private System.Windows.Forms.Button btnNewGroup;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblCurrentUser = new System.Windows.Forms.Label();
            this.lblContactName = new System.Windows.Forms.Label();
            this.lstConversations = new System.Windows.Forms.ListBox();
            this.pnlMessages = new System.Windows.Forms.Panel();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnBlock = new System.Windows.Forms.Button();
            this.btnNewGroup = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // ── lblCurrentUser ────────────────────────────────────
            this.lblCurrentUser.Location = new System.Drawing.Point(10, 10);
            this.lblCurrentUser.Size = new System.Drawing.Size(200, 20);
            this.lblCurrentUser.Text = "User";
            this.lblCurrentUser.Name = "lblCurrentUser";

            // ── lblContactName ────────────────────────────────────
            this.lblContactName.Location = new System.Drawing.Point(220, 10);
            this.lblContactName.Size = new System.Drawing.Size(300, 20);
            this.lblContactName.Text = "Select a chat";
            this.lblContactName.Name = "lblContactName";
            this.lblContactName.Font = new System.Drawing.Font("Segoe UI", 10,
                                                System.Drawing.FontStyle.Bold);

            // ── lstConversations ──────────────────────────────────
            this.lstConversations.Location = new System.Drawing.Point(10, 40);
            this.lstConversations.Size = new System.Drawing.Size(200, 400);
            this.lstConversations.Name = "lstConversations";
            this.lstConversations.ItemHeight = 20;
            this.lstConversations.SelectedIndexChanged +=
                new System.EventHandler(this.lstConversations_SelectedIndexChanged);

            // ── pnlMessages ───────────────────────────────────────
            this.pnlMessages.AutoScroll = true;
            this.pnlMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMessages.Location = new System.Drawing.Point(220, 40);
            this.pnlMessages.Size = new System.Drawing.Size(650, 400);
            this.pnlMessages.Name = "pnlMessages";

            // ── txtMessage ────────────────────────────────────────
            this.txtMessage.Enabled = false;
            this.txtMessage.Location = new System.Drawing.Point(220, 450);
            this.txtMessage.Size = new System.Drawing.Size(500, 26);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Font = new System.Drawing.Font("Segoe UI", 10);
            this.txtMessage.KeyDown +=
                new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyDown);

            // ── btnSend ───────────────────────────────────────────
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(730, 450);
            this.btnSend.Size = new System.Drawing.Size(140, 26);
            this.btnSend.Name = "btnSend";
            this.btnSend.Text = "Send";
            this.btnSend.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Click +=
                new System.EventHandler(this.btnSend_Click);

            // ── txtSearch (placeholder done manually) ─────────────
            this.txtSearch.Location = new System.Drawing.Point(10, 450);
            this.txtSearch.Size = new System.Drawing.Size(120, 26);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Text = "Search users...";
            this.txtSearch.ForeColor = System.Drawing.Color.Gray;
            this.txtSearch.GotFocus += (s, e) =>
            {
                if (this.txtSearch.Text == "Search users...")
                {
                    this.txtSearch.Text = "";
                    this.txtSearch.ForeColor = System.Drawing.Color.Black;
                }
            };
            this.txtSearch.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(this.txtSearch.Text))
                {
                    this.txtSearch.Text = "Search users...";
                    this.txtSearch.ForeColor = System.Drawing.Color.Gray;
                }
            };

            // ── btnSearch ─────────────────────────────────────────
            this.btnSearch.Location = new System.Drawing.Point(135, 450);
            this.btnSearch.Size = new System.Drawing.Size(75, 26);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Text = "Search";
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Click +=
                new System.EventHandler(this.btnSearch_Click);

            // ── btnNewGroup ───────────────────────────────────────
            this.btnNewGroup.Location = new System.Drawing.Point(10, 485);
            this.btnNewGroup.Size = new System.Drawing.Size(200, 26);
            this.btnNewGroup.Name = "btnNewGroup";
            this.btnNewGroup.Text = "New Group";
            this.btnNewGroup.BackColor = System.Drawing.Color.FromArgb(0, 150, 136);
            this.btnNewGroup.ForeColor = System.Drawing.Color.White;
            this.btnNewGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewGroup.Click +=
                new System.EventHandler(this.btnNewGroup_Click);

            // ── btnBlock ──────────────────────────────────────────
            this.btnBlock.Location = new System.Drawing.Point(220, 485);
            this.btnBlock.Size = new System.Drawing.Size(150, 26);
            this.btnBlock.Name = "btnBlock";
            this.btnBlock.Text = "Block User";
            this.btnBlock.BackColor = System.Drawing.Color.FromArgb(211, 47, 47);
            this.btnBlock.ForeColor = System.Drawing.Color.White;
            this.btnBlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlock.Click +=
                new System.EventHandler(this.btnBlock_Click);

            // ── btnLogout ─────────────────────────────────────────
            this.btnLogout.Location = new System.Drawing.Point(10, 520);
            this.btnLogout.Size = new System.Drawing.Size(200, 26);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Text = "Logout";
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(96, 96, 96);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Click +=
                new System.EventHandler(this.btnLogout_Click);

            // ── ChatForm ──────────────────────────────────────────
            this.ClientSize = new System.Drawing.Size(900, 570);
            this.Name = "ChatForm";
            this.Text = "ChatNova";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing +=
                new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.Load +=
                new System.EventHandler(this.ChatForm_Load);

            this.Controls.Add(this.lblCurrentUser);
            this.Controls.Add(this.lblContactName);
            this.Controls.Add(this.lstConversations);
            this.Controls.Add(this.pnlMessages);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnNewGroup);
            this.Controls.Add(this.btnBlock);
            this.Controls.Add(this.btnLogout);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}