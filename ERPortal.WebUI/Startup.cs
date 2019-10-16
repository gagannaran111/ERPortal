using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ERPortal.WebUI.Startup))]
namespace ERPortal.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
