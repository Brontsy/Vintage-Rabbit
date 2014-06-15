using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Orders.Messaging.Messages;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Orders.CommandHandlers
{
    public class SaveOrderCommand
    {
        public Order Order { get; private set; }

        public SaveOrderCommand(Order order)
        {
            this.Order = order;
        }
    }

    internal class SaveOrderCommandHandler : ICommandHandler<SaveOrderCommand>
    {
        private ICacheService _cacheService;
        private IMessageService _messageService;

        public SaveOrderCommandHandler(ICacheService cacheService, IMessageService messageService)
        {
            this._cacheService = cacheService;
            this._messageService = messageService;
        }

        public void Handle(SaveOrderCommand command)
        {
            string cacheKey = CacheKeyHelper.Order.ById(command.Order.Guid);

            this._cacheService.Add(cacheKey, command.Order);

            SaveOrderMessage message = new SaveOrderMessage(command.Order);

            this._messageService.AddMessage(message);
        }
    }
}
