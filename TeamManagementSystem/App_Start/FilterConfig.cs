using System.Web.Mvc;

namespace TeamManagementSystem
{
    /// <summary>
    /// ==> Added: Filter Configuration
    /// Registers global MVC filters that apply to all controllers/actions
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// ==> Register global filters
        /// Called during application startup
        /// </summary>
        /// <param name="filters">GlobalFilterCollection to configure</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // ==> Handle errors globally
            // Displays custom error pages for unhandled exceptions
            // Only active when <customErrors mode="On"> in Web.config
            filters.Add(new HandleErrorAttribute());

            // ==> Add other global filters here if needed
            // Examples:
            // - Authorization filters (e.g., [Authorize])
            // - Action filters (e.g., logging, caching)
            // - Result filters (e.g., response modification)
            // - Exception filters (e.g., custom error handling)
        }
    }
}
