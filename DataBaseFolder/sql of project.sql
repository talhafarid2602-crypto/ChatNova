-- CREATE DATABASE
-- ================================
CREATE DATABASE ChatAppDB;
GO

-- ================================
-- USE DATABASE
-- ================================
USE ChatAppDB;
GO

-- ================================
-- TABLE: Users
-- ================================
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Passwordhash NVARCHAR(256) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    ProfilePic NVARCHAR(255) NULL,
    IsOnline BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    LastSeen DATETIME NULL
);
GO

-- ================================
-- TABLE: Conversations
-- ================================
CREATE TABLE Conversations (
    ConversationID INT IDENTITY(1,1) PRIMARY KEY,
    ConversationName NVARCHAR(100) NULL,
    IsGroup BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NOT NULL,
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserID)
);
GO

-- ================================
-- TABLE: ConversationMembers
-- ================================
CREATE TABLE ConversationMembers (
    MemberID INT IDENTITY(1,1) PRIMARY KEY,
    ConversationID INT NOT NULL,
    UserID INT NOT NULL,
    JoinedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (ConversationID) REFERENCES Conversations(ConversationID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    UNIQUE (ConversationID, UserID)
);
GO

-- ================================
-- TABLE: Messages
-- ================================
CREATE TABLE Messages (
    MessageID INT IDENTITY(1,1) PRIMARY KEY,
    ConversationID INT NOT NULL,
    SenderID INT NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    SentAt DATETIME NOT NULL DEFAULT GETDATE(),
    IsRead BIT NOT NULL DEFAULT 0,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (ConversationID) REFERENCES Conversations(ConversationID),
    FOREIGN KEY (SenderID) REFERENCES Users(UserID)
);
GO
CREATE PROCEDURE sp_GetMessages
    @ConversationID INT
AS
BEGIN
    SELECT 
        m.MessageID,
        m.ConversationID,
        m.SenderID,
        m.Content,
        m.SentAt,
        m.IsDeleted,
        u.Username AS SenderName
    FROM Messages m
    INNER JOIN Users u ON m.SenderID = u.UserID
    WHERE m.ConversationID = @ConversationID
      AND m.IsDeleted = 0
    ORDER BY m.SentAt ASC;
END

















-- ================================
-- INSERT SAMPLE USERS
-- Password = "password123" (SHA256 hashed)
-- ================================
INSERT INTO Users (Username, Email, PasswordHash, FullName, IsOnline)
VALUES
('Talha', 'Talha@student.com', '75K3eLr+dx6JJFuJ7LwIpEpOFmwGZZkRiB84PURz6U8=', 'Talha Fareed', 0),
('Subhan',   'Subhan@student.com',   '75K3eLr+dx6JJFuJ7LwIpEpOFmwGZZkRiB84PURz6U8=', 'Subhan Hameed', 0);
GO
select PasswordHash from users

select* from Users;
Alter Table Users
Add PasswordHash NVarchar(255);
Alter Table Users
Drop Column Password;


INSERT INTO Users (Username, Email, PasswordHash, FullName, IsOnline)
VALUES
('Taha', 'Taha@student.com', '75K3eLr+dx6JJFuJ7LwIpEpOFmwGZZkRiB84PURz6U8=', 'Taha Fareed', 0);

GO