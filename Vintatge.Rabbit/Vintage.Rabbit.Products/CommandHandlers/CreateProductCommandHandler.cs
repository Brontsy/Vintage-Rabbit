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
using Vintage.Rabbit.Interfaces.Products;
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
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;

        public CreateProductCommandHandler(ICacheService cacheService, ICommandDispatcher commandDispatcher, IMessageService messageService)
        {
            this._cacheService = cacheService;
            this._commandDispatcher = commandDispatcher;
            this._messageService = messageService;
        }

        public void Handle(CreateProductCommand command)
        {
            Product product = new Product(command.ProductGuid, command.InventoryCount);

            this._commandDispatcher.Dispatch(new SaveProductCommand(product, command.ActionBy));

            IProductCreatedMessage createdMessage = new ProductCreatedMessage(product);

            this._messageService.AddMessage<IProductCreatedMessage>(createdMessage);
        }
    }
}
