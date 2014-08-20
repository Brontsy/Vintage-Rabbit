using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            return View("FourZeroFour");
        }

        public ActionResult Unknown()
        {
            return View("FiveZeroZero");
        }
	}
}