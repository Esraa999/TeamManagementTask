using System.Data.Entity;

namespace TeamManagementSystem.Models
{
    /// <summary>
    /// ==> DbContext: Entity Framework database context for Team Management System
    /// ==> Handles database connections and entity mappings
    /// </summary>
    public class TeamManagementDbContext : DbContext
    {
        // ==> Constructor: Uses "DefaultConnection" from Web.config
        public TeamManagementDbContext() : base("DefaultConnection")
        {
            // ==> Disable lazy loading for better performance with SignalR
            this.Configuration.LazyLoadingEnabled = false;
            
            // ==> Enable proxy creation for change tracking
            this.Configuration.ProxyCreationEnabled = true;
        }

        // ==> DbSet: Users table
        public DbSet<User> Users { get; set; }

        // ==> DbSet: Activities table
        public DbSet<Activity> Activities { get; set; }

        // ==> DbSet: Tasks table
        public DbSet<Task> Tasks { get; set; }

        /// <summary>
        /// ==> Configure entity relationships and constraints
        /// ==> Called when model is being created
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
                .Property(u => u.Role)
                .IsRequired();

            // ==> Configure Activity entity
            modelBuilder.Entity<Activity>()
                .HasKey(a => a.ActivityId);

            // ==> Configure relationship: Activity -> User (Creator)
            modelBuilder.Entity<Activity>()
                .HasRequired(a => a.Creator)
                .WithMany(u => u.CreatedActivities)
                .HasForeignKey(a => a.CreatedBy)
                .WillCascadeOnDelete(false); // ==> Prevent cascade delete

            // ==> Configure Task entity
            modelBuilder.Entity<Task>()
                .HasKey(t => t.TaskId);

            // ==> Configure relationship: Task -> Activity (Parent)
            modelBuilder.Entity<Task>()
                .HasRequired(t => t.Activity)
                .WithMany(a => a.Tasks)
                .HasForeignKey(t => t.ActivityId)
                .WillCascadeOnDelete(false); // ==> Prevent cascade delete

            // ==> Configure relationship: Task -> User (AssignedTo) - Optional
            modelBuilder.Entity<Task>()
                .HasOptional(t => t.AssignedToUser)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedToUserId)
                .WillCascadeOnDelete(false); // ==> Prevent cascade delete

            // ==> Configure relationship: Task -> User (Creator)
            modelBuilder.Entity<Task>()
                .HasRequired(t => t.CreatorUser)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatedBy)
                .WillCascadeOnDelete(false); // ==> Prevent cascade delete

            // ==> Configure indexes for better query performance
            modelBuilder.Entity<Task>()
                .Property(t => t.Status)
                .IsRequired();

            modelBuilder.Entity<Task>()
                .Property(t => t.Priority)
                .HasMaxLength(20);
        }
    }
}
