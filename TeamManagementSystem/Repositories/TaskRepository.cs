using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TeamManagementSystem.Data;
using TeamManagementSystem.Models;

namespace TeamManagementSystem.Repositories
{
    /// <summary>
    /// ==> Added: Implementation of ITaskRepository
    /// Handles all database operations for Task, User, and Activity entities
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly TeamManagementContext _context;

        /// <summary>
        /// Constructor - receives DbContext through dependency injection
        /// </summary>
        /// <param name="context">Database context</param>
        public TaskRepository(TeamManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// ==> Get all active tasks with related entities (Activity, AssignedUser)
        /// Includes navigation properties for complete data
        /// </summary>
        public List<Task> GetAllTasks()
        {
            try
            {
                // Include related entities to avoid lazy loading issues
                return _context.Tasks
                    .Include(t => t.Activity)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.Creator)
                    .Where(t => t.IsActive)
                    .OrderByDescending(t => t.CreatedDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log exception (in production, use proper logging framework)
                throw new Exception("Error retrieving tasks: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Get a single task by ID with related entities
        /// Returns null if not found
        /// </summary>
        public Task GetTaskById(int taskId)
        {
            try
            {
                return _context.Tasks
                    .Include(t => t.Activity)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.Creator)
                    .FirstOrDefault(t => t.TaskId == taskId && t.IsActive);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving task: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Get all tasks assigned to a specific user
        /// Used for User Dashboard to show their tasks
        /// </summary>
        public List<Task> GetTasksByUserId(int userId)
        {
            try
            {
                return _context.Tasks
                    .Include(t => t.Activity)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.Creator)
                    .Where(t => t.AssignedToUserId == userId && t.IsActive)
                    .OrderByDescending(t => t.CreatedDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user tasks: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Get all tasks with a specific status
        /// Useful for filtering tasks by status (Pending, InProgress, etc.)
        /// </summary>
        public List<Task> GetTasksByStatus(string status)
        {
            try
            {
                return _context.Tasks
                    .Include(t => t.Activity)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.Creator)
                    .Where(t => t.TaskStatus == status && t.IsActive)
                    .OrderByDescending(t => t.CreatedDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving tasks by status: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Create a new task in the database
        /// Sets default values and saves to database
        /// </summary>
        public Task CreateTask(Task task)
        {
            try
            {
                // Set default values
                task.CreatedDate = DateTime.Now;
                task.IsActive = true;
                
                // Set default status if not provided
                if (string.IsNullOrEmpty(task.TaskStatus))
                {
                    task.TaskStatus = "Pending";
                }

                // Add to context and save
                _context.Tasks.Add(task);
                _context.SaveChanges();

                // Reload with navigation properties
                return GetTaskById(task.TaskId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating task: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Update an existing task
        /// Updates all modifiable fields
        /// </summary>
        public Task UpdateTask(Task task)
        {
            try
            {
                // Find existing task
                var existingTask = _context.Tasks.Find(task.TaskId);
                if (existingTask == null)
                {
                    throw new Exception("Task not found");
                }

                // Update fields
                existingTask.TaskName = task.TaskName;
                existingTask.Description = task.Description;
                existingTask.ActivityId = task.ActivityId;
                existingTask.AssignedToUserId = task.AssignedToUserId;
                existingTask.TaskStatus = task.TaskStatus;
                existingTask.Priority = task.Priority;
                existingTask.DueDate = task.DueDate;
                existingTask.LastModifiedDate = DateTime.Now;
                existingTask.LastModifiedBy = task.LastModifiedBy;

                // Mark as modified and save
                _context.Entry(existingTask).State = EntityState.Modified;
                _context.SaveChanges();

                // Reload with navigation properties
                return GetTaskById(task.TaskId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating task: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Update only the status of a task
        /// Lightweight operation for status changes
        /// </summary>
        public bool UpdateTaskStatus(int taskId, string newStatus)
        {
            try
            {
                var task = _context.Tasks.Find(taskId);
                if (task == null)
                {
                    return false;
                }

                // Update status and modification date
                task.TaskStatus = newStatus;
                task.LastModifiedDate = DateTime.Now;

                _context.Entry(task).State = EntityState.Modified;
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating task status: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Soft delete a task (set IsActive = false)
        /// Does not physically delete from database
        /// </summary>
        public bool DeleteTask(int taskId)
        {
            try
            {
                var task = _context.Tasks.Find(taskId);
                if (task == null)
                {
                    return false;
                }

                // Soft delete - just mark as inactive
                task.IsActive = false;
                task.LastModifiedDate = DateTime.Now;

                _context.Entry(task).State = EntityState.Modified;
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting task: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Get all active users
        /// Used for dropdown lists in task assignment
        /// </summary>
        public List<User> GetAllUsers()
        {
            try
            {
                return _context.Users
                    .Where(u => u.IsActive)
                    .OrderBy(u => u.FullName)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving users: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Get all active activities
        /// Used for dropdown lists in task creation
        /// </summary>
        public List<Activity> GetAllActivities()
        {
            try
            {
                return _context.Activities
                    .Where(a => a.IsActive)
                    .OrderBy(a => a.ActivityName)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving activities: " + ex.Message, ex);
            }
        }
    }
}
