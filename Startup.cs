using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(TeamManagementSystem.Startup))]

namespace TeamManagementSystem
{
    /// <summary>
    /// ==> OWIN Startup: Configures SignalR for the application
    /// ==> This enables real-time bidirectional communication between server and clients
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ==> Configure SignalR in the OWIN pipeline
        /// </summary>
        /// <param name="app">OWIN application builder</param>
        public void Configuration(IAppBuilder app)
        {
            // ==> Configure SignalR settings
            var hubConfiguration = new HubConfiguration
            {
                // ==> Enable detailed error messages (set to false in production)
                EnableDetailedErrors = true,
                
                // ==> Enable JavaScript proxies for easier client-side code
                EnableJavaScriptProxies = true
            };

            // ==> Map SignalR hubs to the "/signalr" route
            // ==> Clients will connect to: http://yoursite/signalr
            app.MapSignalR("/signalr", hubConfiguration);
        }
    }
}
