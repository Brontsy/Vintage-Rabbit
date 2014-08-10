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
    public class OrderIsValidAttribute : ActionFilterAttribute
    {
        public IQueryDispatcher QueryDispatcher { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var orderIdValue = filterContext.Controller.ValueProvider.GetValue("orderGuid");
            if (orderIdValue != null)
            {
                Guid orderId = new Guid(orderIdValue.AttemptedValue);

                Order order = this.QueryDispatcher.Dispatch<Order, GetOrderQuery>(new GetOrderQuery(orderId));

                if (order == null)
                {
                    filterContext.Result = new RedirectToRouteResult(Routes.Checkout.Index, new System.Web.Routing.RouteValueDictionary());
                }
                else
                {
                    Guid? memberGuid = this.GetMemberGuid(filterContext.HttpContext);

                    if (memberGuid.HasValue && memberGuid.Value == order.MemberGuid)
                    {
                        if (order.Status == Orders.Enums.OrderStatus.Complete || order.Status == Orders.Enums.OrderStatus.AwaitingShipment)
                        {
                            var routeData = new System.Web.Routing.RouteValueDictionary();
                            routeData.Add("orderGuid", orderId);
                            filterContext.Result = new RedirectToRouteResult(Routes.Checkout.Complete, routeData);
                        }
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(Routes.Checkout.Index, new System.Web.Routing.RouteValueDictionary());
                    }
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(Routes.Checkout.Index, new System.Web.Routing.RouteValueDictionary());
            }
        }

        private Guid? GetMemberGuid(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                return new Guid(httpContext.User.Identity.Name);
            }
            else
            {
                var cookie = httpContext.Request.Cookies["MemberGuid"];

                if (cookie != null)
                {
                    return new Guid(cookie.Value);
                }
            }

            return null;
        }
    }
}