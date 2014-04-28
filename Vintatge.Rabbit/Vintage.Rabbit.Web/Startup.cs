using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Vintage.Rabbit.Web.Startup))]
namespace Vintage.Rabbit.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
