using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using TeamManagementSystem.Models;
using TeamManagementSystem.Hubs;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;

namespace TeamManagementSystem.Controllers
{
    /// <summary>
    /// ==> API Controller: Manages Task CRUD operations
    /// ==> Uses SignalR to broadcast changes in real-time
    /// ==> Base Route: /api/tasks
    /// </summary>
    [RoutePrefix("api/tasks")]
    public class TasksController : ApiController
    {
        // ==> Database context for data access
        private readonly TeamManagementDbContext _context;

        // ==> SignalR hub context for real-time notifications
        private readonly IHubContext _hubContext;

        // ==> Constructor: Initialize database and SignalR contexts
        public TasksController()
        {
            _context = new TeamManagementDbContext();
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();
        }

        /// <summary>
        /// ==> GET: api/tasks
        /// ==> Retrieve all tasks with related data
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllTasks()
        {
            try
            {
                // ==> Query all tasks (AsNoTracking for performance)
                var tasks = _context.Tasks
                    .AsNoTracking()
                    .Select(t => new
                    {
                        t.TaskId,
                        t.Title,
                        t.Description,
                        t.ActivityId,
                        ActivityName = t.Activity.ActivityName,
                        t.AssignedToUserId,
                        AssignedToName = t.AssignedToUser != null ? t.AssignedToUser.FullName : "Unassigned",
                        t.Status,
                        t.Priority,
                        t.DueDate,
                        t.CreatedBy,
                        CreatedByName = t.CreatorUser.FullName,
                        t.CreatedDate,
                        t.UpdatedDate,
                        t.CompletedDate
                    })
                    .OrderByDescending(t => t.CreatedDate)
                    .ToList();

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                // ==> Return error response if query fails
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> GET: api/tasks/{id}
        /// ==> Retrieve a specific task by ID
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetTaskById(int id)
        {
            try
            {
                // ==> Find task by ID with related data
                var task = _context.Tasks
                    .Include(t => t.Activity)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.CreatorUser)
                    .Where(t => t.TaskId == id)
                    .Select(t => new
                    {
                        t.TaskId,
                        t.Title,
                        t.Description,
                        t.ActivityId,
                        ActivityName = t.Activity.ActivityName,
                        t.AssignedToUserId,
                        AssignedToName = t.AssignedToUser != null ? t.AssignedToUser.FullName : "Unassigned",
                        t.Status,
                        t.Priority,
                        t.DueDate,
                        t.CreatedBy,
                        CreatedByName = t.CreatorUser.FullName,
                        t.CreatedDate,
                        t.UpdatedDate,
                        t.CompletedDate
                    })
                    .FirstOrDefault();

                if (task == null)
                {
                    // ==> Task not found
                    return NotFound();
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> GET: api/tasks/user/{userId}
        /// ==> Retrieve all tasks assigned to a specific user
        /// </summary>
        [HttpGet]
        [Route("user/{userId:int}")]
        public IHttpActionResult GetTasksByUser(int userId)
        {
            try
            {
                // ==> Query tasks assigned to the specified user
                var tasks = _context.Tasks
                    .Include(t => t.Activity)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.CreatorUser)
                    .Where(t => t.AssignedToUserId == userId)
                    .Select(t => new
                    {
                        t.TaskId,
                        t.Title,
                        t.Description,
                        t.ActivityId,
                        ActivityName = t.Activity.ActivityName,
                        t.AssignedToUserId,
                        AssignedToName = t.AssignedToUser.FullName,
                        t.Status,
                        t.Priority,
                        t.DueDate,
                        t.CreatedBy,
                        CreatedByName = t.CreatorUser.FullName,
                        t.CreatedDate,
                        t.UpdatedDate,
                        t.CompletedDate
                    })
                    .OrderBy(t => t.DueDate)
                    .ThenBy(t => t.Priority)
                    .ToList();

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> GET: api/tasks/status/{status}
        /// ==> Retrieve all tasks with a specific status
        /// </summary>
        [HttpGet]
        [Route("status/{status}")]
        public IHttpActionResult GetTasksByStatus(string status)
        {
            try
            {
                // ==> Validate status parameter
                var validStatuses = new[] { "Pending", "InProgress", "Completed", "OnHold" };
                if (!validStatuses.Contains(status))
                {
                    return BadRequest("Invalid status. Valid values: Pending, InProgress, Completed, OnHold");
                }

                // ==> Query tasks by status
                var tasks = _context.Tasks
                    .Include(t => t.Activity)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.CreatorUser)
                    .Where(t => t.Status == status)
                    .Select(t => new
                    {
                        t.TaskId,
                        t.Title,
                        t.Description,
                        t.ActivityId,
                        ActivityName = t.Activity.ActivityName,
                        t.AssignedToUserId,
                        AssignedToName = t.AssignedToUser != null ? t.AssignedToUser.FullName : "Unassigned",
                        t.Status,
                        t.Priority,
                        t.DueDate,
                        t.CreatedBy,
                        CreatedByName = t.CreatorUser.FullName,
                        t.CreatedDate,
                        t.UpdatedDate
                    })
                    .OrderByDescending(t => t.CreatedDate)
                    .ToList();

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> POST: api/tasks
        /// ==> Create a new task
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateTask([FromBody] Task task)
        {
            try
            {
                // ==> Validate model
                if (task == null)
                {
                    return BadRequest("Task data is required");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // ==> Set timestamps
                task.CreatedDate = DateTime.Now;
                task.UpdatedDate = DateTime.Now;

                // ==> Set default status if not provided
                if (string.IsNullOrEmpty(task.Status))
                {
                    task.Status = "Pending";
                }

                // ==> Add task to database
                _context.Tasks.Add(task);
                _context.SaveChanges();

                // ==> Reload task with related data for SignalR notification
                var createdTask = _context.Tasks
                    .Include(t => t.Activity)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.CreatorUser)
                    .Where(t => t.TaskId == task.TaskId)
                    .Select(t => new
                    {
                        t.TaskId,
                        t.Title,
                        t.Description,
                        t.ActivityId,
                        ActivityName = t.Activity.ActivityName,
                        t.AssignedToUserId,
                        AssignedToName = t.AssignedToUser != null ? t.AssignedToUser.FullName : "Unassigned",
                        t.Status,
                        t.Priority,
                        t.DueDate,
                        t.CreatedBy,
                        CreatedByName = t.CreatorUser.FullName,
                        t.CreatedDate,
                        t.UpdatedDate
                    })
                    .FirstOrDefault();

                // ==> Notify all connected clients via SignalR
                _hubContext.Clients.All.taskCreated(createdTask);

                return Ok(createdTask);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> PUT: api/tasks/{id}
        /// ==> Update an existing task
        /// </summary>
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateTask(int id, [FromBody] Task updatedTask)
        {
            try
            {
                // ==> Find existing task
                var task = _context.Tasks.Find(id);
                if (task == null)
                {
                    return NotFound();
                }

                // ==> Update task properties
                task.Title = updatedTask.Title;
                task.Description = updatedTask.Description;
                task.ActivityId = updatedTask.ActivityId;
                task.AssignedToUserId = updatedTask.AssignedToUserId;
                task.Status = updatedTask.Status;
                task.Priority = updatedTask.Priority;
                task.DueDate = updatedTask.DueDate;
                task.UpdatedDate = DateTime.Now;

                // ==> Set completion date if status is Completed
                if (updatedTask.Status == "Completed" && task.CompletedDate == null)
                {
                    task.CompletedDate = DateTime.Now;
                }

                // ==> Save changes
                _context.SaveChanges();

                // ==> Reload task with related data for SignalR notification
                var taskData = _context.Tasks
                    .Include(t => t.Activity)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.CreatorUser)
                    .Where(t => t.TaskId == id)
                    .Select(t => new
                    {
                        t.TaskId,
                        t.Title,
                        t.Description,
                        t.ActivityId,
                        ActivityName = t.Activity.ActivityName,
                        t.AssignedToUserId,
                        AssignedToName = t.AssignedToUser != null ? t.AssignedToUser.FullName : "Unassigned",
                        t.Status,
                        t.Priority,
                        t.DueDate,
                        t.CreatedBy,
                        CreatedByName = t.CreatorUser.FullName,
                        t.CreatedDate,
                        t.UpdatedDate,
                        t.CompletedDate
                    })
                    .FirstOrDefault();

                // ==> Notify all connected clients via SignalR
                _hubContext.Clients.All.taskUpdated(taskData);

                return Ok(taskData);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> DELETE: api/tasks/{id}
        /// ==> Delete a task
        /// </summary>
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteTask(int id)
        {
            try
            {
                // ==> Find task
                var task = _context.Tasks.Find(id);
                if (task == null)
                {
                    return NotFound();
                }

                // ==> Remove task from database
                _context.Tasks.Remove(task);
                _context.SaveChanges();

                // ==> Notify all connected clients via SignalR
                _hubContext.Clients.All.taskDeleted(id);

                return Ok(new { message = "Task deleted successfully", taskId = id });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> POST: api/tasks/{taskId}/assign/{userId}
        /// ==> Assign a task to a user
        /// </summary>
        [HttpPost]
        [Route("{taskId:int}/assign/{userId:int}")]
        public IHttpActionResult AssignTask(int taskId, int userId)
        {
            try
            {
                // ==> Find task
                var task = _context.Tasks.Find(taskId);
                if (task == null)
                {
                    return NotFound();
                }

                // ==> Verify user exists
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    return BadRequest("User not found");
                }

                // ==> Assign task to user
                task.AssignedToUserId = userId;
                task.UpdatedDate = DateTime.Now;
                _context.SaveChanges();

                // ==> Reload task with related data
                var taskData = _context.Tasks
                    .Include(t => t.Activity)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.CreatorUser)
                    .Where(t => t.TaskId == taskId)
                    .Select(t => new
                    {
                        t.TaskId,
                        t.Title,
                        t.Description,
                        t.ActivityId,
                        ActivityName = t.Activity.ActivityName,
                        t.AssignedToUserId,
                        AssignedToName = t.AssignedToUser.FullName,
                        t.Status,
                        t.Priority,
                        t.DueDate,
                        t.CreatedBy,
                        CreatedByName = t.CreatorUser.FullName,
                        t.CreatedDate,
                        t.UpdatedDate
                    })
                    .FirstOrDefault();

                // ==> Notify all connected clients via SignalR
                _hubContext.Clients.All.taskAssigned(taskData, userId);

                return Ok(taskData);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> PUT: api/tasks/{taskId}/status
        /// ==> Update task status only
        /// </summary>
        [HttpPut]
        [Route("{taskId:int}/status")]
        public IHttpActionResult UpdateStatus(int taskId, [FromBody] dynamic statusData)
        {
            try
            {
                // ==> Find task
                var task = _context.Tasks.Find(taskId);
                if (task == null)
                {
                    return NotFound();
                }

                // ==> Get status from request body
                string newStatus = statusData.status;

                // ==> Validate status
                var validStatuses = new[] { "Pending", "InProgress", "Completed", "OnHold" };
                if (!validStatuses.Contains(newStatus))
                {
                    return BadRequest("Invalid status");
                }

                // ==> Update status
                task.Status = newStatus;
                task.UpdatedDate = DateTime.Now;

                // ==> Set completion date if status is Completed
                if (newStatus == "Completed" && task.CompletedDate == null)
                {
                    task.CompletedDate = DateTime.Now;
                }

                _context.SaveChanges();

                // ==> Reload task with related data
                var taskData = _context.Tasks
                    .Include(t => t.Activity)
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.CreatorUser)
                    .Where(t => t.TaskId == taskId)
                    .Select(t => new
                    {
                        t.TaskId,
                        t.Title,
                        t.Description,
                        t.ActivityId,
                        ActivityName = t.Activity.ActivityName,
                        t.AssignedToUserId,
                        AssignedToName = t.AssignedToUser != null ? t.AssignedToUser.FullName : "Unassigned",
                        t.Status,
                        t.Priority,
                        t.DueDate,
                        t.CreatedBy,
                        CreatedByName = t.CreatorUser.FullName,
                        t.CreatedDate,
                        t.UpdatedDate,
                        t.CompletedDate
                    })
                    .FirstOrDefault();

                // ==> Notify all connected clients via SignalR
                _hubContext.Clients.All.taskUpdated(taskData);

                return Ok(taskData);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ==> Dispose database context when controller is disposed
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
