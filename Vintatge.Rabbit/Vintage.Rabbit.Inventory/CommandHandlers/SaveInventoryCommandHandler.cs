using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Inventory.Repository;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Caching;

namespace Vintage.Rabbit.Inventory.CommandHandlers
{
    public class SaveInventoryCommand
    {
        public InventoryItem InventoryItem { get; private set; }

        public SaveInventoryCommand(InventoryItem inventoryItem)
        {
            this.InventoryItem = inventoryItem;
        }
    }

    internal class SaveInventoryCommandHandler : ICommandHandler<SaveInventoryCommand>
    {
        private ICacheService _cacheService;
        private IInventoryRepository _inventoryRepository;

        public SaveInventoryCommandHandler(IInventoryRepository inventoryRepository, ICacheService cacheService)
        {
            this._inventoryRepository = inventoryRepository;
        }

        public void Handle(SaveInventoryCommand command)
        {
            this._cacheService.Remove(CacheKeyHelper.Inventory.ByGuid(command.InventoryItem.Guid));
            this._cacheService.Remove(CacheKeyHelper.Inventory.ByProductGuid(command.InventoryItem.ProductGuid));

            this._inventoryRepository.SaveInventory(command.InventoryItem);
        }

    }
}
