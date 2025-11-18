# Team Management System - .NET MVC Assignment

## Overview
This is a complete Team Management System built with ASP.NET MVC 5, Entity Framework 6, SignalR 2.x, and Vue.js 2. The system allows administrators to create and manage tasks, assign them to users, and provides real-time updates across all connected clients without page refresh.

## Technology Stack

### Backend
- **ASP.NET MVC 5** (.NET Framework 4.7.2)
- **Entity Framework 6** (Code First approach)
- **SignalR 2.x** (Real-time communication via WebSockets)
- **SQL Server** (Database)

### Frontend
- **Vue.js 2.6** (JavaScript framework)
- **Bootstrap 4** (UI framework)
- **jQuery 3.6** (Required for SignalR)
- **Font Awesome 5** (Icons)

## Features

### Admin Dashboard (`/Admin/Index`)
- ✅ View all tasks in a card-based grid layout
- ✅ Create new tasks with full details
- ✅ Assign tasks to users
- ✅ Update task status in real-time
- ✅ Filter tasks by status and priority
- ✅ Search functionality
- ✅ Real-time synchronization with SignalR

### User Dashboard (`/User/Index`)
- ✅ View only tasks assigned to the logged-in user
- ✅ See task details (Activity, Priority, Due Date, Status)
- ✅ Real-time updates when new tasks are assigned
- ✅ Real-time status changes
- ✅ Browser notifications for new assignments
- ✅ Task statistics dashboard

### Real-time Features
- ✅ Automatic UI updates without page refresh
- ✅ Instant synchronization across all clients
- ✅ Visual feedback for updates (animations)
- ✅ Connection status indicator

## Prerequisites

1. **Visual Studio 2019 or later** with ASP.NET and web development workload
2. **SQL Server 2016 or later** (Express Edition is fine)
3. **.NET Framework 4.7.2 or later**
4. **IIS Express** (comes with Visual Studio)

## Database Setup

### Step 1: Update Connection String
The connection string is already configured in `Web.config`:

```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=ESRAAPC;Database=TeamManagementDB;User ID=sa;Password=12345678;Trusted_Connection=true;MultipleActiveResultSets=true" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Important:** Update the connection string to match your SQL Server instance:
- Change `Server=ESRAAPC` to your server name
- Update `User ID` and `Password` if needed
- You can also use Windows Authentication by removing User ID/Password and keeping `Trusted_Connection=true`

### Step 2: Create Database and Tables
Execute the SQL script located at `/Database/ERD_Schema.sql` in SQL Server Management Studio (SSMS):

1. Open SSMS and connect to your SQL Server instance
2. Click "New Query"
3. Copy the contents of `ERD_Schema.sql`
4. Execute the script (F5)

This will:
- Create the `TeamManagementDB` database
- Create tables: `Users`, `Activities`, `Tasks`
- Insert sample data (1 admin user, 2 regular users, 3 activities, 5 sample tasks)

### Sample Users Created:
- **Admin User:** Username: `admin`, UserID: 1
- **Regular User 1:** Username: `john.doe`, UserID: 2 (John Doe)
- **Regular User 2:** Username: `jane.smith`, UserID: 3 (Jane Smith)

## Project Setup

### Step 1: Open Solution
1. Extract the ZIP file
2. Navigate to the `TeamManagementSystem` folder
3. Open `TeamManagementSystem.sln` in Visual Studio

### Step 2: Restore NuGet Packages
1. Right-click on the solution in Solution Explorer
2. Select "Restore NuGet Packages"
3. Wait for all packages to download and install

### Step 3: Build Solution
1. Press `Ctrl+Shift+B` or go to Build > Build Solution
2. Ensure there are no build errors

### Step 4: Run the Application
1. Press `F5` or click the "IIS Express" button
2. The application will open in your default browser
3. You should see the Home page with navigation options

## Using the Application

### Home Page (`/`)
- Starting point with links to both dashboards
- Shows technology stack information
- Provides navigation to Admin and User dashboards

### Admin Dashboard (`/Admin/Index`)
**URL:** `http://localhost:[port]/Admin/Index`

**Features:**
1. **Create Task:** Click "Create New Task" button
   - Fill in task details (Name, Description, Activity, Assigned User, Priority, Due Date)
   - Click "Create Task"
   - Task appears instantly on the page via SignalR

2. **View All Tasks:** See all tasks in a grid layout
   - Each task shows: Name, Description, Activity, Assigned User, Status, Priority, Due Date
   - Color-coded priority badges (Red=High, Yellow=Medium, Green=Low)

3. **Update Status:** Change task status using the dropdown
   - Updates instantly across all connected clients
   - Changes reflect in user dashboard immediately

