using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamManagementSystem.Models
{
    /// <summary>
    /// ==> Added: Task entity representing work items assigned to users
    /// Tasks belong to activities and are assigned to users
    /// </summary>
    [Table("Tasks")]
    public class Task
    {
        /// <summary>
        /// Primary key for Task entity
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        /// <summary>
        /// Name/title of the task
        /// </summary>
        [Required]
        [StringLength(200)]
        public string TaskName { get; set; }

        /// <summary>
        /// Detailed description of the task
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Foreign key to Activity entity
        /// </summary>
        [Required]
        public int ActivityId { get; set; }

        /// <summary>
        /// Foreign key to User entity (assigned user)
        /// </summary>
        [Required]
        public int AssignedToUserId { get; set; }

        /// <summary>
        /// Current status of the task
        /// Values: 'Pending', 'InProgress', 'Completed', 'Cancelled'
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TaskStatus { get; set; }

        /// <summary>
        /// Priority level of the task
        /// Values: 'Low', 'Medium', 'High'
        /// </summary>
        [StringLength(50)]
        public string Priority { get; set; }

        /// <summary>
        /// Due date for task completion (nullable)
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Date when the task was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// User ID who created this task
        /// </summary>
        [Required]
        public int CreatedBy { get; set; }

        /// <summary>
        /// Date when the task was last modified
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// User ID who last modified this task
        /// </summary>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Flag to indicate if task is active
        /// </summary>
        public bool IsActive { get; set; }

        // ==> Navigation Properties

        /// <summary>
        /// Activity this task belongs to
        /// Foreign key relationship to Activity entity
        /// </summary>
        [ForeignKey("ActivityId")]
        public virtual Activity Activity { get; set; }

        /// <summary>
        /// User to whom this task is assigned
        /// Foreign key relationship to User entity
        /// </summary>
        [ForeignKey("AssignedToUserId")]
        public virtual User AssignedToUser { get; set; }

        /// <summary>
        /// User who created this task
        /// Foreign key relationship to User entity
        /// </summary>
        [ForeignKey("CreatedBy")]
        public virtual User Creator { get; set; }

        /// <summary>
        /// Constructor initializing default values
        /// </summary>
        public Task()
        {
            // ==> Set default values for new task
            CreatedDate = DateTime.Now;
            IsActive = true;
            TaskStatus = "Pending"; // Default status
        }
    }
}
