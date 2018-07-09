using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(moviles.Backend.Startup))]
namespace moviles.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
