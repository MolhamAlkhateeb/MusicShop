using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MusicWebShopProject.Startup))]
namespace MusicWebShopProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
