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
using Vintage.Rabbit.Common.Enums;

namespace Vintage.Rabbit.Orders.CommandHandlers
{
    public class AddDeliveryAddressCommand
    {
        public Order Order { get; private set; }

        public Address Address { get; private set; }

        public AddDeliveryAddressCommand(Order order, Address address)
        {
            this.Order = order;
            this.Address = address;
        }
    }

    internal class AddDeliveryAddressCommandHandler : ICommandHandler<AddDeliveryAddressCommand>
    {
        private ICommandDispatcher _commandDispatcher;

        public AddDeliveryAddressCommandHandler(ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(AddDeliveryAddressCommand command)
        {
            Order order = command.Order;
            order.AddDeliveryAddress(command.Address);

            if(order.Items.Any(o => o.Product.Type == ProductType.Delivery))
            {
                var guids = order.Items.Where(o => o.Product.Type == ProductType.Delivery).Select(o => o.Guid).ToList();
                foreach (var guid in guids)
                {
                    order.RemoveProduct(guid);
                }
            }

            order.AddDelivery(new Delivery("Hire Delivery", 25M));

            this._commandDispatcher.Dispatch<SaveOrderCommand>(new SaveOrderCommand(order));
        }
    }
}
