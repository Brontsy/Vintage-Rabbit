﻿using Autofac;
using Autofac.Features.Variance;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Web.Providers;

namespace Vintage.Rabbit.Web.App_Start
{
    public class AutofacConfig
    {

        public static void RegisterBindings()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterModule(new Vintage.Rabbit.CQRS.Ioc.Bindings());
            builder.RegisterModule(new Vintage.Rabbit.Carts.Ioc.Bindings());
            builder.RegisterModule(new Vintage.Rabbit.Common.Ioc.Bindings());
            builder.RegisterModule(new Vintage.Rabbit.Products.Ioc.Bindings());
            builder.RegisterModule(new Vintage.Rabbit.Caching.Ioc.Bindings());
            builder.RegisterModule(new Vintage.Rabbit.Messaging.Ioc.Bindings());
            builder.RegisterModule(new Vintage.Rabbit.Orders.Ioc.Bindings());
            builder.RegisterModule(new Vintage.Rabbit.Membership.Ioc.Bindings());


            builder.RegisterType<LoginProvider>().As<ILoginProvider>();

            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}