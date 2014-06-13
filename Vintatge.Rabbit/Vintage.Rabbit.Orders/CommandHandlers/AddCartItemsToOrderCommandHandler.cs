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
using Vintage.Rabbit.Carts.Entities;

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

            this._commandDispatcher.Dispatch<SaveOrderCommand>(new SaveOrderCommand(order));
        }
    }
}
