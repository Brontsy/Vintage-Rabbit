using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Old.Controllers
{
    public class HireController : Controller
    {
        //
        // GET: /Hire/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckAvailability()
        {
            return View();
        }
    }
}
