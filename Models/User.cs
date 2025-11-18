using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamManagementSystem.Models
{
    /// <summary>
    /// ==> User Entity: Represents users in the system
    /// ==> Roles: Admin (can create/assign tasks), User (can view assigned tasks)
    /// </summary>
    [Table("Users")]
    public class User
    {
        // ==> Primary Key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        // ==> User's unique username for login
        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, ErrorMessage = "Username cannot exceed 100 characters")]
        public string Username { get; set; }

        // ==> User's full display name
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(200, ErrorMessage = "Full name cannot exceed 200 characters")]
        public string FullName { get; set; }

        // ==> User's email address
        [Required(ErrorMessage = "Email is required")]
        [StringLength(200)]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        // ==> User role: "Admin" or "User"
        [Required]
        [StringLength(50)]
        public string Role { get; set; }

        // ==> Indicates if user account is active
        public bool IsActive { get; set; } = true;

        // ==> Account creation timestamp
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // ==> Navigation Property: Tasks assigned TO this user
        public virtual ICollection<Task> AssignedTasks { get; set; }

        // ==> Navigation Property: Activities created BY this user
        public virtual ICollection<Activity> CreatedActivities { get; set; }

        // ==> Navigation Property: Tasks created BY this user
        public virtual ICollection<Task> CreatedTasks { get; set; }

        // ==> Constructor: Initialize collections
        public User()
        {
            AssignedTasks = new HashSet<Task>();
            CreatedActivities = new HashSet<Activity>();
            CreatedTasks = new HashSet<Task>();
        }
    }
}
