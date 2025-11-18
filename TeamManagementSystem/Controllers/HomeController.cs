using System.Web.Mvc;

namespace TeamManagementSystem.Controllers
{
    /// <summary>
    /// ==> Added: Home Controller
    /// Handles main landing page and navigation
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// ==> Home page with links to Admin and User dashboards
        /// GET: /
        /// </summary>
        /// <returns>Home view</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// ==> About page
        /// GET: /Home/About
        /// </summary>
        /// <returns>About view</returns>
        public ActionResult About()
        {
            ViewBag.Message = "Team Management System - Assignment Solution";
            return View();
        }
    }
}
