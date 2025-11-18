using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamManagementSystem.Models
{
    /// <summary>
    /// ==> Added: Activity entity representing projects or activity groups
    /// Activities contain multiple tasks and represent work packages
    /// </summary>
    [Table("Activities")]
    public class Activity
    {
        /// <summary>
        /// Primary key for Activity entity
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityId { get; set; }

        /// <summary>
        /// Name of the activity/project
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ActivityName { get; set; }

        /// <summary>
        /// Detailed description of the activity
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Activity start date (nullable)
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Activity end date (nullable)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Date when the activity was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// User ID who created this activity
        /// </summary>
        [Required]
        public int CreatedBy { get; set; }

        /// <summary>
        /// Flag to indicate if activity is active
        /// </summary>
        public bool IsActive { get; set; }

        // ==> Navigation Properties

        /// <summary>
        /// User who created this activity
        /// Foreign key relationship to User entity
        /// </summary>
        [ForeignKey("CreatedBy")]
        public virtual User Creator { get; set; }

        /// <summary>
        /// Collection of tasks belonging to this activity
        /// </summary>
        public virtual ICollection<Task> Tasks { get; set; }

        /// <summary>
        /// Constructor initializing collections and default values
        /// </summary>
        public Activity()
        {
            // ==> Initialize navigation properties to avoid null reference
            Tasks = new HashSet<Task>();
            CreatedDate = DateTime.Now;
            IsActive = true;
        }
    }
}
