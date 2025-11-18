-- ==============================================
-- Entity-Relationship Diagram (ERD) 
-- Team Management System Database Schema
-- ==============================================

-- ==> Create Database
CREATE DATABASE TeamManagementDB;
GO

USE TeamManagementDB;
GO

-- ==============================================
-- Table: Users
-- Purpose: Store user information (Admin or Regular User)
-- ==============================================
-- ==> Added: Users table
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    UserRole NVARCHAR(50) NOT NULL, -- 'Admin' or 'User'
    CreatedDate DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);

-- ==============================================
-- Table: Activities
-- Purpose: Store activity/project information
-- ==============================================
-- ==> Added: Activities table
CREATE TABLE Activities (
    ActivityId INT PRIMARY KEY IDENTITY(1,1),
    ActivityName NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    StartDate DATETIME,
    EndDate DATETIME,
    CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedBy INT FOREIGN KEY REFERENCES Users(UserId),
    IsActive BIT DEFAULT 1
);

-- ==============================================
-- Table: Tasks
-- Purpose: Store task information assigned to users
-- ==============================================
-- ==> Added: Tasks table
CREATE TABLE Tasks (
    TaskId INT PRIMARY KEY IDENTITY(1,1),
    TaskName NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    ActivityId INT FOREIGN KEY REFERENCES Activities(ActivityId),
    AssignedToUserId INT FOREIGN KEY REFERENCES Users(UserId),
    TaskStatus NVARCHAR(50) NOT NULL, -- 'Pending', 'InProgress', 'Completed', 'Cancelled'
    Priority NVARCHAR(50), -- 'Low', 'Medium', 'High'
    DueDate DATETIME,
    CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedBy INT FOREIGN KEY REFERENCES Users(UserId),
    LastModifiedDate DATETIME,
    LastModifiedBy INT,
    IsActive BIT DEFAULT 1
);

-- ==============================================
-- Insert Sample Data for Testing
-- ==============================================

-- ==> Added: Sample users (1 Admin, 2 Regular Users)
INSERT INTO Users (Username, FullName, Email, UserRole) VALUES
('admin', 'Admin User', 'admin@teammanagement.com', 'Admin'),
('john.doe', 'John Doe', 'john.doe@teammanagement.com', 'User'),
('jane.smith', 'Jane Smith', 'jane.smith@teammanagement.com', 'User');

-- ==> Added: Sample activities
INSERT INTO Activities (ActivityName, Description, StartDate, EndDate, CreatedBy) VALUES
('Website Development', 'Develop company website', '2025-01-01', '2025-06-30', 1),
('Mobile App', 'Create mobile application', '2025-02-01', '2025-08-31', 1),
('Marketing Campaign', 'Q1 Marketing campaign', '2025-01-15', '2025-03-31', 1);

-- ==> Added: Sample tasks
INSERT INTO Tasks (TaskName, Description, ActivityId, AssignedToUserId, TaskStatus, Priority, DueDate, CreatedBy) VALUES
('Design Homepage', 'Create homepage mockup and design', 1, 2, 'Pending', 'High', '2025-01-15', 1),
('Setup Database', 'Configure database structure', 1, 2, 'InProgress', 'High', '2025-01-10', 1),
('API Development', 'Develop REST APIs', 1, 3, 'Pending', 'Medium', '2025-02-01', 1),
('UI Design', 'Design mobile app interface', 2, 3, 'InProgress', 'High', '2025-02-15', 1),
('Social Media Posts', 'Create social media content', 3, 2, 'Pending', 'Low', '2025-01-20', 1);

GO

-- ==============================================
-- Create Indexes for Performance
-- ==============================================
-- ==> Added: Index on AssignedToUserId for faster task retrieval
CREATE INDEX IX_Tasks_AssignedToUserId ON Tasks(AssignedToUserId);

-- ==> Added: Index on TaskStatus for filtering tasks by status
CREATE INDEX IX_Tasks_Status ON Tasks(TaskStatus);

-- ==> Added: Index on ActivityId for activity-related queries
CREATE INDEX IX_Tasks_ActivityId ON Tasks(ActivityId);

GO
