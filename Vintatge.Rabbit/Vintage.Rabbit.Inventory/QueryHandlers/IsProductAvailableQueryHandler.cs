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
    public class IsProductAvailableQuery
    {
        public Guid ProductGuid { get; private set; }

        public int Quantity { get; private set; }

        public IsProductAvailableQuery(Guid productGuid, int quantity)
        {
            this.ProductGuid = productGuid;
            this.Quantity = quantity;
        }
    }

    internal class IsProductAvailableQueryHandler : IQueryHandler<bool, IsProductAvailableQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public IsProductAvailableQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public bool Handle(IsProductAvailableQuery query)
        {
            IList<InventoryItem> inventory = this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(query.ProductGuid));

            return inventory.Count(o => o.IsAvailable()) >= query.Quantity;
        }
    }
}
