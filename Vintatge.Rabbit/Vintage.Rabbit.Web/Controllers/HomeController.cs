using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(string version)
        {
            ViewBag.Version = version;
            return View(string.Format("Index", version));
        }

    }
}
