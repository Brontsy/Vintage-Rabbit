using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Parties.Repositories;
using Vintage.Rabbit.Common.Entities;

namespace Vintage.Rabbit.Parties.QueryHandlers
{
    public class GetPartiesQuery
    {
        public int Page { get; private set; }

        public int ResultsPerPage { get; private set; }

        public GetPartiesQuery(int page, int resultsPerPage)
        {
            this.Page = page;
            this.ResultsPerPage = resultsPerPage;
        }
    }

    internal class GetPartiesQueryHandler : IQueryHandler<PagedResult<Party>, GetPartiesQuery>
    {
        private IPartyRepository _partyRepository;

        public GetPartiesQueryHandler(IPartyRepository partyRepository)
        {
            this._partyRepository = partyRepository;
        }

        public PagedResult<Party> Handle(GetPartiesQuery query)
        {
            return this._partyRepository.GetParties(query.Page, query.ResultsPerPage);
        }
    }
}
