using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Areas.v4
{
    public class v4AreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "v4";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "v4_default",
                "v4/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
