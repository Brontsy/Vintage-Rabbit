using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Controllers
{
    public class FAQController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HowItWorks()
        {
            return View("Index");
        }

        public ActionResult TermsAndConditions()
        {
            return View("TermsAndConditions");
        }

        public ActionResult PrivacyPolicy()
        {
            return View("PrivacyPolicy");
        }

        public ActionResult ContactUs()
        {
            return View("ContactUs");
        }

        public ActionResult AboutUs()
        {
            return View("AboutUs");
        }
	}
}