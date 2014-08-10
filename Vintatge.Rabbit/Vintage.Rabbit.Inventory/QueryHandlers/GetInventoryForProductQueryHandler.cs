using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Inventory.Repository;
using Vintage.Rabbit.Caching;

namespace Vintage.Rabbit.Inventory.QueryHandlers
{
    public class GetInventoryForProductQuery
    {
        public Guid ProductGuid { get; private set; }

        public GetInventoryForProductQuery(Guid productGuid)
        {
            this.ProductGuid = productGuid;
        }
    }

    internal class GetInventoryForProductQueryHandler : IQueryHandler<IList<InventoryItem>, GetInventoryForProductQuery>
    {
        private ICacheService _cacheService;
        private IInventoryRepository _inventoryRepository;

        public GetInventoryForProductQueryHandler(IInventoryRepository inventoryRepository, ICacheService cacheService)
        {
            this._inventoryRepository = inventoryRepository;
            this._cacheService = cacheService;
        }

        public IList<InventoryItem> Handle(GetInventoryForProductQuery query)
        {
            string cacheKey = CacheKeyHelper.Inventory.ByProductGuid(query.ProductGuid);

            if (this._cacheService.Exists(cacheKey))
            {
                return this._cacheService.Get<IList<InventoryItem>>(cacheKey);
            }

            IList<InventoryItem> items = this._inventoryRepository.GetInventoryByProductGuid(query.ProductGuid);

            this._cacheService.Add(cacheKey, items);

            return items;
        }
    }
}
