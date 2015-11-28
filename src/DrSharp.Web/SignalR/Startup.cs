using Owin;

namespace DrSharp.Web.SignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        } 
    }
}