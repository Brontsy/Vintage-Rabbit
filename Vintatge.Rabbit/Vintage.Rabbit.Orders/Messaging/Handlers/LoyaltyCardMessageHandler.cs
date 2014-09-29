
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Orders.CommandHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.Messaging.Messages;
using Vintage.Rabbit.Orders.QueryHandlers;
using Vintage.Rabbit.Orders.Repository;
using Vintage.Rabbit.Payment.Messaging.Messages;

namespace Vintage.Rabbit.Orders.Messaging.Handlers
{
    internal class LoyaltyCardMessageHandler : IMessageHandler<IOrderPaidMessage>
    {
        private ILoyaltyCardRepository _loyaltyCardRepository;

        public LoyaltyCardMessageHandler(ILoyaltyCardRepository loyaltyCardRepository)
        {
            this._loyaltyCardRepository = loyaltyCardRepository;
        }

        public void Handle(IOrderPaidMessage message)
        {
            if(message.Order.Items.Any(o => o.Product.Type == Common.Enums.ProductType.Discount))
            {
                IOrderItem discountItem = message.Order.Items.First(o => o.Product.Type == Common.Enums.ProductType.Discount);
                LoyaltyCard loyaltyCard = discountItem.Product as LoyaltyCard;

                if(loyaltyCard != null)
                {
                    loyaltyCard.Consumed(message.Order.Guid);
                    this._loyaltyCardRepository.SaveLoyaltyCard(loyaltyCard);
                }
            }
        }
    }
}
