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
            public static string Type = "Product Type";
            public static string Edit = "Product - Edit";
            public static string AddProduct = "Product - Add";
            public static string SaveProduct = "Product - Save";
            public static string UploadPhoto = "Product - Upload Photo";
            public static string RemovePhoto = "Product - Remove Photo";
            public static string Inventory = "Product - Inventory";
        }

        public static class Blog
        {
            public static string Index = "Blog Index";
            public static string Add = "Blog Add";
            public static string Edit = "Blog Edit";
            public static string Save = "Blog Save";
            public static string UploadPhoto = "Blog - Upload Photo";
            public static string RemovePhoto = "Blog - Remove Photo";
        }

        public static class Membership
        {
            public static string Login = "Member Login";
            public static string Logout = "Member Logout";
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(Routes.Products.AddProduct, url: "products/add", defaults: new { controller = "Products", action = "Add" });
            routes.MapRoute(Routes.Products.List, url: "products", defaults: new { controller = "Products", action = "List" });
            routes.MapRoute(Routes.Products.Type, url: "products/{productType}", defaults: new { controller = "Products", action = "List" });
            routes.MapRoute(Routes.Products.SaveProduct, url: "products/save/{productId}", defaults: new { controller = "Products", action = "Save" });
            routes.MapRoute(Routes.Products.Edit, url: "products/edit/{name}/{productId}", defaults: new { controller = "Products", action = "Edit" });
            routes.MapRoute(Routes.Products.UploadPhoto, url: "products/upload-photo/{productId}", defaults: new { controller = "Products", action = "UploadPhoto" });
            routes.MapRoute(Routes.Products.RemovePhoto, url: "products/{productId}/remove-photo/{photoId}", defaults: new { controller = "Products", action = "RemovePhoto" });
            routes.MapRoute(Routes.Products.Inventory, url: "products/{productId}/inventory", defaults: new { controller = "Products", action = "Inventory" });

            routes.MapRoute(Routes.Membership.Login, url: "login", defaults: new { controller = "Membership", action = "Login" });
            routes.MapRoute(Routes.Membership.Logout, url: "logout", defaults: new { controller = "Membership", action = "Logout" });


            routes.MapRoute(Routes.Blog.Index, url: "blog", defaults: new { controller = "Blog", action = "Index" });
            routes.MapRoute(Routes.Blog.Add, url: "blog/add", defaults: new { controller = "Blog", action = "Add" });
            routes.MapRoute(Routes.Blog.Edit, url: "blog/edit/{key}/{blogId}", defaults: new { controller = "Blog", action = "Edit" });
            routes.MapRoute(Routes.Blog.Save, url: "blog/save/{blogId}", defaults: new { controller = "Blog", action = "Save" });
            routes.MapRoute(Routes.Blog.UploadPhoto, url: "blog/upload-photo/{key}/{blogId}", defaults: new { controller = "Blog", action = "UploadPhoto" });
            routes.MapRoute(Routes.Blog.RemovePhoto, url: "blog/{key}/{blogId}/remove-photo/{photoId}", defaults: new { controller = "Blog", action = "RemovePhoto" });


            routes.MapRoute(name: Routes.Home, url: "", defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
