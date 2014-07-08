using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.QueryHandlers;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Web.Areas.Emails.Models.InvoiceEmail;

namespace Vintage.Rabbit.Web.Areas.Emails.Controllers
{
    public class InvoiceEmailController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private string _websiteUrl;

        public InvoiceEmailController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            this._websiteUrl = System.Configuration.ConfigurationManager.AppSettings["Website_Url"];
        }

        public ActionResult Index(Guid orderGuid, int orderId)
        {
            Order order = this._queryDispatcher.Dispatch<Order, GetOrderQuery>(new GetOrderQuery(orderGuid));

            if (order == null || order.Id != orderId)
            {
                throw new Exception("Order Id and Order Guid do not match");
            }

            InvoiceEmailViewModel viewModel = new InvoiceEmailViewModel(order);

            foreach(var orderItem in order.Items)
            {
                var product = this._queryDispatcher.Dispatch<Product, GetProductByGuidQuery>(new GetProductByGuidQuery(orderItem.Product.Guid));
                viewModel.OrderItems.Add(new OrderItemViewModel(orderItem, product));
            }

            ViewBag.WebsiteUrl = this._websiteUrl;

            return View("InvoiceEmail", viewModel);
        }
	}
}