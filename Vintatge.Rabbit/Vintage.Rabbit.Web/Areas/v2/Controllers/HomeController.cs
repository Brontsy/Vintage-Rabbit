using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Areas.v2.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /v1/Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
