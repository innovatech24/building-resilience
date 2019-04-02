using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Resilience.Startup))]
namespace Resilience
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
