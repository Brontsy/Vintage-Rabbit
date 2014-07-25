using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Areas.Emails.Controllers
{
    public class ContactUsEmailController : Controller
    {
        private string _websiteUrl;

        public ContactUsEmailController()
        {
            this._websiteUrl = System.Configuration.ConfigurationManager.AppSettings["Website_Url"];
        }

        public ActionResult Index(string name, string email, string comments)
        {
            ViewBag.Name = name;
            ViewBag.Email = email;
            ViewBag.Comments = comments;

            ViewBag.WebsiteUrl = this._websiteUrl;

            return View("ContactUsEmail");
        }
	}
}