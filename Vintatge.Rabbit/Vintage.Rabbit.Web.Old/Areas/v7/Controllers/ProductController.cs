using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Old.Areas.v7.Controllers
{
    public class Product1Controller : Controller
    {

        public ActionResult Index(string name, int productId)
        {
            return View(name);
        }
    }
}
