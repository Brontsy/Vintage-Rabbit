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
    public class GetPartyByGuidQuery
    {
        public Guid Guid { get; private set; }

        public GetPartyByGuidQuery(Guid guid)
        {
            this.Guid = guid;
        }
    }

    internal class GetPartyByGuidQueryHandler : IQueryHandler<Party, GetPartyByGuidQuery>
    {
        private IPartyRepository _partyRepository;

        public GetPartyByGuidQueryHandler(IPartyRepository partyRepository)
        {
            this._partyRepository = partyRepository;
        }

        public Party Handle(GetPartyByGuidQuery query)
        {
            return this._partyRepository.GetPartyByGuid(query.Guid);
        }
    }
}
