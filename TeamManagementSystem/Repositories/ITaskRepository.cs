using System.Collections.Generic;
using TeamManagementSystem.Models;

namespace TeamManagementSystem.Repositories
{
    /// <summary>
    /// ==> Added: Interface for Task Repository
    /// Defines contract for data access operations on Task entity
    /// Follows Repository Pattern for separation of concerns
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Get all active tasks from database
        /// </summary>
        /// <returns>List of all tasks</returns>
        List<Task> GetAllTasks();

        /// <summary>
        /// Get a specific task by its ID
        /// </summary>
        /// <param name="taskId">Task ID to search for</param>
        /// <returns>Task object or null if not found</returns>
        Task GetTaskById(int taskId);

        /// <summary>
        /// Get all tasks assigned to a specific user
        /// </summary>
        /// <param name="userId">User ID to filter tasks</param>
        /// <returns>List of tasks assigned to the user</returns>
        List<Task> GetTasksByUserId(int userId);

        /// <summary>
        /// Get all tasks with a specific status
        /// </summary>
        /// <param name="status">Status to filter (e.g., "Pending", "InProgress", "Completed")</param>
        /// <returns>List of tasks with the specified status</returns>
        List<Task> GetTasksByStatus(string status);

        /// <summary>
        /// Create a new task in the database
        /// </summary>
        /// <param name="task">Task object to create</param>
        /// <returns>Created task with generated ID</returns>
        Task CreateTask(Task task);

        /// <summary>
        /// Update an existing task
        /// </summary>
        /// <param name="task">Task object with updated values</param>
        /// <returns>Updated task</returns>
        Task UpdateTask(Task task);

        /// <summary>
        /// Update only the status of a task
        /// </summary>
        /// <param name="taskId">Task ID to update</param>
        /// <param name="newStatus">New status value</param>
        /// <returns>True if successful, false otherwise</returns>
        bool UpdateTaskStatus(int taskId, string newStatus);

        /// <summary>
        /// Soft delete a task (set IsActive = false)
        /// </summary>
        /// <param name="taskId">Task ID to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        bool DeleteTask(int taskId);

        /// <summary>
        /// Get all active users from database
        /// </summary>
        /// <returns>List of users</returns>
        List<User> GetAllUsers();

        /// <summary>
        /// Get all active activities from database
        /// </summary>
        /// <returns>List of activities</returns>
        List<Activity> GetAllActivities();
    }
}
