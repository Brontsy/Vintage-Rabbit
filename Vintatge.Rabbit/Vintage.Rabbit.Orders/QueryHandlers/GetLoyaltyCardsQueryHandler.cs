using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.Repository;

namespace Vintage.Rabbit.Orders.QueryHandlers
{
    public class GetLoyaltyCardsQuery
    {
        public GetLoyaltyCardsQuery()
        {
        }
    }

    internal class GetLoyaltyCardsQueryHandler : IQueryHandler<IList<LoyaltyCard>, GetLoyaltyCardsQuery>
    {
        private ILoyaltyCardRepository _loyaltyCardRepository;

        public GetLoyaltyCardsQueryHandler(ILoyaltyCardRepository loyaltyCardRepository)
        {
            this._loyaltyCardRepository = loyaltyCardRepository;
        }

        public IList<LoyaltyCard> Handle(GetLoyaltyCardsQuery query)
        {
            return this._loyaltyCardRepository.GetLoyaltyCards();
        }
    }
}
