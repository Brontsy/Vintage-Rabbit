using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Areas.v3
{
    public class v3AreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "v3";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "v3_default",
                "v3/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
