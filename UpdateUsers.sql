-- ==============================================
-- Update Users to Esraa Hosam and Ahmed Hosam
-- ==============================================

USE TeamManagementDB;
GO

-- Update user 2 (John Doe) to Esraa Hosam
UPDATE Users
SET Username = 'esraa.hosam',
    FullName = 'Esraa Hosam',
    Email = 'esraa.hosam@company.com'
WHERE Username = 'john.doe';

-- Update user 3 (Jane Smith) to Ahmed Hosam
UPDATE Users
SET Username = 'ahmed.hosam',
    FullName = 'Ahmed Hosam',
    Email = 'ahmed.hosam@company.com'
WHERE Username = 'jane.smith';

-- Display updated users
SELECT UserId, Username, FullName, Email, Role, IsActive
FROM Users
ORDER BY UserId;

PRINT 'Users updated successfully!';
GO
