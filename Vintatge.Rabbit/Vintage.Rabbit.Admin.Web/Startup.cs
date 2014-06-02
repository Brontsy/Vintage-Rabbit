using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Vintage.Rabbit.Admin.Web.Startup))]
namespace Vintage.Rabbit.Admin.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
