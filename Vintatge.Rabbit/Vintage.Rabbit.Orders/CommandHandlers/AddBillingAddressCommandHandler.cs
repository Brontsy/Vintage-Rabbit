using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Orders.Messaging.Messages;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.QueryHandlers;

namespace Vintage.Rabbit.Orders.CommandHandlers
{
    public class AddBillingAddressCommand
    {
        public Order Order { get; private set; }

        public Address Address { get; private set; }

        public AddBillingAddressCommand(Order order, Address address)
        {
            this.Order = order;
            this.Address = address;
        }
    }

    internal class AddBillingAddressCommandHandler : ICommandHandler<AddBillingAddressCommand>
    {
        private ICommandDispatcher _commandDispatcher;

        public AddBillingAddressCommandHandler(ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(AddBillingAddressCommand command)
        {
            Order order = command.Order;
            order.AddBillingAddress(command.Address);

            this._commandDispatcher.Dispatch<SaveOrderCommand>(new SaveOrderCommand(order));
        }
    }
}
