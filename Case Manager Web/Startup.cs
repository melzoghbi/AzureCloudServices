using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Case_Manager_Web.Startup))]
namespace Case_Manager_Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
