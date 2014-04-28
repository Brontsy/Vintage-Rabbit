//using Autofac;
//using Autofac.Integration.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace Vintage.Rabbit.Web.Old.App_Start
//{
//    public class AutofacConfig
//    {

//        public static void RegisterBindings()
//        {
//            ContainerBuilder builder = new ContainerBuilder();

//            builder.RegisterControllers(typeof(MvcApplication).Assembly);

//            builder.RegisterModule(new Vintage.Rabbit.CQRS.Ioc.Bindings());
//            builder.RegisterModule(new Vintage.Rabbit.Carts.Ioc.Bindings());

//            var container = builder.Build();
//            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
//        }
//    }
//}