using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Inventory.Entities;

namespace Vintage.Rabbit.Inventory.QueryHandlers
{
    public class CountInventoryAvailableForHireQuery
    {
        public Guid ProductGuid { get; private set; }

        public DateTime PartyDate { get; private set; }

        public CountInventoryAvailableForHireQuery(Guid productGuid, DateTime partyDate)
        {
            this.ProductGuid = productGuid;
            this.PartyDate = partyDate;
        }
    }

    internal class CountInventoryAvailableForHireQueryHandler : IQueryHandler<int, CountInventoryAvailableForHireQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public CountInventoryAvailableForHireQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public int Handle(CountInventoryAvailableForHireQuery query)
        {
            IList<InventoryItem> inventory = this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(query.ProductGuid));

            return inventory.Count(o => o.IsAvailable(query.PartyDate));
        }
    }
}
