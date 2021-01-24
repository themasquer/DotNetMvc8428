using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DotNetMvc.Startup))]
namespace DotNetMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
