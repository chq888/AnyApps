using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AnyApps.Web.Startup))]
namespace AnyApps.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
