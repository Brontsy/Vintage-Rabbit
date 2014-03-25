using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Areas.v5
{
    public class v5AreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "v5";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "v5_default",
                "v5/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
