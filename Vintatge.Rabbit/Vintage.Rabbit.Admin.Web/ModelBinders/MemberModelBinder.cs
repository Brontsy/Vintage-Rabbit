using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;

namespace Vintage.Rabbit.Admin.Web.ModelBinders
{
    [ModelBinderType(typeof(Member))]
    public class MemberModelBinder : IModelBinder
    {
        private IQueryDispatcher _queryDispatcher;

        public MemberModelBinder(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext.HttpContext.User.Identity.IsAuthenticated)
            {
                Guid memberId = new Guid(controllerContext.HttpContext.User.Identity.Name);
                return this._queryDispatcher.Dispatch<Member, GetMemberByIdQuery>(new GetMemberByIdQuery(memberId));
            }

            return null;
        }
    }
}