
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
    internal class ProductHiredMessageHandler : IMessageHandler<IProductHiredMessage>
    {
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;
        private IQueryDispatcher _queryDispatcher;

        public ProductHiredMessageHandler(IMessageService messageService, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._messageService = messageService;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(IProductHiredMessage message)
        {
            IList<InventoryItem> inventory = this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(message.ProductGuid));

            for (int i = 0; i < message.Qty; i++)
            {
                var inventoryItem = inventory.FirstOrDefault(o => o.IsAvailable(message.PartyDate));
                if (inventoryItem == null)
                {
                    // TODO: HANDLE UNAVAILABLE HIRE ITEM
                }
                else
                {
                    inventoryItem.Hired(message.PartyDate);

                    this._commandDispatcher.Dispatch(new SaveInventoryCommand(inventoryItem));
                }
            }
        }
    }
}
