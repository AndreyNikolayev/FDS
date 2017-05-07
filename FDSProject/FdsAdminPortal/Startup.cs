using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FdsAdminPortal.Startup))]
namespace FdsAdminPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
