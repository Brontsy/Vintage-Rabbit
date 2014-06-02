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
    public class AddShippingAddressCommand
    {
        public Order Order { get; private set; }

        public Address Address { get; private set; }


        public AddShippingAddressCommand(Order order, Address address)
        {
            this.Order = order;
            this.Address = address;
        }
    }

    internal class AddShippingAddressCommandHandler : ICommandHandler<AddShippingAddressCommand>
    {
        private ICommandDispatcher _commandDispatcher;

        public AddShippingAddressCommandHandler(ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(AddShippingAddressCommand command)
        {
            Order order = command.Order;
            order.AddShippingAddress(command.Address);

            this._commandDispatcher.Dispatch<SaveOrderCommand>(new SaveOrderCommand(order));
        }
    }
}
