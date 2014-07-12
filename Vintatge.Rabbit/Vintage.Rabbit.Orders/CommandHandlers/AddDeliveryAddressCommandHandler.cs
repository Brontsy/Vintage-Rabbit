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
using Vintage.Rabbit.Common.Constants;

namespace Vintage.Rabbit.Orders.CommandHandlers
{
    public class AddDeliveryAddressCommand
    {
        public Order Order { get; private set; }

        public Address Address { get; private set; }

        public bool IsPickup { get; private set; }

        public bool IsDropoff { get; private set; }

        public AddDeliveryAddressCommand(Order order, Address address, bool isPickup, bool isDropoff)
        {
            this.Order = order;
            this.Address = address;
            this.IsPickup = isPickup;
            this.IsDropoff = isDropoff;
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

            if (command.IsPickup)
            {
                order.AddDelivery(new Delivery("Pickup Hire Delivery", Constants.HireDeliveryCost));
            }
            else
            {
                IEnumerable<Guid> orderItems = order.Items.Where(o => o.Product.Type == ProductType.Delivery && o.Product.Title == "Pickup Hire Delivery").Select(o => o.Guid);
                foreach(var guid in orderItems)
                {
                    order.RemoveProduct(guid);
                }
            }

            if (command.IsDropoff)
            {
                order.AddDelivery(new Delivery("Dropoff Hire Delivery", Constants.HireDeliveryCost));
            }
            else
            {
                IEnumerable<Guid> orderItems = order.Items.Where(o => o.Product.Type == ProductType.Delivery && o.Product.Title == "Dropoff Hire Delivery").Select(o => o.Guid);

                foreach (var guid in orderItems)
                {
                    order.RemoveProduct(guid);
                }
            }

            this._commandDispatcher.Dispatch<SaveOrderCommand>(new SaveOrderCommand(order));
        }
    }
}
