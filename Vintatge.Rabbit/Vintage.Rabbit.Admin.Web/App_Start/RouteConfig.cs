﻿using System;
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
            public static string InventoryAdd = "Product - Inventory Add";
            public static string InventoryDelete = "Product - Inventory Delete";
        }

        public static class Blog
        {
            public static string Index = "Blog Index";
            public static string View = "Blog View";
        }

        public static class Membership
        {
            public static string Login = "Member Login";
            public static string Logout = "Member Logout";
        }

        public static class Themes
        {
            public static string Index = "Themes - Index";
            public static string Add = "Themes - Add";
            public static string Edit = "Themes - Edit";
            public static string Save = "Themes - Save";
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
            routes.MapRoute(Routes.Products.InventoryAdd, url: "products/{productId}/inventory/add", defaults: new { controller = "Products", action = "InventoryAdd" });
            routes.MapRoute(Routes.Products.InventoryDelete, url: "products/{productId}/inventory/delete/{inventoryGuid}", defaults: new { controller = "Products", action = "InventoryDelete" });

            routes.MapRoute(Routes.Membership.Login, url: "login", defaults: new { controller = "Membership", action = "Login" });
            routes.MapRoute(Routes.Membership.Logout, url: "logout", defaults: new { controller = "Membership", action = "Logout" });


            routes.MapRoute(Routes.Blog.Index, url: "blog", defaults: new { controller = "Blog", action = "Index" });
            routes.MapRoute(Routes.Blog.View, url: "blog/{blogId}", defaults: new { controller = "Blog", action = "View" });

            routes.MapRoute(Routes.Themes.Index, url: "themes", defaults: new { controller = "Themes", action = "Index" });
            routes.MapRoute(Routes.Themes.Add, url: "themes/add", defaults: new { controller = "Themes", action = "Add" });
            routes.MapRoute(Routes.Themes.Edit, url: "themes/edit/{themeGuid}", defaults: new { controller = "Themes", action = "Edit" });
            routes.MapRoute(Routes.Themes.Save, url: "themes/save/{themeGuid}", defaults: new { controller = "Themes", action = "Save" });


            routes.MapRoute(name: Routes.Home, url: "", defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
