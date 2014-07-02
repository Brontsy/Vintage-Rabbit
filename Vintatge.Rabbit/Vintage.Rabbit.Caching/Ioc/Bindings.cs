using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Cache;

namespace Vintage.Rabbit.Caching.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<RequestCache>().As<ICacheService>().InstancePerRequest();
            //builder.RegisterType<MemoryCache>().As<ICacheService>().SingleInstance();

            string cache = ConfigurationManager.AppSettings["Cache"];
            if (!string.IsNullOrEmpty(cache) && cache.ToLower() == "redis")
            {

                builder.RegisterType<RedisCache>().As<ICacheService>().InstancePerRequest();
            }
            else
            {
                builder.RegisterType<MemoryCache>().As<ICacheService>().SingleInstance();
            }
        }
    }
}
