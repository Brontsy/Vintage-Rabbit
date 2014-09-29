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
    public class GetLoyaltyCardByGuidQuery
    {
        public Guid Guid { get; private set; }

        public GetLoyaltyCardByGuidQuery(Guid guid)
        {
            this.Guid = guid;
        }
    }

    internal class GetLoyaltyCardByGuidQueryHandler : IQueryHandler<LoyaltyCard, GetLoyaltyCardByGuidQuery>
    {
        private ILoyaltyCardRepository _loyaltyCardRepository;

        public GetLoyaltyCardByGuidQueryHandler(ILoyaltyCardRepository loyaltyCardRepository)
        {
            this._loyaltyCardRepository = loyaltyCardRepository;
        }

        public LoyaltyCard Handle(GetLoyaltyCardByGuidQuery query)
        {
            return this._loyaltyCardRepository.GetLoyaltyCardByGuid(query.Guid);
        }
    }
}
