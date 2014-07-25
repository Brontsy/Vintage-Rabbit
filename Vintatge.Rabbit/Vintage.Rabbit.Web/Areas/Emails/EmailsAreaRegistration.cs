using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Areas.Emails
{
    public class EmailsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Emails";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("InvoiceEmail", "email/invoice/{orderGuid}/{orderId}", new { controller = "InvoiceEmail", action = "Index" });
            context.MapRoute("ContactUsEmail", "email/contact-us", new { controller = "ContactUsEmail", action = "Index" });

            context.MapRoute("Emails_default", "Emails/{controller}/{action}/{id}", new { action = "Index", id = UrlParameter.Optional });
        }
    }
}