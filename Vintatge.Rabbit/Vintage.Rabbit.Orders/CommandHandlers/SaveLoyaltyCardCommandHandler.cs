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
using Vintage.Rabbit.Orders.Repository;

namespace Vintage.Rabbit.Orders.CommandHandlers
{
    public class SaveLoyaltyCardCommand
    {
        public LoyaltyCard LoyaltyCard { get; private set; }

        public SaveLoyaltyCardCommand(LoyaltyCard loyaltyCard)
        {
            this.LoyaltyCard = loyaltyCard;
        }
    }

    internal class SaveLoyaltyCardCommandHandler : ICommandHandler<SaveLoyaltyCardCommand>
    {
        private ILoyaltyCardRepository _loyaltyCardRepository;

        public SaveLoyaltyCardCommandHandler(ILoyaltyCardRepository loyaltyCardRepository)
        {
            this._loyaltyCardRepository = loyaltyCardRepository;
        }


        public void Handle(SaveLoyaltyCardCommand command)
        {
            this._loyaltyCardRepository.SaveLoyaltyCard(command.LoyaltyCard);
        }
    }
}
