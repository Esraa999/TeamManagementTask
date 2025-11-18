using System.Web.Mvc;

namespace TeamManagementSystem.Controllers
{
    /// <summary>
    /// ==> Added: Controller for Admin Dashboard
    /// Handles admin view rendering and admin-specific operations
    /// </summary>
    public class AdminController : Controller
    {
        /// <summary>
        /// ==> Admin Dashboard - Main view
        /// GET: /Admin/Index
        /// Displays the admin dashboard where admin can:
        /// - View all tasks
        /// - Create new tasks
        /// - Assign tasks to users
        /// - Update task status
        /// - View real-time updates
        /// </summary>
        /// <returns>Admin dashboard view</returns>
        public ActionResult Index()
        {
            // Return the admin dashboard view
            // View contains Vue.js application for task management
            return View();
        }

        /// <summary>
        /// ==> Alternative route for Admin Dashboard
        /// GET: /Admin/Dashboard
        /// Same as Index, just alternative URL
        /// </summary>
        /// <returns>Admin dashboard view</returns>
        public ActionResult Dashboard()
        {
            return View("Index");
        }
    }
}
