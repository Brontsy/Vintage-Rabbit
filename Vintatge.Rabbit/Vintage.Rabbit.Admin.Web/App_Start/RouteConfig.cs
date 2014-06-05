using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vintage.Rabbit.Admin.Web
{
    public class Routes
    {
        public static string Home = "Home";
        public static class Products
        {
            public static string List = "Product List";
            public static string Buy = "Product Buy";
            public static string Hire = "Product Hire";
            public static string Edit = "Product - Edit";
            public static string AddProduct = "Product - Add";
            public static string SaveProduct = "Product - Save";
            public static string UploadPhoto = "Product - Upload Photo";
        }

        public static class Membership
        {
            public static string Login = "Member Login";
            public static string Logout = "Member Logout";
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(Routes.Products.List, url: "products", defaults: new { controller = "Products", action = "BuyList" });
            routes.MapRoute(Routes.Products.Buy, url: "products/buy", defaults: new { controller = "Products", action = "BuyList" });
            routes.MapRoute(Routes.Products.Hire, url: "products/hire", defaults: new { controller = "Products", action = "HireList" });
            routes.MapRoute(Routes.Products.AddProduct, url: "products/add", defaults: new { controller = "Products", action = "Add" });
            routes.MapRoute(Routes.Products.SaveProduct, url: "products/save/{productId}", defaults: new { controller = "Products", action = "Save" });
            routes.MapRoute(Routes.Products.Edit, url: "products/edit/{name}/{productId}", defaults: new { controller = "Products", action = "Edit" });
            routes.MapRoute(Routes.Products.UploadPhoto, url: "products/upload-photo/{productId}", defaults: new { controller = "Products", action = "UploadPhoto" });

            routes.MapRoute(Routes.Membership.Login, url: "login", defaults: new { controller = "Membership", action = "Login" });
            routes.MapRoute(Routes.Membership.Logout, url: "logout", defaults: new { controller = "Membership", action = "Logout" });

            routes.MapRoute(name: Routes.Home, url: "", defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
