--CTEATE DATABASE:
create Database newChatNovaDB;
GO

--USE DATABASE:
Use newChatNovaDB:
GO

--TABLE:--USERS--:


CREATE TABLE IUsers (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20) UNIQUE,
    Email NVARCHAR(100) UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE()
);

--TABLE:__CHATS__:

CREATE TABLE Chats (
    ChatId INT IDENTITY(1,1) PRIMARY KEY,
    ChatType NVARCHAR(20) NOT NULL, -- 'Private' or 'Group'
    CreatedAt DATETIME DEFAULT GETDATE()
);

--TABLE:__CHATPARTICIPANTS__:

CREATE TABLE ChatParticipants (
    ChatParticipantId INT IDENTITY(1,1) PRIMARY KEY,
    ChatId INT NOT NULL,
    UserId INT NOT NULL,
    JoinedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (ChatId) REFERENCES Chats(ChatId),
    FOREIGN KEY (UserId) REFERENCES IUsers(UserId),

    CONSTRAINT UQ_Chat_User UNIQUE (ChatId, UserId)
);

--TABLE:__MESSAGES__:

CREATE TABLE IMessages (
    MessageId INT IDENTITY(1,1) PRIMARY KEY,
    ChatId INT NOT NULL,
    SenderId INT NOT NULL,
    MessageText NVARCHAR(MAX) NULL,
    SentAt DATETIME DEFAULT GETDATE(),
    EditedAt DATETIME NULL,
    IsDeleted BIT DEFAULT 0,

    FOREIGN KEY (ChatId) REFERENCES Chats(ChatId),
    FOREIGN KEY (SenderId) REFERENCES IUsers(UserId)
);

--TABLE:__MESSAGE STATUS__:


CREATE TABLE IMessageStatus (
    MessageStatusId INT IDENTITY(1,1) PRIMARY KEY,
    MessageId INT NOT NULL,
    UserId INT NOT NULL,
    Status NVARCHAR(20) NOT NULL, 
    -- Sent / Delivered / Seen

    UpdatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (MessageId) REFERENCES IMessages(MessageId),
    FOREIGN KEY (UserId) REFERENCES IUsers(UserId),

    CONSTRAINT UQ_Message_User UNIQUE (MessageId, UserId)
);

--TABLE:__NOTIFICATIONS__:


CREATE TABLE Notifications (
    NotificationId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Title NVARCHAR(100),
    Message NVARCHAR(255),
    IsRead BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (UserId) REFERENCES IUsers(UserId)
);

--TABLE:__CONTACTS__:

CREATE TABLE Contacts (
    ContactId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    ContactUserId INT NOT NULL,
    NickName NVARCHAR(50) NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (UserId) REFERENCES IUsers(UserId),
    FOREIGN KEY (ContactUserId) REFERENCES IUsers(UserId),

    CONSTRAINT UQ_User_Contact UNIQUE (UserId, ContactUserId)
);

--TABLE:__BLOCKED CONTACTS__:

CREATE TABLE BlockedContacts (
    BlockId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    BlockedUserId INT NOT NULL,
    BlockedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (UserId) REFERENCES IUsers(UserId),
    FOREIGN KEY (BlockedUserId) REFERENCES IUsers(UserId),

    CONSTRAINT UQ_Block UNIQUE (UserId, BlockedUserId)
);

--TABLE:__GROUPS__:


CREATE TABLE Groups (
    GroupId INT IDENTITY(1,1) PRIMARY KEY,
    ChatId INT NOT NULL,
    GroupName NVARCHAR(100) NOT NULL,
    GroupDescription NVARCHAR(255) NULL,
    CreatedBy INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (ChatId) REFERENCES Chats(ChatId),
    FOREIGN KEY (CreatedBy) REFERENCES IUsers(UserId)
);


