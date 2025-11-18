using System.Data.Entity;
using TeamManagementSystem.Models;

namespace TeamManagementSystem.Data
{
    /// <summary>
    /// ==> Added: Entity Framework DbContext for Team Management System
    /// Manages database connection and entity sets
    /// </summary>
    public class TeamManagementContext : DbContext
    {
        /// <summary>
        /// Constructor - uses connection string from Web.config
        /// Connection string name: "DefaultConnection"
        /// </summary>
        public TeamManagementContext() : base("DefaultConnection")
        {
            // ==> Enable lazy loading for navigation properties
            Configuration.LazyLoadingEnabled = true;
            
            // ==> Enable proxy creation for change tracking
            Configuration.ProxyCreationEnabled = true;
        }

        // ==> DbSet properties representing database tables

        /// <summary>
        /// Users table - contains all system users (Admin and Regular)
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Activities table - contains all activities/projects
        /// </summary>
        public DbSet<Activity> Activities { get; set; }

        /// <summary>
        /// Tasks table - contains all tasks assigned to users
        /// </summary>
        public DbSet<Task> Tasks { get; set; }

        /// <summary>
        /// ==> Configure entity relationships and constraints
        /// Called when the model is being created
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==> Configure User entity
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            // ==> Configure Activity entity
            modelBuilder.Entity<Activity>()
                .HasKey(a => a.ActivityId);

            modelBuilder.Entity<Activity>()
                .Property(a => a.ActivityName)
                .IsRequired()
                .HasMaxLength(200);

            // ==> Configure relationship: Activity -> Creator (User)
            modelBuilder.Entity<Activity>()
                .HasRequired(a => a.Creator)
                .WithMany(u => u.CreatedActivities)
                .HasForeignKey(a => a.CreatedBy)
                .WillCascadeOnDelete(false); // Prevent cascade delete

            // ==> Configure Task entity
            modelBuilder.Entity<Task>()
                .HasKey(t => t.TaskId);

            modelBuilder.Entity<Task>()
                .Property(t => t.TaskName)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Task>()
                .Property(t => t.TaskStatus)
                .IsRequired()
                .HasMaxLength(50);

            // ==> Configure relationship: Task -> Activity
            modelBuilder.Entity<Task>()
                .HasRequired(t => t.Activity)
                .WithMany(a => a.Tasks)
                .HasForeignKey(t => t.ActivityId)
                .WillCascadeOnDelete(false); // Prevent cascade delete

            // ==> Configure relationship: Task -> AssignedToUser
            modelBuilder.Entity<Task>()
                .HasRequired(t => t.AssignedToUser)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedToUserId)
                .WillCascadeOnDelete(false); // Prevent cascade delete

            // ==> Configure relationship: Task -> Creator (User)
            modelBuilder.Entity<Task>()
                .HasRequired(t => t.Creator)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatedBy)
                .WillCascadeOnDelete(false); // Prevent cascade delete
        }

        /// <summary>
        /// ==> Create a static factory method for creating context instances
        /// Useful for dependency injection and unit testing
        /// </summary>
        public static TeamManagementContext Create()
        {
            return new TeamManagementContext();
        }
    }
}
