using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Logging;

namespace Vintage.Rabbit.Web.Admin.Attributes
{
    public class HandleExceptionAttribute : HandleErrorAttribute
    {
        public ILogger Logger { get; set; }

        public HandleExceptionAttribute(ILogger logger)
        {
            this.Logger = logger;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            this.Logger.Error(filterContext.Exception, "Unhandled Web Exception");
        }
    }
}