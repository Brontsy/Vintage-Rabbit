
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Parties.QueryHandlers;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.Messaging.Messages;
using Vintage.Rabbit.Products.QueryHandlers;


namespace Vintage.Rabbit.Products.Messaging.Handlers
{
    internal class ProductHiredMessageHandler : IMessageHandler<IOrderPaidMessage>
    {
        private IMessageService _messageService;
        private IQueryDispatcher _queryDispatcher;

        public ProductHiredMessageHandler(IMessageService messageService, IQueryDispatcher queryDispatcher)
        {
            this._messageService = messageService;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(IOrderPaidMessage message)
        {
            if (message.Order.Items.Any(o => o.Product.Type == Common.Enums.ProductType.Hire))
            {
                Party party = this._queryDispatcher.Dispatch<Party, GetPartyByOrderGuidQuery>(new GetPartyByOrderGuidQuery(message.Order.Guid));

                if (party == null)
                {
                    // TODO: HANDLE BIG PROBLEM BECAUSE WE HAVE HIRED A THEME BUT NO PARTY RECORD WAS CREATED
                }
                else
                {
                    foreach (var orderItem in message.Order.Items.Where(o => o.Product.Type == Common.Enums.ProductType.Hire))
                    {
                        this._messageService.AddMessage<IProductHiredMessage>(new ProductHiredMessage() { ProductGuid = orderItem.Product.Guid, Qty = orderItem.Quantity, PartyDate = party.PartyDate });
                    }
                }
            }
        }
    }
}
