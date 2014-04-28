using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Interfaces.CQRS;

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
            var cart = this._queryDispatcher.Dispatch<Cart, GetCartQuery>(new GetCartQuery(Guid.NewGuid()));
            return View();
        }
    }
}