using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using TeamManagementSystem.Hubs;
using TeamManagementSystem.Models;
using TeamManagementSystem.Models.ViewModels;
using TeamManagementSystem.Repositories;

namespace TeamManagementSystem.Services
{
    /// <summary>
    /// ==> Added: Implementation of ITaskService
    /// Handles business logic for task operations
    /// Integrates with repository for data access and SignalR for real-time updates
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IHubContext _hubContext;

        /// <summary>
        /// Constructor - receives dependencies through injection
        /// </summary>
        /// <param name="taskRepository">Repository for data access</param>
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            // ==> Get SignalR hub context for broadcasting notifications
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<TaskNotificationHub>();
        }

        /// <summary>
        /// ==> Get all tasks and convert to DTOs
        /// </summary>
        public List<TaskDTO> GetAllTasks()
        {
            try
            {
                var tasks = _taskRepository.GetAllTasks();
                return tasks.Select(t => MapToDTO(t)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAllTasks: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Get a single task by ID and convert to DTO
        /// </summary>
        public TaskDTO GetTaskById(int taskId)
        {
            try
            {
                var task = _taskRepository.GetTaskById(taskId);
                return task != null ? MapToDTO(task) : null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetTaskById: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Get all tasks for a specific user
        /// </summary>
        public List<TaskDTO> GetUserTasks(int userId)
        {
            try
            {
                var tasks = _taskRepository.GetTasksByUserId(userId);
                return tasks.Select(t => MapToDTO(t)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetUserTasks: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Get all tasks with a specific status
        /// </summary>
        public List<TaskDTO> GetTasksByStatus(string status)
        {
            try
            {
                var tasks = _taskRepository.GetTasksByStatus(status);
                return tasks.Select(t => MapToDTO(t)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetTasksByStatus: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Create a new task and broadcast notification
        /// This is where real-time functionality is triggered
        /// </summary>
        public TaskDTO CreateTask(CreateTaskViewModel model, int createdByUserId)
        {
            try
            {
                // ==> Map ViewModel to Entity
                var task = new Task
                {
                    TaskName = model.TaskName,
                    Description = model.Description,
                    ActivityId = model.ActivityId,
                    AssignedToUserId = model.AssignedToUserId,
                    Priority = model.Priority ?? "Medium",
                    DueDate = model.DueDate,
                    CreatedBy = createdByUserId,
                    TaskStatus = "Pending", // Default status
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                // ==> Save to database
                var createdTask = _taskRepository.CreateTask(task);
                
                // ==> Convert to DTO
                var taskDTO = MapToDTO(createdTask);

                // ==> REAL-TIME UPDATE: Broadcast to all connected clients via SignalR
                _hubContext.Clients.All.taskCreated(taskDTO);

                return taskDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in CreateTask: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Update task status and broadcast notification
        /// Real-time update triggers here
        /// </summary>
        public bool UpdateTaskStatus(int taskId, string newStatus, int modifiedByUserId)
        {
            try
            {
                // ==> Update in database
                var success = _taskRepository.UpdateTaskStatus(taskId, newStatus);

                if (success)
                {
                    // ==> Get updated task for broadcasting
                    var updatedTask = _taskRepository.GetTaskById(taskId);
                    if (updatedTask != null)
                    {
                        var taskDTO = MapToDTO(updatedTask);
                        
                        // ==> REAL-TIME UPDATE: Broadcast status change to all clients
                        _hubContext.Clients.All.taskStatusChanged(taskId, newStatus);
                        
                        // ==> Also send the full updated task object
                        _hubContext.Clients.All.taskUpdated(taskDTO);
                    }
                }

                return success;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateTaskStatus: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Assign task to a user and broadcast notification
        /// </summary>
        public TaskDTO AssignTask(int taskId, int userId, int modifiedByUserId)
        {
            try
            {
                // ==> Get existing task
                var task = _taskRepository.GetTaskById(taskId);
                if (task == null)
                {
                    throw new Exception("Task not found");
                }

                // ==> Update assignment
                task.AssignedToUserId = userId;
                task.LastModifiedDate = DateTime.Now;
                task.LastModifiedBy = modifiedByUserId;

                // ==> Save changes
                var updatedTask = _taskRepository.UpdateTask(task);
                var taskDTO = MapToDTO(updatedTask);

                // ==> REAL-TIME UPDATE: Broadcast to all clients
                _hubContext.Clients.All.taskUpdated(taskDTO);

                return taskDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AssignTask: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Delete task and broadcast notification
        /// </summary>
        public bool DeleteTask(int taskId)
        {
            try
            {
                var success = _taskRepository.DeleteTask(taskId);
                
                if (success)
                {
                    // ==> REAL-TIME UPDATE: Broadcast deletion to all clients
                    _hubContext.Clients.All.taskDeleted(taskId);
                }

                return success;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in DeleteTask: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Get all users for dropdown lists
        /// </summary>
        public List<UserDTO> GetAllUsers()
        {
            try
            {
                var users = _taskRepository.GetAllUsers();
                return users.Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    FullName = u.FullName,
                    Email = u.Email,
                    UserRole = u.UserRole
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAllUsers: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Get all activities for dropdown lists
        /// </summary>
        public List<ActivityDTO> GetAllActivities()
        {
            try
            {
                var activities = _taskRepository.GetAllActivities();
                return activities.Select(a => new ActivityDTO
                {
                    ActivityId = a.ActivityId,
                    ActivityName = a.ActivityName,
                    Description = a.Description,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAllActivities: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ==> Helper method to map Task entity to TaskDTO
        /// Flattens the object structure for easier client consumption
        /// </summary>
        private TaskDTO MapToDTO(Task task)
        {
            return new TaskDTO
            {
                TaskId = task.TaskId,
                TaskName = task.TaskName,
                Description = task.Description,
                ActivityId = task.ActivityId,
                ActivityName = task.Activity?.ActivityName ?? "N/A",
                AssignedToUserId = task.AssignedToUserId,
                AssignedToUserName = task.AssignedToUser?.FullName ?? "Unassigned",
                TaskStatus = task.TaskStatus,
                Priority = task.Priority,
                DueDate = task.DueDate,
                CreatedDate = task.CreatedDate
            };
        }
    }
}
