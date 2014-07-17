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
    public class GetLoyaltyCardQuery
    {
        public string Number { get; private set; }

        public GetLoyaltyCardQuery(string number)
        {
            this.Number = number;
        }
    }

    internal class GetLoyaltyCardQueryHandler : IQueryHandler<LoyaltyCard, GetLoyaltyCardQuery>
    {
        private ILoyaltyCardRepository _loyaltyCardRepository;

        public GetLoyaltyCardQueryHandler(ILoyaltyCardRepository loyaltyCardRepository)
        {
            this._loyaltyCardRepository = loyaltyCardRepository;
        }

        public LoyaltyCard Handle(GetLoyaltyCardQuery query)
        {
            return this._loyaltyCardRepository.GetLoyaltyCard(query.Number);
        }
    }
}
