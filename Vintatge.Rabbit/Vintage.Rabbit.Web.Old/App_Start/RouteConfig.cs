using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vintage.Rabbit.Web.Old
{
    public class Routes
    {
        public static string Home = "Home";
        public static string Buy = "Buy";
        public static string Hire = "Hire";
        public static string HireCheckAvailability = "HireCheckAvailability";
        public static string Style = "Styles";
        public static string ContactUs = "ContactUs";
        public static string Blog = "Blog";

        public static string Product = "Product";
        public static string StyleProduct = "StyleProduct";

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(Routes.Buy, url: "buy", defaults: new { controller = "Buy", action = "Index" });
            routes.MapRoute(Routes.Hire, url: "hire", defaults: new { controller = "Hire", action = "Index" });
            routes.MapRoute(Routes.Style, url: "style", defaults: new { controller = "Style", action = "Index" });
            routes.MapRoute(Routes.ContactUs, url: "contact-us", defaults: new { controller = "ContactUs", action = "Index" });
            routes.MapRoute(Routes.Blog, url: "blog", defaults: new { controller = "Blog", action = "Index" });


            routes.MapRoute(Routes.Product, url: "product/{name}/{productId}", defaults: new { controller = "Product", action = "Index" });
            routes.MapRoute(Routes.StyleProduct, url: "styles/{name}/{styleId}", defaults: new { controller = "Style", action = "Style" });

            routes.MapRoute(Routes.HireCheckAvailability, url: "hire/check-availability", defaults: new { controller = "Hire", action = "CheckAvailability" });
            

            routes.MapRoute(
                name: Routes.Home,
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}