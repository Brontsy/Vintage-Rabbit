
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Inventory.CommandHandlers;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Inventory.Messaging.Messages;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Payment.Messaging.Messages;

namespace Vintage.Rabbit.Inventory.Messaging.Handlers
{
    internal class ProductPurchasedMessageHandler : IMessageHandler<IProductPurchasedMessage>
    {
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;
        private IQueryDispatcher _queryDispatcher;

        public ProductPurchasedMessageHandler(IMessageService messageService, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._messageService = messageService;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(IProductPurchasedMessage message)
        {
            IList<InventoryItem> inventory = this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(message.OrderItem.Product.Guid));

            for (int i = 0; i < message.OrderItem.Quantity; i++)
            {
                var inventoryItem = inventory.FirstOrDefault(o => o.IsAvailable());
                if (inventoryItem == null)
                {
                    // TODO: HANDLE UNAVAILABLE PURCHASE ITEM
                }
                else
                {
                    inventoryItem.Sold(message.OrderItem);

                    this._commandDispatcher.Dispatch(new SaveInventoryCommand(inventoryItem));
                    this._messageService.AddMessage<IInventorySoldMessage>(new InventorySoldMessage(inventoryItem));
                }
            }
        }
    }
}
