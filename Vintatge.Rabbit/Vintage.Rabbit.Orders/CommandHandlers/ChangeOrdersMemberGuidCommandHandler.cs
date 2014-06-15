using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.Messaging.Messages;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Orders.CommandHandlers
{
    public class ChangeOrdersMemberGuidCommand
    {
        public Order Order { get; private set; }

        public Guid MemberGuid { get; private set; }

        public ChangeOrdersMemberGuidCommand(Order order, Guid memberGuid)
        {
            this.Order = order;
            this.MemberGuid = memberGuid;
        }
    }

    internal class ChangeOrdersMemberGuidCommandHandler : ICommandHandler<ChangeOrdersMemberGuidCommand>
    {
        private ICommandDispatcher _commandDispatcher;

        public ChangeOrdersMemberGuidCommandHandler(ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(ChangeOrdersMemberGuidCommand command)
        {
            Order order = command.Order;
            order.MemberGuid = command.MemberGuid;

            this._commandDispatcher.Dispatch(new SaveOrderCommand(order));
        }
    }
}
