-- ==============================================
-- Team Management System - Database Schema
-- ==============================================

USE master;
GO

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TeamManagementDB')
BEGIN
    CREATE DATABASE TeamManagementDB;
END
GO

USE TeamManagementDB;
GO

-- ==============================================
-- Table: Users
-- Stores user information with role (Admin/User)
-- ==============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        UserId INT PRIMARY KEY IDENTITY(1,1),
        Username NVARCHAR(100) NOT NULL UNIQUE,
        FullName NVARCHAR(200) NOT NULL,
        Email NVARCHAR(200) NOT NULL,
        Role NVARCHAR(50) NOT NULL CHECK (Role IN ('Admin', 'User')), -- ==> Admin can create/assign tasks, User can view
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        CONSTRAINT CK_Email CHECK (Email LIKE '%@%')
    );
END
GO

-- ==============================================
-- Table: Activities
-- Stores activity/project information
-- ==============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Activities')
BEGIN
    CREATE TABLE Activities (
        ActivityId INT PRIMARY KEY IDENTITY(1,1),
        ActivityName NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX),
        StartDate DATETIME,
        EndDate DATETIME,
        CreatedBy INT NOT NULL, -- ==> Reference to User who created this activity
        CreatedDate DATETIME DEFAULT GETDATE(),
        IsActive BIT DEFAULT 1,
        CONSTRAINT FK_Activities_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(UserId)
    );
END
GO

-- ==============================================
-- Table: Tasks
-- Stores task information with status and assignment
-- ==============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tasks')
BEGIN
    CREATE TABLE Tasks (
        TaskId INT PRIMARY KEY IDENTITY(1,1),
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX),
        ActivityId INT NOT NULL, -- ==> Each task belongs to an activity
        AssignedToUserId INT, -- ==> User assigned to this task (nullable for unassigned tasks)
        Status NVARCHAR(50) NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending', 'InProgress', 'Completed', 'OnHold')),
        Priority NVARCHAR(20) DEFAULT 'Medium' CHECK (Priority IN ('Low', 'Medium', 'High', 'Critical')),
        DueDate DATETIME,
        CreatedBy INT NOT NULL, -- ==> Admin who created this task
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME DEFAULT GETDATE(),
        CompletedDate DATETIME NULL,
        CONSTRAINT FK_Tasks_Activity FOREIGN KEY (ActivityId) REFERENCES Activities(ActivityId),
        CONSTRAINT FK_Tasks_AssignedTo FOREIGN KEY (AssignedToUserId) REFERENCES Users(UserId),
        CONSTRAINT FK_Tasks_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(UserId)
    );
END
GO

-- ==============================================
-- Insert Sample Data
-- ==============================================

-- Insert Users (1 Admin, 2 Regular Users)
IF NOT EXISTS (SELECT * FROM Users)
BEGIN
    INSERT INTO Users (Username, FullName, Email, Role) VALUES
    ('admin', 'Admin User', 'admin@company.com', 'Admin'),
    ('esraa.hosam', 'Esraa Hosam', 'esraa.hosam@company.com', 'User'),
    ('ahmed.hosam', 'Ahmed Hosam', 'ahmed.hosam@company.com', 'User');
END
GO

-- Insert Sample Activities
IF NOT EXISTS (SELECT * FROM Activities)
BEGIN
    INSERT INTO Activities (ActivityName, Description, StartDate, EndDate, CreatedBy) VALUES
    ('Website Redesign', 'Complete redesign of company website', GETDATE(), DATEADD(MONTH, 3, GETDATE()), 1),
    ('Mobile App Development', 'Develop iOS and Android mobile applications', GETDATE(), DATEADD(MONTH, 6, GETDATE()), 1),
    ('Database Migration', 'Migrate legacy database to new system', GETDATE(), DATEADD(MONTH, 2, GETDATE()), 1);
END
GO

-- Insert Sample Tasks
IF NOT EXISTS (SELECT * FROM Tasks)
BEGIN
    INSERT INTO Tasks (Title, Description, ActivityId, AssignedToUserId, Status, Priority, DueDate, CreatedBy) VALUES
    ('Create Wireframes', 'Design initial wireframes for homepage', 1, 2, 'InProgress', 'High', DATEADD(DAY, 7, GETDATE()), 1),
    ('Setup Development Environment', 'Configure development servers and tools', 2, 2, 'Completed', 'High', DATEADD(DAY, -2, GETDATE()), 1),
    ('API Design', 'Design RESTful API endpoints', 2, 3, 'Pending', 'Medium', DATEADD(DAY, 14, GETDATE()), 1),
    ('Database Schema Design', 'Create entity relationship diagram', 3, 3, 'InProgress', 'Critical', DATEADD(DAY, 5, GETDATE()), 1),
    ('User Testing', 'Conduct user acceptance testing', 1, NULL, 'Pending', 'Low', DATEADD(DAY, 30, GETDATE()), 1);
END
GO

-- ==============================================
-- Create Index for Better Performance
-- ==============================================
CREATE NONCLUSTERED INDEX IX_Tasks_AssignedToUserId ON Tasks(AssignedToUserId);
CREATE NONCLUSTERED INDEX IX_Tasks_Status ON Tasks(Status);
CREATE NONCLUSTERED INDEX IX_Tasks_ActivityId ON Tasks(ActivityId);
GO

-- ==============================================
-- Stored Procedure: Get Tasks by User
-- ==============================================
CREATE OR ALTER PROCEDURE sp_GetTasksByUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        t.TaskId,
        t.Title,
        t.Description,
        t.Status,
        t.Priority,
        t.DueDate,
        t.CreatedDate,
        t.UpdatedDate,
        a.ActivityName,
        a.ActivityId,
        u.FullName as AssignedToName,
        creator.FullName as CreatedByName
    FROM Tasks t
    INNER JOIN Activities a ON t.ActivityId = a.ActivityId
    LEFT JOIN Users u ON t.AssignedToUserId = u.UserId
    INNER JOIN Users creator ON t.CreatedBy = creator.UserId
    WHERE t.AssignedToUserId = @UserId
    ORDER BY t.DueDate ASC, t.Priority DESC;
END
GO

-- ==============================================
-- Stored Procedure: Get Tasks by Status
-- ==============================================
CREATE OR ALTER PROCEDURE sp_GetTasksByStatus
    @Status NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        t.TaskId,
        t.Title,
        t.Description,
        t.Status,
        t.Priority,
        t.DueDate,
        t.CreatedDate,
        t.UpdatedDate,
        a.ActivityName,
        a.ActivityId,
        u.FullName as AssignedToName,
        u.UserId as AssignedToUserId,
        creator.FullName as CreatedByName
    FROM Tasks t
    INNER JOIN Activities a ON t.ActivityId = a.ActivityId
    LEFT JOIN Users u ON t.AssignedToUserId = u.UserId
    INNER JOIN Users creator ON t.CreatedBy = creator.UserId
    WHERE t.Status = @Status
    ORDER BY t.DueDate ASC, t.Priority DESC;
END
GO

PRINT 'Database schema created successfully!';
GO
