using System;
using System.Linq;
using System.Web.Http;
using TeamManagementSystem.Models;

namespace TeamManagementSystem.Controllers
{
    /// <summary>
    /// ==> API Controller: Manages User CRUD operations
    /// ==> Base Route: /api/users
    /// </summary>
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        // ==> Database context for data access
        private readonly TeamManagementDbContext _context;

        // ==> Constructor: Initialize database context
        public UsersController()
        {
            _context = new TeamManagementDbContext();
        }

        /// <summary>
        /// ==> GET: api/users
        /// ==> Retrieve all active users
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllUsers()
        {
            try
            {
                // ==> Query all active users (AsNoTracking for performance)
                var users = _context.Users
                    .AsNoTracking()
                    .Where(u => u.IsActive)
                    .Select(u => new
                    {
                        u.UserId,
                        u.Username,
                        u.FullName,
                        u.Email,
                        u.Role,
                        u.IsActive,
                        u.CreatedDate,
                        // ==> Count of assigned tasks
                        AssignedTasksCount = u.AssignedTasks.Count()
                    })
                    .OrderBy(u => u.FullName)
                    .ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> GET: api/users/{id}
        /// ==> Retrieve a specific user by ID
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetUserById(int id)
        {
            try
            {
                // ==> Find user by ID
                var user = _context.Users
                    .Where(u => u.UserId == id)
                    .Select(u => new
                    {
                        u.UserId,
                        u.Username,
                        u.FullName,
                        u.Email,
                        u.Role,
                        u.IsActive,
                        u.CreatedDate,
                        // ==> Statistics
                        AssignedTasksCount = u.AssignedTasks.Count(),
                        CompletedTasksCount = u.AssignedTasks.Count(t => t.Status == "Completed"),
                        PendingTasksCount = u.AssignedTasks.Count(t => t.Status == "Pending"),
                        InProgressTasksCount = u.AssignedTasks.Count(t => t.Status == "InProgress")
                    })
                    .FirstOrDefault();

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> GET: api/users/role/{role}
        /// ==> Retrieve users by role (Admin or User)
        /// </summary>
        [HttpGet]
        [Route("role/{role}")]
        public IHttpActionResult GetUsersByRole(string role)
        {
            try
            {
                // ==> Validate role parameter
                var validRoles = new[] { "Admin", "User" };
                if (!validRoles.Contains(role))
                {
                    return BadRequest("Invalid role. Valid values: Admin, User");
                }

                // ==> Query users by role
                var users = _context.Users
                    .Where(u => u.Role == role && u.IsActive)
                    .Select(u => new
                    {
                        u.UserId,
                        u.Username,
                        u.FullName,
                        u.Email,
                        u.Role,
                        u.IsActive,
                        u.CreatedDate,
                        AssignedTasksCount = u.AssignedTasks.Count()
                    })
                    .OrderBy(u => u.FullName)
                    .ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> POST: api/users
        /// ==> Create a new user
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateUser([FromBody] User user)
        {
            try
            {
                // ==> Validate model
                if (user == null)
                {
                    return BadRequest("User data is required");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // ==> Check if username already exists
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    return BadRequest("Username already exists");
                }

                // ==> Check if email already exists
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    return BadRequest("Email already exists");
                }

                // ==> Validate role
                var validRoles = new[] { "Admin", "User" };
                if (!validRoles.Contains(user.Role))
                {
                    return BadRequest("Invalid role. Valid values: Admin, User");
                }

                // ==> Set timestamps
                user.CreatedDate = DateTime.Now;
                user.IsActive = true;

                // ==> Add user to database
                _context.Users.Add(user);
                _context.SaveChanges();

                // ==> Return created user
                var createdUser = new
                {
                    user.UserId,
                    user.Username,
                    user.FullName,
                    user.Email,
                    user.Role,
                    user.IsActive,
                    user.CreatedDate
                };

                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> PUT: api/users/{id}
        /// ==> Update an existing user
        /// </summary>
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            try
            {
                // ==> Find existing user
                var user = _context.Users.Find(id);
                if (user == null)
                {
                    return NotFound();
                }

                // ==> Update user properties
                user.FullName = updatedUser.FullName;
                user.Email = updatedUser.Email;
                user.Role = updatedUser.Role;
                user.IsActive = updatedUser.IsActive;

                // ==> Save changes
                _context.SaveChanges();

                // ==> Return updated user
                var userData = new
                {
                    user.UserId,
                    user.Username,
                    user.FullName,
                    user.Email,
                    user.Role,
                    user.IsActive,
                    user.CreatedDate
                };

                return Ok(userData);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> DELETE: api/users/{id}
        /// ==> Soft delete a user (set IsActive = false)
        /// </summary>
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteUser(int id)
        {
            try
            {
                // ==> Find user
                var user = _context.Users.Find(id);
                if (user == null)
                {
                    return NotFound();
                }

                // ==> Soft delete: just mark as inactive
                user.IsActive = false;
                _context.SaveChanges();

                return Ok(new { message = "User deactivated successfully", userId = id });
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
