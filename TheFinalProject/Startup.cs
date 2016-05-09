using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheFinalProject.Startup))]
namespace TheFinalProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
