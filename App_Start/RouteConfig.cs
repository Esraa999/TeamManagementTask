using System.Web.Mvc;
using System.Web.Routing;

namespace TeamManagementSystem
{
    /// <summary>
    /// ==> Route Configuration: Defines URL routing for MVC controllers
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// ==> Register MVC routes
        /// </summary>
        public static void RegisterRoutes(RouteCollection routes)
        {
            // ==> Ignore routes for specific files
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // ==> Default route: {controller}/{action}/{id}
            // ==> Example: /Admin/Index
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
