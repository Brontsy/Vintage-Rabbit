using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Logging;
using Vintage.Rabbit.Products.Services;
using Vintage.Rabbit.Web.Attributes;

namespace Vintage.Rabbit.Web.Controllers
{
    public class HomeController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        public HomeController(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public ActionResult Index()
        {
            bool isLive = false;

            if(ConfigurationManager.AppSettings["IsLive"] != null)
            {
                bool.TryParse(ConfigurationManager.AppSettings["IsLive"], out isLive);
            }

            if (isLive || this.Request.Cookies["IsLive"] != null)
            {
                return View();
            }

            return View("ComingSoon");
        }
    }
}