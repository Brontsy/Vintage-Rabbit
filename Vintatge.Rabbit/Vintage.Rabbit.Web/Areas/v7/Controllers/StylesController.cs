﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Areas.v7.Controllers
{
    public class StylesController : Controller
    {
        //
        // GET: /v7/Styles/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Style(string styleId, string name)
        {
            return View(name);
        }
    }
}
