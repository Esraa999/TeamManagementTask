using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using TeamManagementSystem.Models;

namespace TeamManagementSystem
{
    /// <summary>
    /// ==> Global Application Class
    /// ==> Entry point for ASP.NET MVC application
    /// ==> Configures routing, filters, and application startup
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// ==> Application Start: Called when application first starts
        /// ==> Registers routes, filters, and configurations
        /// </summary>
        protected void Application_Start()
        {
            // ==> Register MVC areas (must be first)
            AreaRegistration.RegisterAllAreas();

            // ==> Register global filters
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // ==> Register Web API routes (before MVC routes to take precedence)
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // ==> Register MVC routes (for view controllers)
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // ==> Warm up Entity Framework to avoid first-request delay
            Database.SetInitializer<TeamManagementDbContext>(null);
            using (var context = new TeamManagementDbContext())
            {
                // Force EF to compile and cache queries
                context.Database.Initialize(force: false);
            }
        }
    }
}
