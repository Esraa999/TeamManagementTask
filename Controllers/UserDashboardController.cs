using System.Web.Mvc;

namespace TeamManagementSystem.Controllers
{
    /// <summary>
    /// ==> MVC Controller: User Dashboard
    /// ==> Provides view for regular users to view their assigned tasks
    /// ==> Route: /UserDashboard/Tasks
    /// </summary>
    public class UserDashboardController : Controller
    {
        /// <summary>
        /// ==> GET: /UserDashboard/Tasks
        /// ==> Display tasks assigned to the logged-in user
        /// ==> Tasks are loaded and updated in real-time via SignalR
        /// ==> Assumes logged-in user (UserId = 2 for demo)
        /// </summary>
        public ActionResult Tasks()
        {
            // ==> For demo purposes, assume regular user with ID = 2
            // ==> In a real application, you would get this from authentication/session
            ViewBag.CurrentUserId = 2;
            ViewBag.CurrentUserName = "John Doe";
            ViewBag.UserRole = "User";

            return View();
        }
    }
}
