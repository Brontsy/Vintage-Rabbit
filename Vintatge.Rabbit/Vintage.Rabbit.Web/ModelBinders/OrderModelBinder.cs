using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.QueryHandlers;

namespace Vintage.Rabbit.Web.ModelBinders
{
    [ModelBinderType(typeof(Order))]
    public class OrderModelBinder : IModelBinder
    {
        private IQueryDispatcher _queryDispatcher;

        public OrderModelBinder(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var orderIdValue = controllerContext.Controller.ValueProvider.GetValue("orderGuid");
            if(orderIdValue != null)
            {
                Guid orderId = new Guid(orderIdValue.AttemptedValue);

                return this._queryDispatcher.Dispatch<Order, GetOrderQuery>(new GetOrderQuery(orderId));
            }

            //var cookie = controllerContext.RequestContext.HttpContext.Request.Cookies["OrderGuid"];

            //if (cookie != null)
            //{
            //    Guid orderId = new Guid(cookie.Value);

            //    return this._queryDispatcher.Dispatch<Order, GetOrderQuery>(new GetOrderQuery(orderId));
            //}

            return null;
        }
    }
}