4. **Filter & Search:**
   - Filter by Status (Pending, InProgress, Completed, Cancelled)
   - Filter by Priority (High, Medium, Low)
   - Search by task name, description, activity, or user name
   - Reset button to clear all filters

5. **Real-time Indicator:**
   - Green "Connected" badge shows SignalR is active
   - All updates sync automatically

### User Dashboard (`/User/Index`)
**URL:** `http://localhost:[port]/User/Index` (Shows tasks for UserID 2 - John Doe)

**Features:**
1. **View Assigned Tasks:** See only tasks assigned to you
   - Task cards with full details
   - Color-coded borders by status

2. **Statistics:** Top dashboard shows:
   - Total tasks assigned
   - Pending tasks count
   - In Progress tasks count
   - Completed tasks count

3. **Real-time Updates:**
   - New tasks appear instantly when admin assigns them
   - "Just Assigned!" badge on new tasks
   - Status changes reflect immediately
   - Browser notifications for new assignments (after permission granted)

4. **Filter & Search:**
   - Filter by status
   - Search tasks
   - Reset filters

### Testing Real-time Features

**Test Scenario 1: Create Task in Admin, See in User**
1. Open Admin dashboard in one browser tab
2. Open User dashboard in another tab (or different browser)
3. In Admin: Create a new task and assign to John Doe (UserID 2)
4. Watch User dashboard: Task appears automatically without refresh!

**Test Scenario 2: Update Status**
1. Keep both Admin and User dashboards open
2. In Admin: Change status of a task from "Pending" to "InProgress"
3. Watch User dashboard: Status updates instantly!

**Test Scenario 3: Multiple Admins**
1. Open Admin dashboard in two different browsers
2. Create a task in one browser
3. Watch the task appear in the other browser automatically!

## Project Structure

```
TeamManagementSystem/
│
├── Controllers/
│   ├── HomeController.cs          # Home page controller
│   ├── AdminController.cs         # Admin dashboard controller
│   ├── UserController.cs          # User dashboard controller
│   └── TaskController.cs          # API endpoints for task operations
│
├── Models/
│   ├── User.cs                    # User entity
│   ├── Activity.cs                # Activity entity
│   ├── Task.cs                    # Task entity
│   └── ViewModels/
│       └── DTOs.cs                # Data Transfer Objects
│
├── Data/
│   └── TeamManagementContext.cs   # Entity Framework DbContext
│
├── Repositories/
│   ├── ITaskRepository.cs         # Repository interface
│   └── TaskRepository.cs          # Repository implementation
│
├── Services/
│   ├── ITaskService.cs            # Service interface
│   └── TaskService.cs             # Service implementation with SignalR
│
├── Hubs/
│   └── TaskNotificationHub.cs     # SignalR Hub for real-time updates
│
├── Views/
│   ├── Home/
│   │   └── Index.cshtml           # Home page with navigation
│   ├── Admin/
│   │   └── Index.cshtml           # Admin dashboard with Vue.js
│   └── User/
│       └── Index.cshtml           # User dashboard with Vue.js
│
├── App_Start/
│   ├── RouteConfig.cs             # URL routing configuration
│   ├── BundleConfig.cs            # CSS/JS bundling configuration
│   └── FilterConfig.cs            # Global filters configuration
│
├── Database/
│   └── ERD_Schema.sql             # Database creation script
│
├── Documentation/
│   └── ClassDiagram.txt           # Class diagram documentation
│
├── Global.asax.cs                 # Application startup
├── Startup.cs                     # OWIN startup (SignalR configuration)
├── Web.config                     # Application configuration
└── packages.config                # NuGet packages
```

## API Endpoints

