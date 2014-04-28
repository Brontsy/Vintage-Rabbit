using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vintage.Rabbit.Web
{
    public class Routes
    {
        public static string Home = "Home";
        public static string Hire = "Hire";
        public static string HireCheckAvailability = "HireCheckAvailability";
        public static string Style = "Styles";
        public static string ContactUs = "ContactUs";
        public static string Blog = "Blog";

        public static string StyleProduct = "StyleProduct";

        public static class ShoppingCart
        {
            public static string PageHeader = "ShoppingCartPageHeader";
            public static string Add = "ShoppingCartAdd";
            public static string Remove = "ShoppingCartRemove";

        }

        public static class Product
        {
            public static string Index = "Product";
            public static string Preview = "ProductPreview";

        }

        public static class Buy
        {
            public static string Index = "Buy";
            public static string Category = "BuyCategory";

        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(Routes.Hire, url: "hire", defaults: new { controller = "Hire", action = "Index" });
            routes.MapRoute(Routes.Style, url: "style", defaults: new { controller = "Style", action = "Index" });
            routes.MapRoute(Routes.ContactUs, url: "contact-us", defaults: new { controller = "ContactUs", action = "Index" });
            routes.MapRoute(Routes.Blog, url: "blog", defaults: new { controller = "Blog", action = "Index" });


            routes.MapRoute(Routes.Buy.Index, url: "buy", defaults: new { controller = "Buy", action = "Index" });
            routes.MapRoute(Routes.Buy.Category, url: "buy/{categoryName}", defaults: new { controller = "Buy", action = "Category" });


            routes.MapRoute(Routes.Product.Index, url: "product/{name}/{productId}", defaults: new { controller = "Product", action = "Index" });
            routes.MapRoute(Routes.Product.Preview, url: "product/preview/{productId}-{name}", defaults: new { controller = "Product", action = "Preview" });

            routes.MapRoute(Routes.StyleProduct, url: "styles/{name}/{styleId}", defaults: new { controller = "Style", action = "Style" });

            routes.MapRoute(Routes.HireCheckAvailability, url: "hire/check-availability", defaults: new { controller = "Hire", action = "CheckAvailability" });


            routes.MapRoute(Routes.ShoppingCart.Add, url: "shopping-cart/add/{name}/{productId}", defaults: new { controller = "ShoppingCart", action = "Add" });
            routes.MapRoute(Routes.ShoppingCart.Remove, url: "shopping-cart/remove/{name}/{cartItemId}", defaults: new { controller = "ShoppingCart", action = "Remove" });
            routes.MapRoute(Routes.ShoppingCart.PageHeader, url: "shopping-cart/get/page-header", defaults: new { controller = "ShoppingCart", action = "PageHeader" });


            routes.MapRoute(
                name: Routes.Home,
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
