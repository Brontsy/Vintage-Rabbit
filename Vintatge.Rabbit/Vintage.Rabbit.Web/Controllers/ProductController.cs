﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Controllers
{
    public class ProductController : Controller
    {

        public ActionResult Index(string name, int productId)
        {
            return View(name);
        }
    }
}
