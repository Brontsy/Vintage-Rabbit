using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.Messaging.Messages;

namespace Vintage.Rabbit.Products.CommandHandlers
{
    public class SaveProductCommand
    {
        public Product Product { get; private set; }
    }

    internal class SaveProductCommandHandler : ICommandHandler<SaveProductCommand>
    {
        private ICacheService _cacheService;
        private IMessageService _messageService;

        public SaveProductCommandHandler(ICacheService cacheService, IMessageService messageService)
        {
            this._cacheService = cacheService;
            this._messageService = messageService;
        }

        public void Handle(SaveProductCommand command)
        {
            string cacheKey = CacheKeyHelper.Product.ById(command.Product.Id);

            this._cacheService.Add(cacheKey, command.Product);

            IMessage message = new SaveProductMessage(command.Product);

            this._messageService.AddMessage(message);
        }
    }
}
