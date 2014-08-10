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
    public class GetInventoryByGuidQuery
    {
        public Guid Guid { get; private set; }

        public GetInventoryByGuidQuery(Guid guid)
        {
            this.Guid = guid;
        }
    }

    internal class GetInventoryByGuidQueryHandler : IQueryHandler<InventoryItem, GetInventoryByGuidQuery>
    {
        private ICacheService _cacheService;
        private IInventoryRepository _inventoryRepository;

        public GetInventoryByGuidQueryHandler(IInventoryRepository inventoryRepository, ICacheService cacheService)
        {
            this._inventoryRepository = inventoryRepository;
            this._cacheService = cacheService;
        }

        public InventoryItem Handle(GetInventoryByGuidQuery query)
        {
            string cacheKey = CacheKeyHelper.Inventory.ByGuid(query.Guid);

            if (this._cacheService.Exists(cacheKey))
            {
                return this._cacheService.Get<InventoryItem>(cacheKey);
            }

            InventoryItem item = this._inventoryRepository.GetInventoryByGuid(query.Guid);

            this._cacheService.Add(cacheKey, item);

            return item;
        }
    }
}
