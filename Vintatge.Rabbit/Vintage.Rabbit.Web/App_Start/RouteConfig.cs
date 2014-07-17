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
        public static string ContactUs = "ContactUs";
        public static string FAQ = "FAQ";
        public static string Blog = "Blog";
        public static string TermsAndConditions = "TermsAndConditions";
        public static string PrivacyPolicy = "PrivacyPolicy";
        public static string AboutUs = "AboutUs";
        public static string HowItWorks = "HowItWorks";

        public static string StyleProduct = "StyleProduct";

        public static class Themes
        {
            public static string Index = "Themes Index";
            public static string WizardOfOz = "Themes Wizard Of Oz";
            public static string Carnival = "Themes Carnival";
            public static string TeddyBearsPicnic = "Themes TeddyBearsPicnic";
            public static string TeaParty = "Themes TeaParty";
            public static string MaryPoppins = "Themes MaryPoppins";
            public static string Cinderella = "Themes Cinderella";
            public static string Product = "Themes Product";
        }

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
            public static string PayPalSuccess = "Checkout - PayPalSuccess";
            public static string PayPalCancel = "Checkout - PayPalCancel";
            public static string Complete = "Checkout - Complete";
        }

        public static class Order
        {
            public static string ApplyDiscount = "Order - Apply Discount";
            public static string Summary = "Order - Summary";
            
        }

        public static class Buy
        {
            public static string Index = "Buy";
            public static string Category = "Buy-Category";
            public static string CategoryPaged = "Buy-CategoryPaged";
            public static string CategoryChild = "Buy-Category-Child";
            public static string CategoryChildPaged = "Buy-Category-ChildPaged";
            public static string PartySupplies = "Buy-PartySupplies";
            public static string PartySuppliesPaged = "Buy-PartySuppliesPaged";
            public static string Gifts = "Buy-Gifts";
            public static string GiftsPaged = "Buy-GiftsPaged";
            public static string Games = "Buy-Games";
            public static string GamesPaged = "Buy-GamesPaged";
            public static string Product = "Buy-Product";
            public static string Preview = "Buy-Preview";

        }
        public static class Hire
        {
            public static string Index = "Hire";
            public static string IndexPaged = "Hire Paged";
            public static string Category = "Hire - Category";
            public static string CategoryPaged = "Hire - CategoryPaged";
            public static string Product = "HireProduct";
            public static string Preview = "HirePreview";
            public static string CheckProductAvailability = "CheckProductAvailability";
            public static string PostcodeCheck = "PostcodeCheck";
            public static string AvailabilityCheck = "AvailabilityCheck";
        }

        public static class Membership
        {
            public static string Register = "Membership - Register";
            public static string Login = "Membership - Login";
            public static string Logout = "Membership - Logout";
        }

        public static class Search
        {
            public static string Index = "Search - Index";
            public static string RebuildIndex = "Search - RebuildIndex";
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(Routes.ContactUs, url: "contact-us", defaults: new { controller = "FAQ", action = "ContactUs" });
            routes.MapRoute(Routes.FAQ, url: "faq", defaults: new { controller = "FAQ", action = "Index" });
            routes.MapRoute(Routes.Blog, url: "blog", defaults: new { controller = "Blog", action = "Index" });
            routes.MapRoute(Routes.TermsAndConditions, url: "terms-and-conditions", defaults: new { controller = "FAQ", action = "TermsAndConditions" });
            routes.MapRoute(Routes.PrivacyPolicy, url: "privacy-policy", defaults: new { controller = "FAQ", action = "PrivacyPolicy" });
            routes.MapRoute(Routes.AboutUs, url: "about-us", defaults: new { controller = "FAQ", action = "AboutUs" });
            routes.MapRoute(Routes.HowItWorks, url: "how-it-works", defaults: new { controller = "FAQ", action = "HowItWorks" });

            routes.MapRoute(Routes.Order.ApplyDiscount, url: "order/{orderGuid}/apply-discount", defaults: new { controller = "Order", action = "ApplyDiscount" });
            routes.MapRoute(Routes.Order.Summary, url: "order/{orderGuid}/summary", defaults: new { controller = "Order", action = "Summary" });

            routes.MapRoute(Routes.Themes.Index, url: "style", defaults: new { controller = "Style", action = "Index" });
            routes.MapRoute(Routes.Themes.WizardOfOz, url: "style/wizard-of-oz", defaults: new { controller = "Style", action = "WizardOfOz" });
            routes.MapRoute(Routes.Themes.Carnival, url: "style/carnival", defaults: new { controller = "Style", action = "Carnival" });
            routes.MapRoute(Routes.Themes.TeddyBearsPicnic, url: "style/teddy-bears-picnic", defaults: new { controller = "Style", action = "TeddyBearsPicnic" });
            routes.MapRoute(Routes.Themes.TeaParty, url: "style/tea-party", defaults: new { controller = "Style", action = "TeaParty" });
            routes.MapRoute(Routes.Themes.MaryPoppins, url: "style/mary-poppins", defaults: new { controller = "Style", action = "MaryPoppins" });
            routes.MapRoute(Routes.Themes.Cinderella, url: "style/cinderella", defaults: new { controller = "Style", action = "Cinderella" });
            routes.MapRoute(Routes.Themes.Product, url: "style/{name}/{productId}", defaults: new { controller = "Style", action = "Product" });

            routes.MapRoute(Routes.Hire.Index, url: "hire", defaults: new { controller = "Hire", action = "Index" });
            routes.MapRoute(Routes.Hire.IndexPaged, url: "hire/page/{page}", defaults: new { controller = "Hire", action = "Index" });
            routes.MapRoute(Routes.Hire.PostcodeCheck, url: "hire/postcode-check", defaults: new { controller = "Hire", action = "AvailabilityCheck", postcodeChecked = true });
            routes.MapRoute(Routes.Hire.AvailabilityCheck, url: "hire/availability-check/{productGuid}", defaults: new { controller = "Hire", action = "AvailabilityCheck" });
            routes.MapRoute(Routes.Hire.CheckProductAvailability, url: "hire/availability/{name}/{productGuid}", defaults: new { controller = "Hire", action = "CheckProductAvailability" });
            routes.MapRoute(Routes.Hire.Preview, url: "hire/preview/{categoryName}/{name}/{productId}", defaults: new { controller = "Hire", action = "Preview" });
            routes.MapRoute(Routes.Hire.Category, url: "hire/{categoryName}", defaults: new { controller = "Hire", action = "Category" });
            routes.MapRoute(Routes.Hire.CategoryPaged, url: "hire/{categoryName}/page/{page}", defaults: new { controller = "Hire", action = "Category" });
            routes.MapRoute(Routes.Hire.Product, url: "hire/{categoryName}/{name}/{productId}", defaults: new { controller = "Hire", action = "Product" });

            routes.MapRoute(Routes.Checkout.Index, url: "checkout", defaults: new { controller = "Payment", action = "Index" });
            routes.MapRoute(Routes.Checkout.LoginRegister, url: "checkout/register", defaults: new { controller = "Payment", action = "LoginRegister" });
            routes.MapRoute(Routes.Checkout.Guest, url: "checkout/guest", defaults: new { controller = "Payment", action = "Guest" });
            routes.MapRoute(Routes.Checkout.Devliery, url: "checkout-{orderGuid}/hire/delivery-address/{guid}", defaults: new { controller = "Payment", action = "Delivery", guid = UrlParameter.Optional });
            routes.MapRoute(Routes.Checkout.PickupHiredProducts, url: "checkout-{orderGuid}/hire/pickup", defaults: new { controller = "Payment", action = "PickupHiredProducts" });
            routes.MapRoute(Routes.Checkout.ShippingInformation, url: "checkout-{orderGuid}/buy/shipping-address/{guid}", defaults: new { controller = "Payment", action = "ShippingInformation", guid = UrlParameter.Optional });
            routes.MapRoute(Routes.Checkout.BillingInformation, url: "checkout-{orderGuid}/billing/address/{guid}", defaults: new { controller = "Payment", action = "BillingInformation", guid = UrlParameter.Optional });
            routes.MapRoute(Routes.Checkout.PaymentInfo, url: "checkout-{orderGuid}/payment", defaults: new { controller = "Payment", action = "PaymentInfo" });
            routes.MapRoute(Routes.Checkout.CreditCard, url: "checkout-{orderGuid}/credit-card", defaults: new { controller = "Payment", action = "CreditCard" });
            routes.MapRoute(Routes.Checkout.PayPal, url: "checkout-{orderGuid}/paypal", defaults: new { controller = "Payment", action = "PayPal" });
            routes.MapRoute(Routes.Checkout.PayPalSuccess, url: "checkout-{orderGuid}/paypal/success/{paypalPaymentGuid}", defaults: new { controller = "Payment", action = "PayPalSuccess" });
            routes.MapRoute(Routes.Checkout.PayPalCancel, url: "checkout-{orderGuid}/paypal/cancel/{paypalPaymentGuid}", defaults: new { controller = "Payment", action = "PayPalCancel" });
            routes.MapRoute(Routes.Checkout.Complete, url: "checkout-{orderGuid}/complete", defaults: new { controller = "Payment", action = "Complete" });


            routes.MapRoute(Routes.Membership.Register, url: "register", defaults: new { controller = "Membership", action = "Register" });
            routes.MapRoute(Routes.Membership.Login, url: "login", defaults: new { controller = "Membership", action = "Login" });
            routes.MapRoute(Routes.Membership.Logout, url: "logout", defaults: new { controller = "Membership", action = "Logout" });

            routes.MapRoute(Routes.Search.Index, url: "search", defaults: new { controller = "Search", action = "Index" });
            routes.MapRoute(Routes.Search.RebuildIndex, url: "search/rebuild/index", defaults: new { controller = "Search", action = "RebuildIndex" });


            routes.MapRoute(Routes.StyleProduct, url: "styles/{name}/{styleId}", defaults: new { controller = "Style", action = "Style" });


            routes.MapRoute(Routes.ShoppingCart.Add, url: "shopping-cart/add/{name}/{productId}", defaults: new { controller = "ShoppingCart", action = "Add" });
            routes.MapRoute(Routes.ShoppingCart.AddHireProduct, url: "shopping-cart/hire/add/{name}/{productId}", defaults: new { controller = "ShoppingCart", action = "AddHireProduct" });
            routes.MapRoute(Routes.ShoppingCart.Remove, url: "shopping-cart/remove/{name}/{cartItemId}", defaults: new { controller = "ShoppingCart", action = "Remove" });
            routes.MapRoute(Routes.ShoppingCart.PageHeader, url: "shopping-cart/get/page-header", defaults: new { controller = "ShoppingCart", action = "PageHeader" });


            routes.MapRoute(Routes.Buy.Index, url: "buy", defaults: new { controller = "Buy", action = "Index" });
            routes.MapRoute(Routes.Buy.PartySupplies, url: "party-supplies", defaults: new { controller = "Buy", action = "Category", categoryName = "party-supplies" });
            routes.MapRoute(Routes.Buy.PartySuppliesPaged, url: "party-supplies/page/{page}", defaults: new { controller = "Buy", action = "Category", categoryName = "party-supplies" });
            routes.MapRoute(Routes.Buy.Gifts, url: "gifts", defaults: new { controller = "Buy", action = "Category", categoryName = "gifts" });
            routes.MapRoute(Routes.Buy.GiftsPaged, url: "gifts/page/{page}", defaults: new { controller = "Buy", action = "Category", categoryName = "gifts" });
            routes.MapRoute(Routes.Buy.Games, url: "games", defaults: new { controller = "Buy", action = "Category", categoryName = "games" });
            routes.MapRoute(Routes.Buy.GamesPaged, url: "games/page/{page}", defaults: new { controller = "Buy", action = "Category", categoryName = "games" });
            routes.MapRoute(Routes.Buy.Product, url: "{categoryName}/{name}/{productId}", defaults: new { controller = "Buy", action = "Product" });
            routes.MapRoute(Routes.Buy.Preview, url: "{categoryName}/preview/{name}/{productId}", defaults: new { controller = "Buy", action = "Preview" });
            routes.MapRoute(Routes.Buy.Category, url: "{categoryName}", defaults: new { controller = "Buy", action = "Category" });
            routes.MapRoute(Routes.Buy.CategoryPaged, url: "{categoryName}/page/{page}", defaults: new { controller = "Buy", action = "Category" });
            routes.MapRoute(Routes.Buy.CategoryChild, url: "{categoryName}/{childCategoryName}", defaults: new { controller = "Buy", action = "Category" });
            routes.MapRoute(Routes.Buy.CategoryChildPaged, url: "{categoryName}/{childCategoryName}/page/{page}", defaults: new { controller = "Buy", action = "Category" });



            routes.MapRoute(name: Routes.Home, url: "", defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
