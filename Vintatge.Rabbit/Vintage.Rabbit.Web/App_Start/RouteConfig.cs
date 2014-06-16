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
        public static string Themes = "Themes";
        public static string ContactUs = "ContactUs";
        public static string FAQ = "FAQ";
        public static string Blog = "Blog";
        public static string TermsAndConditions = "TermsAndConditions";
        public static string PrivacyPolicy = "PrivacyPolicy";

        public static string StyleProduct = "StyleProduct";

        public static class ShoppingCart
        {
            public static string PageHeader = "ShoppingCartPageHeader";
            public static string Add = "ShoppingCartAdd";
            public static string AddHireProduct = "ShoppingCartAddHireProduct";
            public static string Remove = "ShoppingCartRemove";

        }

        public static class Checkout
        {
            public static string Index = "Checkout";
            public static string LoginRegister = "Checkout - LoginRegister";
            public static string Guest = "Checkout - Guest";
            public static string Devliery = "Checkout - Devliery";
            public static string PickupHiredProducts = "Checkout - PickupHiredProducts";
            public static string ShippingInformation = "Checkout - ShippingInformation";
            public static string BillingInformation = "Checkout - BillingInformation";
            public static string PaymentInfo = "Checkout - PaymentInfo";
            public static string CreditCard = "Checkout - CreditCard";
            public static string PayPal = "Checkout - PayPal";
            public static string Complete = "Checkout - Complete";

        }

        public static class Buy
        {
            public static string Index = "Buy";
            public static string Category = "Buy-Category";
            public static string PartySupplies = "Buy-PartySupplies";
            public static string Gifts = "Buy-Gifts";
            public static string Games = "Buy-Games";
            public static string Product = "Buy-Product";
            public static string Preview = "Buy-Preview";

        }
        public static class Hire
        {
            public static string Index = "Hire";
            public static string Category = "Hire - Category";
            public static string Product = "HireProduct";
            public static string Preview = "HirePreview";
            public static string CheckProductAvailability = "CheckProductAvailability";
        }

        public static class Membership
        {
            public static string Register = "Membership - Register";
            public static string Login = "Membership - Login";
            public static string Logout = "Membership - Logout";
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(Routes.Themes, url: "style", defaults: new { controller = "Style", action = "Index" });
            routes.MapRoute(Routes.ContactUs, url: "contact-us", defaults: new { controller = "ContactUs", action = "Index" });
            routes.MapRoute(Routes.FAQ, url: "faq", defaults: new { controller = "FAQ", action = "Index" });
            routes.MapRoute(Routes.Blog, url: "blog", defaults: new { controller = "Blog", action = "Index" });
            routes.MapRoute(Routes.TermsAndConditions, url: "terms-and-conditions", defaults: new { controller = "Legal", action = "TermsAndConditions" });
            routes.MapRoute(Routes.PrivacyPolicy, url: "privacy-policy", defaults: new { controller = "Legal", action = "PrivacyPolicy" });



            routes.MapRoute(Routes.Hire.Index, url: "hire", defaults: new { controller = "Hire", action = "Index" });
            routes.MapRoute(Routes.Hire.Product, url: "hire/{name}/{productId}", defaults: new { controller = "Hire", action = "Product" });
            routes.MapRoute(Routes.Hire.Preview, url: "hire/preview/{name}/{productId}", defaults: new { controller = "Hire", action = "Preview" });
            routes.MapRoute(Routes.Hire.CheckProductAvailability, url: "hire/availability/{name}/{productGuid}", defaults: new { controller = "Hire", action = "CheckProductAvailability" });
            routes.MapRoute(Routes.Hire.Category, url: "hire/{categoryName}", defaults: new { controller = "Hire", action = "Category" });

            routes.MapRoute(Routes.Checkout.Index, url: "checkout", defaults: new { controller = "Payment", action = "Index" });
            routes.MapRoute(Routes.Checkout.LoginRegister, url: "checkout/register", defaults: new { controller = "Payment", action = "LoginRegister" });
            routes.MapRoute(Routes.Checkout.Guest, url: "checkout/guest", defaults: new { controller = "Payment", action = "Guest" });
            routes.MapRoute(Routes.Checkout.Devliery, url: "checkout/hire/delivery-address", defaults: new { controller = "Payment", action = "Delivery" });
            routes.MapRoute(Routes.Checkout.PickupHiredProducts, url: "checkout/hire/pickup", defaults: new { controller = "Payment", action = "PickupHiredProducts" });
            routes.MapRoute(Routes.Checkout.ShippingInformation, url: "checkout/buy/shipping-address", defaults: new { controller = "Payment", action = "ShippingInformation" });
            routes.MapRoute(Routes.Checkout.BillingInformation, url: "checkout/billing/address", defaults: new { controller = "Payment", action = "BillingInformation" });
            routes.MapRoute(Routes.Checkout.PaymentInfo, url: "checkout/payment", defaults: new { controller = "Payment", action = "PaymentInfo" });
            routes.MapRoute(Routes.Checkout.CreditCard, url: "checkout/credit-card", defaults: new { controller = "Payment", action = "CreditCard" });
            routes.MapRoute(Routes.Checkout.PayPal, url: "checkout/paypal", defaults: new { controller = "Payment", action = "PayPal" });
            routes.MapRoute(Routes.Checkout.Complete, url: "checkout/complete", defaults: new { controller = "Payment", action = "Complete" });


            routes.MapRoute(Routes.Membership.Register, url: "register", defaults: new { controller = "Membership", action = "Register" });
            routes.MapRoute(Routes.Membership.Login, url: "login", defaults: new { controller = "Membership", action = "Login" });
            routes.MapRoute(Routes.Membership.Logout, url: "logout", defaults: new { controller = "Membership", action = "Logout" });


            routes.MapRoute(Routes.StyleProduct, url: "styles/{name}/{styleId}", defaults: new { controller = "Style", action = "Style" });


            routes.MapRoute(Routes.ShoppingCart.Add, url: "shopping-cart/add/{name}/{productId}", defaults: new { controller = "ShoppingCart", action = "Add" });
            routes.MapRoute(Routes.ShoppingCart.AddHireProduct, url: "shopping-cart/hire/add/{name}/{productId}", defaults: new { controller = "ShoppingCart", action = "AddHireProduct" });
            routes.MapRoute(Routes.ShoppingCart.Remove, url: "shopping-cart/remove/{name}/{cartItemId}", defaults: new { controller = "ShoppingCart", action = "Remove" });
            routes.MapRoute(Routes.ShoppingCart.PageHeader, url: "shopping-cart/get/page-header", defaults: new { controller = "ShoppingCart", action = "PageHeader" });


            routes.MapRoute(Routes.Buy.Index, url: "buy", defaults: new { controller = "Buy", action = "Index" });
            routes.MapRoute(Routes.Buy.PartySupplies, url: "party-supplies", defaults: new { controller = "Buy", action = "Category", categoryName = "party-supplies" });
            routes.MapRoute(Routes.Buy.Gifts, url: "gifts", defaults: new { controller = "Buy", action = "Category", categoryName = "gifts" });
            routes.MapRoute(Routes.Buy.Games, url: "games", defaults: new { controller = "Buy", action = "Category", categoryName = "games" });
            routes.MapRoute(Routes.Buy.Product, url: "{categoryName}/{name}/{productId}", defaults: new { controller = "Buy", action = "Product" });
            routes.MapRoute(Routes.Buy.Preview, url: "{categoryName}/preview/{name}/{productId}", defaults: new { controller = "Buy", action = "Preview" });
            routes.MapRoute(Routes.Buy.Category, url: "{categoryName}", defaults: new { controller = "Buy", action = "Category" });

            routes.MapRoute(name: Routes.Home, url: "", defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
