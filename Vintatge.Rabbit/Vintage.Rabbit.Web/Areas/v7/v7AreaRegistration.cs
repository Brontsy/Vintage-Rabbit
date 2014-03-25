using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Areas.v7
{
    public class v7AreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "v7";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "v7_default",
                "v7/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
