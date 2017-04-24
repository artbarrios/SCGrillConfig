using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SCGrillConfig.Startup))]
namespace SCGrillConfig
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
