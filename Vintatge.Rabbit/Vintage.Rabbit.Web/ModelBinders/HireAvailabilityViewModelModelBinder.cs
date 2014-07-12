using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Web.Models.Hire;

namespace Vintage.Rabbit.Web.ModelBinders
{
    [ModelBinderType(typeof(HireAvailabilityViewModel))]
    public class HireAvailabilityViewModelModelBinder : IModelBinder
    {        
        private IQueryDispatcher _queryDispatcher;

        public HireAvailabilityViewModelModelBinder(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            bool isValidPostcode = false;
            string postcode = null;

            ValueProviderResult postcodeCheck = controllerContext.Controller.ValueProvider.GetValue("PostcodeCheck");

            if (postcodeCheck != null)
            {
                postcode = postcodeCheck.AttemptedValue;

                HttpCookie myCookie = new HttpCookie("PostcodeCheck");

                myCookie.Value = postcode;
                myCookie.Expires = DateTime.Now.AddDays(14);
                controllerContext.HttpContext.Response.Cookies.Add(myCookie);
            }
            else
            {
                var cookie = controllerContext.RequestContext.HttpContext.Request.Cookies["PostcodeCheck"];

                if (cookie != null)
                {
                    postcode = cookie.Value;
                }
            }

            if(!string.IsNullOrEmpty(postcode))
            {
                isValidPostcode = isValidPostcode = this._queryDispatcher.Dispatch<bool, IsValidHirePostcodeQuery>(new IsValidHirePostcodeQuery(postcode)); 
            }

            return new HireAvailabilityViewModel(postcode, isValidPostcode);
        }
    }
}