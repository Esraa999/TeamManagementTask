using System;
using System.ComponentModel.DataAnnotations;

namespace TeamManagementSystem.Models.ViewModels
{
    /// <summary>
    /// ==> Added: DTO for transferring task data to client
    /// Flattened structure with all necessary display information
    /// </summary>
    public class TaskDTO
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; }
        public string TaskStatus { get; set; }
        public string Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// ==> Added: ViewModel for creating new tasks
    /// Contains only the fields needed from the client
    /// </summary>
    public class CreateTaskViewModel
    {
        [Required(ErrorMessage = "Task name is required")]
        [StringLength(200)]
        public string TaskName { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Activity is required")]
        public int ActivityId { get; set; }

        [Required(ErrorMessage = "Assigned user is required")]
        public int AssignedToUserId { get; set; }

        [StringLength(50)]
        public string Priority { get; set; }

        public DateTime? DueDate { get; set; }
    }

    /// <summary>
    /// ==> Added: ViewModel for updating task status
    /// Simple model for status updates
    /// </summary>
    public class UpdateTaskStatusViewModel
    {
        [Required]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(50)]
        public string TaskStatus { get; set; }
    }

    /// <summary>
    /// ==> Added: ViewModel for assigning/reassigning tasks
    /// </summary>
    public class AssignTaskViewModel
    {
        [Required]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "User is required")]
        public int AssignedToUserId { get; set; }
    }

    /// <summary>
    /// ==> Added: DTO for User information
    /// Lightweight user data for dropdowns/selection
    /// </summary>
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
    }

    /// <summary>
    /// ==> Added: DTO for Activity information
    /// Lightweight activity data for dropdowns/selection
    /// </summary>
    public class ActivityDTO
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    /// <summary>
    /// ==> Added: Generic API response wrapper
    /// Provides consistent response structure for all API endpoints
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        /// <summary>
        /// Create successful response
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Create error response
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default(T)
            };
        }
    }
}
