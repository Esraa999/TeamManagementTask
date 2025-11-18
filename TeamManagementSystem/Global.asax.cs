using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TeamManagementSystem.Data;
using TeamManagementSystem.Repositories;
using TeamManagementSystem.Services;

namespace TeamManagementSystem
{
    /// <summary>
    /// ==> Added: Global application class
    /// Handles application-level events and initialization
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// ==> Application Start event
        /// Called once when the application starts
        /// Initializes routing, dependency injection, and other configurations
        /// </summary>
        protected void Application_Start()
        {
            // ==> Register MVC areas (if any)
            AreaRegistration.RegisterAllAreas();

            // ==> Configure Web API routes (if using Web API)
            // GlobalConfiguration.Configure(WebApiConfig.Register);

            // ==> Register filter configuration (global filters)
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // ==> Register MVC routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // ==> Register bundles (CSS/JS bundling and minification)
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // ==> Initialize dependency injection container
            InitializeDependencyInjection();

            // ==> Database initialization (creates DB if not exists)
            System.Data.Entity.Database.SetInitializer(new System.Data.Entity.CreateDatabaseIfNotExists<TeamManagementContext>());
        }

        /// <summary>
        /// ==> Initialize Dependency Injection
        /// Sets up IoC container for dependency resolution
        /// In this simple implementation, we use a basic service locator pattern
        /// For production, consider using Unity, Autofac, or Ninject
        /// </summary>
        private void InitializeDependencyInjection()
        {
            // ==> Register custom dependency resolver
            DependencyResolver.SetResolver(new CustomDependencyResolver());
        }

        /// <summary>
        /// ==> Application Error event
        /// Global error handler for unhandled exceptions
        /// </summary>
        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            
            // Log the exception (in production, use a logging framework like NLog or Serilog)
            System.Diagnostics.Debug.WriteLine("Application Error: " + exception?.Message);
            
            // Clear the error (prevents default error page)
            // Server.ClearError();
            
            // Redirect to error page or return error view
            // Response.Redirect("/Error");
        }
    }

    /// <summary>
    /// ==> Added: Custom Dependency Resolver
    /// Simple implementation of IDependencyResolver for MVC
    /// Resolves dependencies for controllers
    /// </summary>
    public class CustomDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// ==> Resolve a single service of the specified type
        /// </summary>
        public object GetService(Type serviceType)
        {
            // ==> Create instances based on type
            if (serviceType == typeof(Controllers.TaskController))
            {
                // Create TaskController with its dependencies
                var context = new TeamManagementContext();
                var repository = new TaskRepository(context);
                var service = new TaskService(repository);
                return new Controllers.TaskController(service);
            }

            // ==> For other controllers, return null to use default activation
            return null;
        }

        /// <summary>
        /// ==> Resolve all services of the specified type
        /// </summary>
        public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType)
        {
            return new System.Collections.Generic.List<object>();
        }
    }
}
