using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Membership;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.Messaging.Messages;

namespace Vintage.Rabbit.Products.CommandHandlers
{
    public class CreateProductCommand
    {
        public Guid ProductGuid { get; private set; }

        public int InventoryCount { get; private set; }

        public IActionBy ActionBy { get; private set; }

        public CreateProductCommand(Guid productGuid, int inventoryCount, IActionBy actionBy)
        {
            this.ProductGuid = productGuid;
            this.InventoryCount = inventoryCount;
            this.ActionBy = actionBy;
        }
    }

    internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
    {
        private ICacheService _cacheService;
        private IMessageService _messageService;

        public CreateProductCommandHandler(ICacheService cacheService, IMessageService messageService)
        {
            this._cacheService = cacheService;
            this._messageService = messageService;
        }

        public void Handle(CreateProductCommand command)
        {
            Product product = new Product(command.ProductGuid);

            for (int i = 0; i < command.InventoryCount; i++)
            {
                Inventory inventory = new Inventory() { ProductGuid = product.Guid };
                product.Inventory.Add(inventory);
            }

            SaveProductMessage message = new SaveProductMessage(product, command.ActionBy);

            this._messageService.AddMessage(message);
        }
    }
}
