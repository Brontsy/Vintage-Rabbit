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
            var orderIdValue = filterContext.Controller.ValueProvider.GetValue("orderGuid");
            if (orderIdValue != null)
            {
                Guid orderId = new Guid(orderIdValue.AttemptedValue);

                if (this.QueryDispatcher.Dispatch<Order, GetOrderQuery>(new GetOrderQuery(orderId)) == null)
                {
                    filterContext.Result = new RedirectToRouteResult(Routes.Checkout.Index, new System.Web.Routing.RouteValueDictionary());
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(Routes.Checkout.Index, new System.Web.Routing.RouteValueDictionary());
            }
        }
    }
}