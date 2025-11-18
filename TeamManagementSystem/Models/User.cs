using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamManagementSystem.Models
{
    /// <summary>
    /// ==> Added: User entity representing system users (Admin or Regular User)
    /// Stores user information and their role in the system
    /// </summary>
    [Table("Users")]
    public class User
    {
        /// <summary>
        /// Primary key for User entity
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        /// <summary>
        /// Unique username for login/identification
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        /// <summary>
        /// Full name of the user
        /// </summary>
        [Required]
        [StringLength(200)]
        public string FullName { get; set; }

        /// <summary>
        /// Email address of the user
        /// </summary>
        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User role: 'Admin' or 'User'
        /// Admin can create/manage tasks, User can view assigned tasks
        /// </summary>
        [Required]
        [StringLength(50)]
        public string UserRole { get; set; }

        /// <summary>
        /// Date when the user was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Flag to indicate if user is active
        /// </summary>
        public bool IsActive { get; set; }

        // ==> Navigation Properties

        /// <summary>
        /// Collection of tasks assigned to this user
        /// </summary>
        public virtual ICollection<Task> AssignedTasks { get; set; }

        /// <summary>
        /// Collection of tasks created by this user
        /// </summary>
        public virtual ICollection<Task> CreatedTasks { get; set; }

        /// <summary>
        /// Collection of activities created by this user
        /// </summary>
        public virtual ICollection<Activity> CreatedActivities { get; set; }

        /// <summary>
        /// Constructor initializing collections
        /// </summary>
        public User()
        {
            // ==> Initialize navigation properties to avoid null reference
            AssignedTasks = new HashSet<Task>();
            CreatedTasks = new HashSet<Task>();
            CreatedActivities = new HashSet<Activity>();
            CreatedDate = DateTime.Now;
            IsActive = true;
        }
    }
}
