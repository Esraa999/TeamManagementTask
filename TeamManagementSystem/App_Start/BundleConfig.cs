using System.Web.Optimization;

namespace TeamManagementSystem
{
    /// <summary>
    /// ==> Added: Bundle Configuration
    /// Configures CSS and JavaScript bundling and minification
    /// Improves page load performance by combining and compressing assets
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// ==> Register application bundles
        /// Called during application startup
        /// </summary>
        /// <param name="bundles">BundleCollection to configure</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            // ==> jQuery bundle
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // ==> jQuery Validation bundle
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // ==> Modernizr bundle (feature detection)
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // ==> Bootstrap JavaScript bundle
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            // ==> Bootstrap CSS bundle
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // ==> Enable optimizations for production
            // Set to true to enable bundling and minification
            // Set to false for debugging (uses separate files)
#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
