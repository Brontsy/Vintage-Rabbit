
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
            foreach(var orderItem in message.Order.Items)
            {
                IList<InventoryItem> inventory = this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(orderItem.Product.Guid));
                IList<IInventoryItem> inventorySold = new List<IInventoryItem>();

                for(int i = 0; i < orderItem.Quantity; i++)
                {
                    if(orderItem.Product.Type == Common.Enums.ProductType.Buy)
                    {
                        var inventoryItem = inventory.First(o => o.IsAvailable());
                        inventoryItem.Sold();

                        this._commandDispatcher.Dispatch(new SaveInventoryCommand(inventoryItem));
                        inventorySold.Add(inventoryItem);
                    }
                    else
                    {
                        DateTime startDate = DateTime.Parse(orderItem.Properties["StartDate"].ToString());
                        DateTime endDate = DateTime.Parse(orderItem.Properties["EndDate"].ToString());

                        var inventoryItem = inventory.First(o => o.IsAvailable(startDate, endDate));
                        inventoryItem.Hired(startDate, endDate);

                        this._commandDispatcher.Dispatch(new SaveInventoryCommand(inventoryItem));
                    }
                }

                if(inventorySold.Any())
                {
                    this._messageService.AddMessage<IInventorySoldMessage>(new InventorySoldMessage(inventorySold));
                }
            }
        }
    }
}
