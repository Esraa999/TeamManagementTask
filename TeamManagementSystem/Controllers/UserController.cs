using System.Web.Mvc;

namespace TeamManagementSystem.Controllers
{
    /// <summary>
    /// ==> Added: Controller for User Dashboard
    /// Handles user view rendering for viewing assigned tasks
    /// </summary>
    public class UserController : Controller
    {
        /// <summary>
        /// ==> User Dashboard - Main view
        /// GET: /User/Index
        /// Displays the user dashboard where users can:
        /// - View tasks assigned to them
        /// - See task details (Activity, Priority, Due Date, Status)
        /// - Receive real-time updates when tasks are assigned or modified
        /// 
        /// Note: In a real application, we would get the userId from authentication/session
        /// For this demo, we're using a hardcoded userId (2 = John Doe)
        /// </summary>
        /// <returns>User dashboard view</returns>
        public ActionResult Index()
        {
            // In a real application, get userId from Session or Identity
            // For demo purposes, we'll pass it through ViewBag
            // User ID 2 = John Doe (regular user)
            ViewBag.UserId = 2;
            ViewBag.UserName = "John Doe";

            return View();
        }

        /// <summary>
        /// ==> User Dashboard for specific user
        /// GET: /User/Dashboard/2
        /// Allows viewing dashboard for different users (for testing)
        /// </summary>
        /// <param name="userId">User ID to display tasks for</param>
        /// <returns>User dashboard view</returns>
        public ActionResult Dashboard(int? userId)
        {
            // Default to user ID 2 if not specified
            int selectedUserId = userId ?? 2;
            
            // Get user name based on ID (in real app, query from database)
            string userName = selectedUserId == 2 ? "John Doe" : 
                             selectedUserId == 3 ? "Jane Smith" : "User";

            ViewBag.UserId = selectedUserId;
            ViewBag.UserName = userName;

            return View("Index");
        }
    }
}
