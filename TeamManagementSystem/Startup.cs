using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TeamManagementSystem.Startup))]

namespace TeamManagementSystem
{
    /// <summary>
    /// ==> Added: OWIN Startup class
    /// Configures middleware components including SignalR
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ==> Configure OWIN middleware pipeline
        /// This method is called by the OWIN host at application startup
        /// </summary>
        /// <param name="app">IAppBuilder instance</param>
        public void Configuration(IAppBuilder app)
        {
            // ==> Enable detailed errors for SignalR (useful for debugging)
            // In production, set this to false
            var hubConfiguration = new Microsoft.AspNet.SignalR.HubConfiguration
            {
                EnableDetailedErrors = true,
                EnableJavaScriptProxies = true
            };

            // ==> Map SignalR hubs to /signalr endpoint
            // This allows clients to connect to SignalR at http://localhost/signalr
            app.MapSignalR(hubConfiguration);

            // ==> Configure CORS if needed (for cross-origin requests)
            // Uncomment if your frontend is on a different domain
            // app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
        }
    }
}
