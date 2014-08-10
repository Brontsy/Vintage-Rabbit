using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Inventory.Repository;

namespace Vintage.Rabbit.Inventory.QueryHandlers
{
    public class GetInventoryForProductsQuery
    {
        public IList<Guid> ProductGuids { get; private set; }

        public GetInventoryForProductsQuery(IList<Guid> productGuids)
        {
            this.ProductGuids = productGuids;
        }
    }

    internal class GetInventoryForProductsQueryHandler : IQueryHandler<IList<InventoryItem>, GetInventoryForProductsQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public GetInventoryForProductsQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public IList<InventoryItem> Handle(GetInventoryForProductsQuery query)
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();

            foreach(var productGuid in query.ProductGuids)
            {
                inventoryItems.AddRange(this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(productGuid)));
            }

            return inventoryItems;
        }
    }
}
