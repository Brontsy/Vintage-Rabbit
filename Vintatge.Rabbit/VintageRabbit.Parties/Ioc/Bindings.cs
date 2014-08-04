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
using Vintage.Rabbit.Parties.CommandHandlers;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Parties.Messaging.Handlers;
using Vintage.Rabbit.Parties.QueryHandlers;
using Vintage.Rabbit.Parties.Repositories;

namespace Vintage.Rabbit.Parties.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SavePartyCommandHandler>().As<ICommandHandler<SavePartyCommand>>();
            builder.RegisterType<AddInvitationDetailsCommandHandler>().As<ICommandHandler<AddInvitationDetailsCommand>>();
            builder.RegisterType<AddPartyAddressCommandHandler>().As<ICommandHandler<AddPartyAddressCommand>>();
            builder.RegisterType<CreatePartyCommandHandler>().As<ICommandHandler<CreatePartyCommand>>();

            builder.RegisterType<GetPartiesQueryHandler>().As<IQueryHandler<PagedResult<Party>, GetPartiesQuery>>();
            builder.RegisterType<GetPartyByGuidQueryHandler>().As<IQueryHandler<Party, GetPartyByGuidQuery>>();
            builder.RegisterType<GetPartyByOrderGuidQueryHandler>().As<IQueryHandler<Party, GetPartyByOrderGuidQuery>>();

            builder.RegisterType<PartyRepository>().As<IPartyRepository>();

            builder.RegisterType<OrderPaidMessageHandler>().As<IMessageHandler<IOrderPaidMessage>>();
        }
    }
}
