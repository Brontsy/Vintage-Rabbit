using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Web.Models.Orders;

namespace Vintage.Rabbit.Web.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult Summary(Order order)
        {
            OrderViewModel viewModel = new OrderViewModel(order);

            return this.PartialView("Summary", viewModel);
        }
	}
}