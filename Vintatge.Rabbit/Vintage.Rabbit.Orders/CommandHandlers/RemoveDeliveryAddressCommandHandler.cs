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
    public class RemoveDeliveryAddressCommand
    {
        public Order Order { get; private set; }

        public RemoveDeliveryAddressCommand(Order order)
        {
            this.Order = order;
        }
    }

    internal class RemoveDeliveryAddressCommandHandler : ICommandHandler<RemoveDeliveryAddressCommand>
    {
        private ICommandDispatcher _commandDispatcher;

        public RemoveDeliveryAddressCommandHandler(ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(RemoveDeliveryAddressCommand command)
        {
            Order order = command.Order;
            order.RemoveDeliveryAddress();

            if(order.Items.Any(o => o.Product.Type == ProductType.Delivery))
            {
                var guids = order.Items.Where(o => o.Product.Type == ProductType.Delivery).Select(o => o.Guid).ToList();
                foreach (var guid in guids)
                {
                    order.RemoveProduct(guid);
                }
            }

            this._commandDispatcher.Dispatch<SaveOrderCommand>(new SaveOrderCommand(order));
        }
    }
}
