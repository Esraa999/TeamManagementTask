using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using TeamManagementSystem.Models;

namespace TeamManagementSystem.Controllers
{
    /// <summary>
    /// ==> API Controller: Manages Activity CRUD operations
    /// ==> Base Route: /api/activities
    /// </summary>
    [RoutePrefix("api/activities")]
    public class ActivitiesController : ApiController
    {
        // ==> Database context for data access
        private readonly TeamManagementDbContext _context;

        // ==> Constructor: Initialize database context
        public ActivitiesController()
        {
            _context = new TeamManagementDbContext();
        }

        /// <summary>
        /// ==> GET: api/activities
        /// ==> Retrieve all activities with creator information
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllActivities()
        {
            try
            {
                // ==> Query all active activities (AsNoTracking for performance)
                var activities = _context.Activities
                    .AsNoTracking()
                    .Where(a => a.IsActive)
                    .Select(a => new
                    {
                        a.ActivityId,
                        a.ActivityName,
                        a.Description,
                        a.StartDate,
                        a.EndDate,
                        a.CreatedBy,
                        CreatedByName = a.Creator.FullName,
                        a.CreatedDate,
                        a.IsActive,
                        TaskCount = a.Tasks.Count()
                    })
                    .ToList();

                return Ok(activities);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> GET: api/activities/{id}
        /// ==> Retrieve a specific activity by ID
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetActivityById(int id)
        {
            try
            {
                // ==> Find activity by ID (optimized)
                var activity = _context.Activities
                    .Where(a => a.ActivityId == id)
                    .Select(a => new
                    {
                        a.ActivityId,
                        a.ActivityName,
                        a.Description,
                        a.StartDate,
                        a.EndDate,
                        a.CreatedBy,
                        CreatedByName = a.Creator.FullName,
                        a.CreatedDate,
                        a.IsActive,
                        Tasks = a.Tasks.Select(t => new
                        {
                            t.TaskId,
                            t.Title,
                            t.Status,
                            t.Priority,
                            t.AssignedToUserId,
                            AssignedToName = t.AssignedToUser != null ? t.AssignedToUser.FullName : "Unassigned"
                        }).ToList()
                    })
                    .FirstOrDefault();

                if (activity == null)
                {
                    return NotFound();
                }

                return Ok(activity);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> POST: api/activities
        /// ==> Create a new activity
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateActivity([FromBody] Activity activity)
        {
            try
            {
                // ==> Validate model
                if (activity == null)
                {
                    return BadRequest("Activity data is required");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // ==> Set timestamps
                activity.CreatedDate = DateTime.Now;
                activity.IsActive = true;

                // ==> Add activity to database
                _context.Activities.Add(activity);
                _context.SaveChanges();

                // ==> Return created activity with creator info
                var createdActivity = _context.Activities
                    .Where(a => a.ActivityId == activity.ActivityId)
                    .Select(a => new
                    {
                        a.ActivityId,
                        a.ActivityName,
                        a.Description,
                        a.StartDate,
                        a.EndDate,
                        a.CreatedBy,
                        CreatedByName = a.Creator.FullName,
                        a.CreatedDate,
                        a.IsActive
                    })
                    .FirstOrDefault();

                return Ok(createdActivity);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> PUT: api/activities/{id}
        /// ==> Update an existing activity
        /// </summary>
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateActivity(int id, [FromBody] Activity updatedActivity)
        {
            try
            {
                // ==> Find existing activity
                var activity = _context.Activities.Find(id);
                if (activity == null)
                {
                    return NotFound();
                }

                // ==> Update activity properties
                activity.ActivityName = updatedActivity.ActivityName;
                activity.Description = updatedActivity.Description;
                activity.StartDate = updatedActivity.StartDate;
                activity.EndDate = updatedActivity.EndDate;

                // ==> Save changes
                _context.SaveChanges();

                // ==> Return updated activity with creator info
                var activityData = _context.Activities
                    .Where(a => a.ActivityId == id)
                    .Select(a => new
                    {
                        a.ActivityId,
                        a.ActivityName,
                        a.Description,
                        a.StartDate,
                        a.EndDate,
                        a.CreatedBy,
                        CreatedByName = a.Creator.FullName,
                        a.CreatedDate,
                        a.IsActive
                    })
                    .FirstOrDefault();

                return Ok(activityData);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// ==> DELETE: api/activities/{id}
        /// ==> Soft delete an activity (set IsActive = false)
        /// ==> Physical deletion prevented if activity has tasks
        /// </summary>
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteActivity(int id)
        {
            try
            {
                // ==> Find activity
                var activity = _context.Activities
                    .FirstOrDefault(a => a.ActivityId == id);

                if (activity == null)
                {
                    return NotFound();
                }

                // ==> Check if activity has tasks
                if (activity.Tasks.Any())
                {
                    // ==> Soft delete: just mark as inactive
                    activity.IsActive = false;
                    _context.SaveChanges();
                    return Ok(new { message = "Activity archived (has tasks)", activityId = id });
                }
                else
                {
                    // ==> Hard delete: remove from database
                    _context.Activities.Remove(activity);
                    _context.SaveChanges();
                    return Ok(new { message = "Activity deleted successfully", activityId = id });
                }
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
