using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;

namespace Vintage.Rabbit.Web.ModelBinders
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
            else
            {
                var cookie = controllerContext.RequestContext.HttpContext.Request.Cookies["MemberGuid"];

                if (cookie != null)
                {
                    return new Member(new Guid(cookie.Value));
                }
                else
                {
                    Guid memberId = Guid.NewGuid();
                    HttpCookie myCookie = new HttpCookie("MemberGuid");

                    // Set the cookie value.
                    myCookie.Value = memberId.ToString();
                    // Set the cookie expiration date.
                    myCookie.Expires = DateTime.Now.AddYears(1);

                    controllerContext.HttpContext.Response.Cookies.Add(myCookie);
                    return new Member(memberId);
                }
            }
        }
    }
}