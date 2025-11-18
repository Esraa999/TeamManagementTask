using System;
using System.Web.Mvc;
using TeamManagementSystem.Models.ViewModels;
using TeamManagementSystem.Services;

namespace TeamManagementSystem.Controllers
{
    /// <summary>
    /// ==> Added: API Controller for Task operations
    /// Handles HTTP requests for task management
    /// Returns JSON responses for AJAX calls from Vue.js frontend
    /// </summary>
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        /// <summary>
        /// Constructor - receives task service through dependency injection
        /// </summary>
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// ==> API: Get all tasks
        /// GET: /Task/GetAllTasks
        /// Used by admin dashboard to display all tasks
        /// </summary>
        /// <returns>JSON array of all tasks</returns>
        [HttpGet]
        public JsonResult GetAllTasks()
        {
            try
            {
                var tasks = _taskService.GetAllTasks();
                return Json(ApiResponse<object>.SuccessResponse(tasks), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ApiResponse<object>.ErrorResponse("Error retrieving tasks: " + ex.Message), 
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// ==> API: Get a single task by ID
        /// GET: /Task/GetTaskById?id=1
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>JSON object of the task</returns>
        [HttpGet]
        public JsonResult GetTaskById(int id)
        {
            try
            {
                var task = _taskService.GetTaskById(id);
                if (task == null)
                {
                    return Json(ApiResponse<object>.ErrorResponse("Task not found"), 
                        JsonRequestBehavior.AllowGet);
                }
                return Json(ApiResponse<object>.SuccessResponse(task), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ApiResponse<object>.ErrorResponse("Error retrieving task: " + ex.Message), 
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// ==> API: Get all tasks for a specific user
        /// GET: /Task/GetUserTasks?userId=2
        /// Used by user dashboard to show assigned tasks
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>JSON array of user's tasks</returns>
        [HttpGet]
        public JsonResult GetUserTasks(int userId)
        {
            try
            {
                var tasks = _taskService.GetUserTasks(userId);
                return Json(ApiResponse<object>.SuccessResponse(tasks), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ApiResponse<object>.ErrorResponse("Error retrieving user tasks: " + ex.Message), 
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// ==> API: Get tasks by status
        /// GET: /Task/GetTasksByStatus?status=Pending
        /// Useful for filtering tasks
        /// </summary>
        /// <param name="status">Task status (Pending, InProgress, Completed, Cancelled)</param>
        /// <returns>JSON array of tasks with specified status</returns>
        [HttpGet]
        public JsonResult GetTasksByStatus(string status)
        {
            try
            {
                var tasks = _taskService.GetTasksByStatus(status);
                return Json(ApiResponse<object>.SuccessResponse(tasks), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ApiResponse<object>.ErrorResponse("Error retrieving tasks by status: " + ex.Message), 
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// ==> API: Create a new task
        /// POST: /Task/CreateTask
        /// Accepts JSON payload with task details
        /// Triggers SignalR notification to all clients
        /// </summary>
        /// <param name="model">CreateTaskViewModel with task data</param>
        /// <returns>JSON object of created task</returns>
        [HttpPost]
        public JsonResult CreateTask(CreateTaskViewModel model)
        {
            try
            {
                // Validate model
                if (!ModelState.IsValid)
                {
                    return Json(ApiResponse<object>.ErrorResponse("Invalid task data"));
                }

                // In a real app, get the current user ID from session/authentication
                // For this demo, we'll use admin user (ID = 1)
                int currentUserId = 1;

                // Create task (this will also trigger SignalR notification)
                var createdTask = _taskService.CreateTask(model, currentUserId);

                return Json(ApiResponse<object>.SuccessResponse(createdTask, "Task created successfully"));
            }
            catch (Exception ex)
            {
                return Json(ApiResponse<object>.ErrorResponse("Error creating task: " + ex.Message));
            }
        }

        /// <summary>
        /// ==> API: Update task status
        /// POST: /Task/UpdateTaskStatus
        /// Accepts JSON payload with task ID and new status
        /// Triggers SignalR notification to all clients
        /// </summary>
        /// <param name="model">UpdateTaskStatusViewModel</param>
        /// <returns>JSON response indicating success/failure</returns>
        [HttpPost]
        public JsonResult UpdateTaskStatus(UpdateTaskStatusViewModel model)
        {
            try
            {
                // Validate model
                if (!ModelState.IsValid)
                {
                    return Json(ApiResponse<object>.ErrorResponse("Invalid data"));
                }

                // In a real app, get the current user ID from session/authentication
                int currentUserId = 1;

                // Update status (this will also trigger SignalR notification)
                var success = _taskService.UpdateTaskStatus(model.TaskId, model.TaskStatus, currentUserId);

                if (success)
                {
                    return Json(ApiResponse<object>.SuccessResponse(null, "Task status updated successfully"));
                }
                else
                {
                    return Json(ApiResponse<object>.ErrorResponse("Failed to update task status"));
                }
            }
            catch (Exception ex)
            {
                return Json(ApiResponse<object>.ErrorResponse("Error updating task status: " + ex.Message));
            }
        }

        /// <summary>
        /// ==> API: Assign task to a user
        /// POST: /Task/AssignTask
        /// Accepts JSON payload with task ID and user ID
        /// Triggers SignalR notification to all clients
        /// </summary>
        /// <param name="model">AssignTaskViewModel</param>
        /// <returns>JSON object of updated task</returns>
        [HttpPost]
        public JsonResult AssignTask(AssignTaskViewModel model)
        {
            try
            {
                // Validate model
                if (!ModelState.IsValid)
                {
                    return Json(ApiResponse<object>.ErrorResponse("Invalid data"));
                }

                // In a real app, get the current user ID from session/authentication
                int currentUserId = 1;

                // Assign task (this will also trigger SignalR notification)
                var updatedTask = _taskService.AssignTask(model.TaskId, model.AssignedToUserId, currentUserId);

                return Json(ApiResponse<object>.SuccessResponse(updatedTask, "Task assigned successfully"));
            }
            catch (Exception ex)
            {
                return Json(ApiResponse<object>.ErrorResponse("Error assigning task: " + ex.Message));
            }
        }

        /// <summary>
        /// ==> API: Delete a task
        /// POST: /Task/DeleteTask?id=1
        /// Soft deletes the task (sets IsActive = false)
        /// Triggers SignalR notification to all clients
        /// </summary>
        /// <param name="id">Task ID to delete</param>
        /// <returns>JSON response indicating success/failure</returns>
        [HttpPost]
        public JsonResult DeleteTask(int id)
        {
            try
            {
                // Delete task (this will also trigger SignalR notification)
                var success = _taskService.DeleteTask(id);

                if (success)
                {
                    return Json(ApiResponse<object>.SuccessResponse(null, "Task deleted successfully"));
                }
                else
                {
                    return Json(ApiResponse<object>.ErrorResponse("Failed to delete task"));
                }
            }
            catch (Exception ex)
            {
                return Json(ApiResponse<object>.ErrorResponse("Error deleting task: " + ex.Message));
            }
        }

        /// <summary>
        /// ==> API: Get all users for dropdown
        /// GET: /Task/GetUsers
        /// </summary>
        /// <returns>JSON array of users</returns>
        [HttpGet]
        public JsonResult GetUsers()
        {
            try
            {
                var users = _taskService.GetAllUsers();
                return Json(ApiResponse<object>.SuccessResponse(users), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ApiResponse<object>.ErrorResponse("Error retrieving users: " + ex.Message), 
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// ==> API: Get all activities for dropdown
        /// GET: /Task/GetActivities
        /// </summary>
        /// <returns>JSON array of activities</returns>
        [HttpGet]
        public JsonResult GetActivities()
        {
            try
            {
                var activities = _taskService.GetAllActivities();
                return Json(ApiResponse<object>.SuccessResponse(activities), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ApiResponse<object>.ErrorResponse("Error retrieving activities: " + ex.Message), 
                    JsonRequestBehavior.AllowGet);
            }
        }
    }
}
