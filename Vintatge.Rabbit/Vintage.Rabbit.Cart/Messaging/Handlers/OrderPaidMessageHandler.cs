
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Carts.CommandHandlers;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Inventory.CommandHandlers;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Inventory.Messaging.Messages;
using Vintage.Rabbit.Inventory.QueryHandlers;

namespace Vintage.Rabbit.Carts.Messaging.Handlers
{
    internal class OrderPaidMessageHandler : IMessageHandler<IOrderPaidMessage>
    {
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;
        private IQueryDispatcher _queryDispatcher;

        public OrderPaidMessageHandler(IMessageService messageService, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._messageService = messageService;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(IOrderPaidMessage message)
        {
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(message.Order.MemberGuid));

            foreach(var orderItem in message.Order.Items)
            {
                var cartItem = cart.Items.FirstOrDefault(o => o.Product.Guid == orderItem.Product.Guid);
                if(cartItem != null)
                {
                    cart.RemoveProduct(cartItem.Id);
                }
            }

            this._commandDispatcher.Dispatch(new SaveCartCommand(cart));
        }
    }
}
