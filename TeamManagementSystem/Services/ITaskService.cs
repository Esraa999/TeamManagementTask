using System.Collections.Generic;
using TeamManagementSystem.Models.ViewModels;

namespace TeamManagementSystem.Services
{
    /// <summary>
    /// ==> Added: Interface for Task Service
    /// Defines business logic operations for task management
    /// Separates business logic from data access and presentation layers
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Get all tasks as DTOs
        /// </summary>
        /// <returns>List of TaskDTO objects</returns>
        List<TaskDTO> GetAllTasks();

        /// <summary>
        /// Get a specific task by ID
        /// </summary>
        /// <param name="taskId">Task ID</param>
        /// <returns>TaskDTO object or null</returns>
        TaskDTO GetTaskById(int taskId);

        /// <summary>
        /// Get all tasks assigned to a specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of TaskDTO objects</returns>
        List<TaskDTO> GetUserTasks(int userId);

        /// <summary>
        /// Get all tasks with a specific status
        /// </summary>
        /// <param name="status">Task status</param>
        /// <returns>List of TaskDTO objects</returns>
        List<TaskDTO> GetTasksByStatus(string status);

        /// <summary>
        /// Create a new task and notify all clients
        /// </summary>
        /// <param name="model">CreateTaskViewModel with task data</param>
        /// <param name="createdByUserId">User ID who created the task</param>
        /// <returns>Created TaskDTO</returns>
        TaskDTO CreateTask(CreateTaskViewModel model, int createdByUserId);

        /// <summary>
        /// Update task status and notify all clients
        /// </summary>
        /// <param name="taskId">Task ID to update</param>
        /// <param name="newStatus">New status value</param>
        /// <param name="modifiedByUserId">User ID who modified the task</param>
        /// <returns>True if successful</returns>
        bool UpdateTaskStatus(int taskId, string newStatus, int modifiedByUserId);

        /// <summary>
        /// Assign or reassign a task to a user
        /// </summary>
        /// <param name="taskId">Task ID</param>
        /// <param name="userId">User ID to assign to</param>
        /// <param name="modifiedByUserId">User ID who made the assignment</param>
        /// <returns>Updated TaskDTO</returns>
        TaskDTO AssignTask(int taskId, int userId, int modifiedByUserId);

        /// <summary>
        /// Delete a task (soft delete)
        /// </summary>
        /// <param name="taskId">Task ID to delete</param>
        /// <returns>True if successful</returns>
        bool DeleteTask(int taskId);

        /// <summary>
        /// Get all users for dropdowns
        /// </summary>
        /// <returns>List of UserDTO objects</returns>
        List<UserDTO> GetAllUsers();

        /// <summary>
        /// Get all activities for dropdowns
        /// </summary>
        /// <returns>List of ActivityDTO objects</returns>
        List<ActivityDTO> GetAllActivities();
    }
}
