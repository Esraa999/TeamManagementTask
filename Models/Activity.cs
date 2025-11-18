using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamManagementSystem.Models
{
    /// <summary>
    /// ==> Activity Entity: Represents projects or activity groups
    /// ==> Each activity can contain multiple tasks
    /// </summary>
    [Table("Activities")]
    public class Activity
    {
        // ==> Primary Key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityId { get; set; }

        // ==> Activity name/title
        [Required(ErrorMessage = "Activity name is required")]
        [StringLength(200, ErrorMessage = "Activity name cannot exceed 200 characters")]
        public string ActivityName { get; set; }

        // ==> Detailed description of the activity
        [StringLength(4000)]
        public string Description { get; set; }

        // ==> Activity start date (optional)
        public DateTime? StartDate { get; set; }

        // ==> Activity end date (optional)
        public DateTime? EndDate { get; set; }

        // ==> Foreign Key: User who created this activity
        [Required]
        public int CreatedBy { get; set; }

        // ==> Timestamp when activity was created
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // ==> Indicates if activity is active or archived
        public bool IsActive { get; set; } = true;

        // ==> Navigation Property: Reference to the user who created this activity
        [ForeignKey("CreatedBy")]
        public virtual User Creator { get; set; }

        // ==> Navigation Property: Collection of tasks under this activity
        public virtual ICollection<Task> Tasks { get; set; }

        // ==> Constructor: Initialize collections
        public Activity()
        {
            Tasks = new HashSet<Task>();
        }
    }
}
