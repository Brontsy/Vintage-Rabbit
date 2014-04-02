using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Areas.v7
{
    public class v7Routes : AreaRegistration
    {
        public static string Product = "Product";
        public static string Style = "Style";

        public override string AreaName
        {
            get
            {
                return "v7";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(v7Routes.Product, url: "v7/product/{name}/{productId}", defaults: new { controller = "Product", action = "Index" });
            context.MapRoute(v7Routes.Style, url: "v7/styles/{name}/{styleId}", defaults: new { controller = "Styles", action = "Style" });

            context.MapRoute("v7_default", "v7/{controller}/{action}/{id}", new { action = "Index", id = UrlParameter.Optional });
        }
    }
}
