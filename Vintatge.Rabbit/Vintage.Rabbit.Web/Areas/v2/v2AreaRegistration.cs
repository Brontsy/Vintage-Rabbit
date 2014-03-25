using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Areas.v1
{
    public class v2AreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "v2";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "v2_default",
                "v2/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
