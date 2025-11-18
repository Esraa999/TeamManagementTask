using System.Web.Mvc;

namespace TeamManagementSystem.Controllers
{
    /// <summary>
    /// ==> MVC Controller: Admin Dashboard
    /// ==> Provides view for admins to manage tasks
    /// ==> Route: /Admin/Index
    /// </summary>
    public class AdminController : Controller
    {
        /// <summary>
        /// ==> GET: /Admin/Index
        /// ==> Display admin dashboard for creating and managing tasks
        /// ==> Assumes logged-in admin user (UserId = 1 for demo)
        /// </summary>
        public ActionResult Index()
        {
            // ==> For demo purposes, assume admin user with ID = 1
            // ==> In a real application, you would get this from authentication/session
            ViewBag.CurrentUserId = 1;
            ViewBag.CurrentUserName = "Admin User";
            ViewBag.UserRole = "Admin";

            return View();
        }

        /// <summary>
        /// ==> GET: /Admin/Test
        /// ==> API diagnostic test page
        /// </summary>
        public ActionResult Test()
        {
            return View();
        }
    }
}
