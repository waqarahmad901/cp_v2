using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CP_v2.Startup))]
namespace CP_v2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
