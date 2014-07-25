
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Parties.Repositories;

namespace Vintage.Rabbit.Parties.QueryHandlers
{
    public class GetPartyByOrderGuidQuery
    {
        public Guid OrderGuid { get; private set; }

        public GetPartyByOrderGuidQuery(Guid orderGuid)
        {
            this.OrderGuid = orderGuid;
        }
    }

    internal class GetPartyByOrderGuidQueryHandler : IQueryHandler<Party, GetPartyByOrderGuidQuery>
    {
        private IPartyRepository _partyRepository;

        public GetPartyByOrderGuidQueryHandler(IPartyRepository partyRepository)
        {
            this._partyRepository = partyRepository;
        }

        public Party Handle(GetPartyByOrderGuidQuery query)
        {
            return this._partyRepository.GetPartyByOrderGuid(query.OrderGuid);
        }
    }
}
