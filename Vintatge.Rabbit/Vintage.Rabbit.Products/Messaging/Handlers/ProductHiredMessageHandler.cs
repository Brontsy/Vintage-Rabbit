
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.Messaging.Messages;
using Vintage.Rabbit.Products.QueryHandlers;


namespace Vintage.Rabbit.Products.Messaging.Handlers
{
    internal class ProductHiredMessageHandler : IMessageHandler<IOrderPaidMessage>
    {
        private IMessageService _messageService;

        public ProductHiredMessageHandler(IMessageService messageService)
        {
            this._messageService = messageService;
        }

        public void Handle(IOrderPaidMessage message)
        {
            foreach (var orderItem in message.Order.Items.Where(o => o.Product.Type == Common.Enums.ProductType.Hire))
            {
                DateTime partyDate = DateTime.Parse(orderItem.Properties["PartyDate"].ToString());

                this._messageService.AddMessage<IProductHiredMessage>(new ProductHiredMessage() { ProductGuid = orderItem.Product.Guid, Qty = orderItem.Quantity, PartyDate = partyDate });
            }
        }
    }
}
