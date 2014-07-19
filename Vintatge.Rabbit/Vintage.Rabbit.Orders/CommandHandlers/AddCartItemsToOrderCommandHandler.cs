﻿using System;
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
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Common.Enums;

namespace Vintage.Rabbit.Orders.CommandHandlers
{
    public class AddCartItemsToOrderCommand
    {
        public Order Order { get; private set; }

        public Cart Cart { get; private set; }

        public AddCartItemsToOrderCommand(Order order, Cart cart)
        {
            this.Order = order;
            this.Cart = cart;
        }
    }

    internal class AddCartItemsToOrderCommandHandler : ICommandHandler<AddCartItemsToOrderCommand>
    {
        private ICommandDispatcher _commandDispatcher;

        public AddCartItemsToOrderCommandHandler(ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(AddCartItemsToOrderCommand command)
        {
            Order order = command.Order;

            order.Clear();
            foreach(var cartItem in command.Cart.Items)
            {
                order.AddProduct(cartItem);
            }

            if (command.Cart.Items.Any(o => o.Product.Type == ProductType.Buy))
            {
                order.AddDelivery(new Delivery("Delivery", 9.95M));
            }

            if (command.Cart.Items.Any(o => o.Product.Type == Common.Enums.ProductType.Hire))
            {
                order.PartyDate = (DateTime)command.Cart.Items.First(o => o.Product.Type == Common.Enums.ProductType.Hire).Properties["PartyDate"];
            }


            this._commandDispatcher.Dispatch<SaveOrderCommand>(new SaveOrderCommand(order));
        }
    }
}
