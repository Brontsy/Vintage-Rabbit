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
    public class DeleteLoyaltyCardCommand
    {
        public Guid Guid { get; private set; }

        public DeleteLoyaltyCardCommand(Guid guid)
        {
            this.Guid = guid;
        }
    }

    internal class DeleteLoyaltyCardCommandHandler : ICommandHandler<DeleteLoyaltyCardCommand>
    {
        private ILoyaltyCardRepository _loyaltyCardRepository;

        public DeleteLoyaltyCardCommandHandler(ILoyaltyCardRepository loyaltyCardRepository)
        {
            this._loyaltyCardRepository = loyaltyCardRepository;
        }


        public void Handle(DeleteLoyaltyCardCommand command)
        {
            this._loyaltyCardRepository.DeleteLoyaltyCard(command.Guid);
        }
    }
}
