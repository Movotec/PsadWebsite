using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PsadWebsite.Startup))]
namespace PsadWebsite
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
