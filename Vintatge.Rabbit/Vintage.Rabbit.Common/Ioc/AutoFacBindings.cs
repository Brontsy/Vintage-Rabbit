using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Http;
using Vintage.Rabbit.Common.Serialization;

namespace Vintage.Rabbit.Common.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonSerializer>().As<ISerializer>();
            builder.RegisterType<HttpWebUtility>().As<IHttpWebUtility>();
        }
    }
}
