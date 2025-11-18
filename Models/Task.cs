using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamManagementSystem.Models
{
    /// <summary>
    /// ==> Task Entity: Represents individual tasks within activities
    /// ==> Tasks can be assigned to users and have status tracking
    /// </summary>
    [Table("Tasks")]
    public class Task
    {
        // ==> Primary Key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        // ==> Task title/name
        [Required(ErrorMessage = "Task title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }

        // ==> Detailed task description
        [StringLength(4000)]
        public string Description { get; set; }

        // ==> Foreign Key: Parent activity this task belongs to
        [Required]
        public int ActivityId { get; set; }

        // ==> Foreign Key: User assigned to complete this task (nullable - can be unassigned)
        public int? AssignedToUserId { get; set; }

        // ==> Task status: Pending, InProgress, Completed, OnHold
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        // ==> Task priority: Low, Medium, High, Critical
        [StringLength(20)]
        public string Priority { get; set; } = "Medium";

        // ==> Task due date (optional)
        public DateTime? DueDate { get; set; }

        // ==> Foreign Key: User who created this task
        [Required]
        public int CreatedBy { get; set; }

        // ==> Timestamp when task was created
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // ==> Timestamp of last update
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // ==> Timestamp when task was completed (nullable)
        public DateTime? CompletedDate { get; set; }

        // ==> Navigation Property: Reference to parent activity
        [ForeignKey("ActivityId")]
        public virtual Activity Activity { get; set; }

        // ==> Navigation Property: Reference to assigned user
        [ForeignKey("AssignedToUserId")]
        public virtual User AssignedToUser { get; set; }

        // ==> Navigation Property: Reference to user who created this task
        [ForeignKey("CreatedBy")]
        public virtual User CreatorUser { get; set; }
    }
}
