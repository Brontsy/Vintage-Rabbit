using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Membership.Entities;

namespace Vintage.Rabbit.Web.ModelBinders
{
    [ModelBinderType(typeof(Member))]
    public class MemberModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //do implementation here

             var cookie = controllerContext.RequestContext.HttpContext.Request.Cookies["MemberGuid"];

            if(cookie != null)
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