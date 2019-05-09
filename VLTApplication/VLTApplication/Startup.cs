using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VLTApplication.Startup))]
namespace VLTApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
