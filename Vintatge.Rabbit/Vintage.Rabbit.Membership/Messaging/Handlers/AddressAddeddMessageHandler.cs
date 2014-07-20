
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Membership.CommandHandlers;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Messaging.Messages;
using Vintage.Rabbit.Membership.QueryHandlers;

namespace Vintage.Rabbit.Membership.Messaging.Handlers
{
    internal class AddressAddeddMessageHandler : IMessageHandler<AddressAddedMessage>
    {
        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;

        public AddressAddeddMessageHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(AddressAddedMessage message)
        {
            Member member = this._queryDispatcher.Dispatch<Member, GetMemberByIdQuery>(new GetMemberByIdQuery(message.Address.MemberGuid));

            this._commandDispatcher.Dispatch(new SaveMemberCommand(member));
        }
    }
}
