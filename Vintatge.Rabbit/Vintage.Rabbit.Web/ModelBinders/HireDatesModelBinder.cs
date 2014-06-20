using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Web.Models.Hire;

namespace Vintage.Rabbit.Web.ModelBinders
{
    [ModelBinderType(typeof(HireDatesViewModel))]
    public class HireDatesModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //do implementation here

            ValueProviderResult startResult = controllerContext.Controller.ValueProvider.GetValue("startDate");
            ValueProviderResult endResult = controllerContext.Controller.ValueProvider.GetValue("endDate");
            ValueProviderResult partyDateResult = controllerContext.Controller.ValueProvider.GetValue("partyDate");

            if (partyDateResult != null)//startResult != null && endResult != null)
            {
                DateTime startDate;
                DateTime endDate;
                DateTime partyDate;
                if (DateTime.TryParse(partyDateResult.AttemptedValue, out partyDate)) //DateTime.TryParse(startResult.AttemptedValue, out startDate) && DateTime.TryParse(endResult.AttemptedValue, out endDate))
                {
                    HttpCookie myCookie = new HttpCookie("HireDatesViewModel");

                    // Set the cookie value.
                    myCookie.Value = partyDateResult.ToString();// startDate.ToString() + "|" + endDate.ToString();
                    // Set the cookie expiration date.
                    myCookie.Expires = DateTime.Now.AddDays(14);

                    controllerContext.HttpContext.Response.Cookies.Add(myCookie);

                    return new HireDatesViewModel(partyDate);
                }
            }
            else
            {
                var cookie = controllerContext.RequestContext.HttpContext.Request.Cookies["HireDatesViewModel"];

                if (cookie != null)
                {
                    //string[] dates = cookie.Value.Split('|');
                    DateTime startDate;
                    DateTime endDate;
                    DateTime partyDate;

                    if (DateTime.TryParse(cookie.Value, out partyDate)) //DateTime.TryParse(dates[0], out startDate) && DateTime.TryParse(dates[1], out endDate))
                    {
                        return new HireDatesViewModel(partyDate);
                    }
                }
            }
                
            return new HireDatesViewModel(null);
        }
    }
}