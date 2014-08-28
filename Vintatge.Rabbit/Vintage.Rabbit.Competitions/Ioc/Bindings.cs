using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Competitions.CommandHandlers;
using Vintage.Rabbit.Competitions.Entities;
using Vintage.Rabbit.Competitions.Repositories;

namespace Vintage.Rabbit.Competitions.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SaveCompetitionEntryCommandHandler>().As<ICommandHandler<SaveCompetitionEntryCommand>>();

            builder.RegisterType<CompeitionRepository>().As<ICompeitionRepository>();
        }
    }
}
