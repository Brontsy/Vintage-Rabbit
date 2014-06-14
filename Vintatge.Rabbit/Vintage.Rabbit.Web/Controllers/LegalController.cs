﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Controllers
{
    public class LegalController : Controller
    {
        public ActionResult TermsAndConditions()
        {
            return View("TermsAndConditions");
        }

        public ActionResult PrivacyPolicy()
        {
            return View("PrivacyPolicy");
        }
	}
}