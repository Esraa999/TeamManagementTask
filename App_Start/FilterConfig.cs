using System.Web.Mvc;

namespace TeamManagementSystem
{
    /// <summary>
    /// ==> Filter Configuration: Registers global MVC filters
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// ==> Register global filters (error handling, authorization, etc.)
        /// </summary>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // ==> Handle errors globally
            filters.Add(new HandleErrorAttribute());
        }
    }
}
