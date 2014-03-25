using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Areas.v6
{
    public class v6AreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "v6";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "v6_default",
                "v6/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
