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
        private IInventoryRepository _inventoryRepository;

        public GetInventoryForProductsQueryHandler(IInventoryRepository inventoryRepository)
        {
            this._inventoryRepository = inventoryRepository;
        }

        public IList<InventoryItem> Handle(GetInventoryForProductsQuery query)
        {
            return this._inventoryRepository.GetInventoryByProductGuid(query.ProductGuids);
        }
    }
}
