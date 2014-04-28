using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;

namespace Vintage.Rabbit.CQRS.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<QueryDispatcher>().As<IQueryDispatcher>();
            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();
        }
    }
}
