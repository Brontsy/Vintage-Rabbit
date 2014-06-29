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
        private IInventoryRepository _inventoryRepository;

        public GetInventoryByGuidQueryHandler(IInventoryRepository inventoryRepository)
        {
            this._inventoryRepository = inventoryRepository;
        }

        public InventoryItem Handle(GetInventoryByGuidQuery query)
        {
            return this._inventoryRepository.GetInventoryByGuid(query.Guid);
        }
    }
}
