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
        private IInventoryRepository _inventoryRepository;

        public GetInventoryForProductQueryHandler(IInventoryRepository inventoryRepository)
        {
            this._inventoryRepository = inventoryRepository;
        }

        public IList<InventoryItem> Handle(GetInventoryForProductQuery query)
        {
            return this._inventoryRepository.GetInventoryByProductGuid(query.ProductGuid);
        }
    }
}
