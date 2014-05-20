using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Membership.CommandHandlers;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Messaging.Messages;
using Vintage.Rabbit.Membership.QueryHandlers;
using Vintage.Rabbit.Membership.Repository;

namespace Vintage.Rabbit.Membership.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ValidateLoginQueryHandler>().As<IQueryHandler<bool, ValidateLoginQuery>>();
            builder.RegisterType<GetMemberByEmailQueryHandler>().As<IQueryHandler<Member, GetMemberByEmailQuery>>();

            builder.RegisterType<RegisterCommandHandler>().As<ICommandHandler<RegisterCommand>>();
            builder.RegisterType<SaveMemberCommandHandler>().As<ICommandHandler<SaveMemberCommand>>();

            builder.RegisterType<MembershipRepository>().As<IMessageHandler<SaveMemberMessage>>();
            builder.RegisterType<MembershipRepository>().As<IMembershipRepository>();
        }
    }
}
