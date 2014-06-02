using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.QueryHandlers;

namespace Vintage.Rabbit.Web.Attributes
{
    public class HasOrderAttribute : ActionFilterAttribute
    {
        public IQueryDispatcher QueryDispatcher { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = filterContext.RequestContext.HttpContext.Request.Cookies["OrderGuid"];

            if (cookie == null)
            {
                filterContext.Result = new RedirectToRouteResult(Routes.Checkout.Index, new System.Web.Routing.RouteValueDictionary());
            }
            else
            {
                Guid orderId = new Guid(cookie.Value);

                Order order = this.QueryDispatcher.Dispatch<Order, GetOrderQuery>(new GetOrderQuery(orderId));
                if(order == null)
                {
                    filterContext.Result = new RedirectToRouteResult(Routes.Checkout.Index, new System.Web.Routing.RouteValueDictionary());
                }
            }
        }
    }
}