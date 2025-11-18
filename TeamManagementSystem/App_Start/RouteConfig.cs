using System.Web.Mvc;
using System.Web.Routing;

namespace TeamManagementSystem
{
    /// <summary>
    /// ==> Added: Route Configuration
    /// Defines URL routing patterns for the application
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// ==> Register application routes
        /// Called during application startup
        /// </summary>
        /// <param name="routes">RouteCollection to configure</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            // ==> Ignore routes for special files (e.g., .axd files)
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // ==> Map SignalR routes (handled by OWIN, but ignore here)
            routes.IgnoreRoute("signalr/{*pathInfo}");

            // ==> Default MVC route pattern
            // Pattern: {controller}/{action}/{id}
            // Example URLs:
            //   /Home/Index
            //   /Admin/Index
            //   /User/Index
            //   /Task/GetAllTasks
            //   /Task/CreateTask
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
