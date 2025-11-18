using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TeamManagementSystem
{
    /// <summary>
    /// ==> Web API Configuration: Defines API routing and settings
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// ==> Register Web API routes and configuration
        /// </summary>
        public static void Register(HttpConfiguration config)
        {
            // ==> Enable attribute routing (for [Route] attributes on controllers)
            config.MapHttpAttributeRoutes();

            // ==> Configure JSON formatter to handle circular references
            var json = config.Formatters.JsonFormatter;
            // ==> Use default PascalCase (removed camelCase to match JavaScript expectations)
            json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;

            // ==> Default API route: api/{controller}/{id}
            // ==> Example: /api/tasks/1
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // ==> Enable CORS (Cross-Origin Resource Sharing) for API calls
            // ==> Allows JavaScript from any origin to call our API
            // ==> Note: CORS is disabled as the package is not installed
            // config.EnableCors();
        }
    }
}