### Task Management APIs

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/Task/GetAllTasks` | Get all tasks |
| GET | `/Task/GetTaskById?id={id}` | Get task by ID |
| GET | `/Task/GetUserTasks?userId={userId}` | Get tasks for specific user |
| GET | `/Task/GetTasksByStatus?status={status}` | Get tasks by status |
| POST | `/Task/CreateTask` | Create new task |
| POST | `/Task/UpdateTaskStatus` | Update task status |
| POST | `/Task/AssignTask` | Assign task to user |
| POST | `/Task/DeleteTask?id={id}` | Delete task |
| GET | `/Task/GetUsers` | Get all users |
| GET | `/Task/GetActivities` | Get all activities |

### Request/Response Examples

**Create Task (POST):**
```json
{
  "taskName": "Design Homepage",
  "description": "Create mockup for homepage",
  "activityId": 1,
  "assignedToUserId": 2,
  "priority": "High",
  "dueDate": "2025-12-31"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Task created successfully",
  "data": {
    "taskId": 6,
    "taskName": "Design Homepage",
    "activityName": "Website Development",
    "assignedToUserName": "John Doe",
    "taskStatus": "Pending",
    ...
  }
}
```

## SignalR Hub Methods

### Server to Client Methods:
- `taskCreated(taskDTO)` - Broadcast when new task is created
- `taskUpdated(taskDTO)` - Broadcast when task is updated
- `taskStatusChanged(taskId, newStatus)` - Broadcast status change
- `taskDeleted(taskId)` - Broadcast when task is deleted

## Architecture & Design Patterns

### Layered Architecture:
1. **Presentation Layer:** Controllers and Views (MVC pattern with Vue.js)
2. **Business Logic Layer:** Services (TaskService)
3. **Data Access Layer:** Repositories (TaskRepository)
4. **Database Layer:** SQL Server with Entity Framework

### Design Patterns Used:
- **Repository Pattern:** Abstracts data access logic
- **Service Layer Pattern:** Encapsulates business logic
- **Dependency Injection:** Loose coupling between layers
- **DTO Pattern:** Data transfer between layers
- **Observer Pattern:** SignalR for real-time updates

## Troubleshooting

### Issue: SignalR not connecting
**Solution:**
- Ensure OWIN startup is configured correctly in `Startup.cs`
- Check browser console for errors
- Verify SignalR scripts are loading (check Network tab in DevTools)
- Make sure `jquery.signalR` is loaded before `signalr/hubs`

### Issue: Database connection error
**Solution:**
- Verify SQL Server is running
- Check connection string in `Web.config`
- Ensure database `TeamManagementDB` exists
- Run the `ERD_Schema.sql` script

### Issue: Real-time updates not working
**Solution:**
- Check SignalR connection status in browser console
- Verify `TaskNotificationHub.cs` is in the Hubs folder
- Make sure `app.MapSignalR()` is called in `Startup.cs`
- Check if any firewall is blocking WebSocket connections

### Issue: NuGet packages not restoring
**Solution:**
- Right-click solution > Restore NuGet Packages
- Clean and rebuild solution
- Check internet connection
- Try deleting `packages` folder and restoring again

## Assignment Deliverables Checklist

✅ **1. Entity-Relationship Diagram (ERD)**
- Located in: `/Database/ERD_Schema.sql`
- Shows Users, Activities, Tasks tables with relationships

✅ **2. Class Diagram**
- Located in: `/Documentation/ClassDiagram.txt`
- Shows all classes, interfaces, relationships

✅ **3. API Implementation**
- Task CRUD operations
- User and Activity retrieval
- Status updates
- Task assignment

✅ **4. Web Page Development**
- Admin dashboard with Vue.js
- User dashboard with Vue.js
- Real-time updates via SignalR
- No page refresh required

✅ **5. Additional Features**
- Comprehensive error handling
- Commented code throughout
- Responsive design (Bootstrap 4)
- Filter and search functionality
- Visual feedback for updates
- Browser notifications

## Code Comments
All code files include comprehensive comments explaining:
- Purpose of each class/method
- Logic and algorithms used
- Design decisions
- SignalR integration points
- Real-time update triggers

Comments are marked with `==>` for easy identification.

## Performance Considerations
- Database indexes on frequently queried columns
- SignalR for efficient real-time updates (WebSocket protocol)
- Entity Framework lazy loading for related entities
- Bundling and minification for CSS/JS
- Asynchronous JavaScript operations

## Security Considerations (Production Recommendations)
- Implement proper authentication (ASP.NET Identity)
- Add authorization filters to controllers
- Use HTTPS for SignalR connections
- Validate all user inputs
- Implement CSRF protection
- Add rate limiting for API endpoints
- Store connection strings securely

## Future Enhancements (If Time Permits)
- User authentication system
- Task comments/notes
- File attachments
- Email notifications
- Activity logs
- Dashboard charts/analytics
- Mobile app (using same API)

## Contact & Support
For questions or issues:
- Review the code comments (marked with `==>`)
- Check the class diagram in `/Documentation/ClassDiagram.txt`
- Examine the database schema in `/Database/ERD_Schema.sql`

---

## Quick Start Summary

1. **Database:** Execute `/Database/ERD_Schema.sql` in SSMS
2. **Connection:** Update connection string in `Web.config`
3. **Build:** Open solution in Visual Studio and build
4. **Run:** Press F5
5. **Test:** Open `/Admin/Index` and `/User/Index` in separate tabs
6. **Enjoy:** Create tasks in admin and watch them appear in user dashboard!

---

**Assignment completed with all required features and additional enhancements!**
