using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.QueryHandlers;

namespace Vintage.Rabbit.Web.ModelBinders
{
    [ModelBinderType(typeof(Cart))]
    public class CartModelBinder : IModelBinder
    {
        private IQueryDispatcher _queryDispatcher;

        public CartModelBinder(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext.HttpContext.User.Identity.IsAuthenticated)
            {
                Guid memberGuid = new Guid(controllerContext.HttpContext.User.Identity.Name);
                return this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(memberGuid));
            }

            var cookie = controllerContext.RequestContext.HttpContext.Request.Cookies["MemberGuid"];

            if (cookie != null)
            {
                Guid memberGuid = new Guid(cookie.Value);

                return this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(memberGuid));
            }

            return null;
        }
    }
}