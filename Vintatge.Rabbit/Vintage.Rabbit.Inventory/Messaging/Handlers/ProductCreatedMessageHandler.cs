
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Products;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Inventory.Repository;


namespace Vintage.Rabbit.Inventory.Messaging.Handlers
{
    internal class ProductCreatedMessageHandler : IMessageHandler<IProductCreatedMessage>
    {
        private IInventoryRepository _inventoryRepository;

        public ProductCreatedMessageHandler(IInventoryRepository inventoryRepository)
        {
            this._inventoryRepository = inventoryRepository;
        }

        public void Handle(IProductCreatedMessage message)
        {
            IProduct product = message.Product;

            for (int i = 0; i < product.Inventory; i++)
            {
                InventoryItem inventory = new InventoryItem() { ProductGuid = product.Guid };

                this._inventoryRepository.SaveInventory(inventory);
            }
        }
    }
}
