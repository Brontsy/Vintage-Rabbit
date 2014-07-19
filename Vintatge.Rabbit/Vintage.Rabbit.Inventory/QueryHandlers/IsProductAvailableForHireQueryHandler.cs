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
    public class IsProductAvailableForHireQuery
    {
        public Guid ProductGuid { get; private set; }

        public int Quantity { get; private set; }

        public DateTime PartyDate { get; private set; }

        public IsProductAvailableForHireQuery(Guid productGuid, int quantity, DateTime partyDate)
        {
            this.ProductGuid = productGuid;
            this.Quantity = quantity;
            this.PartyDate = partyDate;
        }
    }

    internal class IsProductAvailableForHireQueryHandler : IQueryHandler<bool, IsProductAvailableForHireQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public IsProductAvailableForHireQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public bool Handle(IsProductAvailableForHireQuery query)
        {
            IList<InventoryItem> inventory = this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(query.ProductGuid));

            return inventory.Count(o => o.IsAvailable(query.PartyDate)) >= query.Quantity;
        }
    }
}
