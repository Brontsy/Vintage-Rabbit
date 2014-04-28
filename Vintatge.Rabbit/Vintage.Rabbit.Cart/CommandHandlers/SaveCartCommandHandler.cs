using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.Messaging.Messages;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;

namespace Vintage.Rabbit.Carts.CommandHandlers
{
    public class SaveCartCommand
    {
        public Cart Cart { get; private set; }

        public SaveCartCommand(Cart cart)
        {
            this.Cart = cart;
        }
    }

    internal class SaveCartCommandHandler : ICommandHandler<SaveCartCommand>
    {
        private ICacheService _cacheService;
        private IMessageService _messageService;

        public SaveCartCommandHandler(ICacheService cacheService, IMessageService messageService)
        {
            this._cacheService = cacheService;
            this._messageService = messageService;
        }

        public void Handle(SaveCartCommand command)
        {
            this._cacheService.Add(CacheKeyHelper.Cart.ById(command.Cart.Id), command.Cart);
            this._cacheService.Add(CacheKeyHelper.Cart.ByOwnerId(command.Cart.MemberId), command.Cart);

            SaveCartMessage message = new SaveCartMessage(command.Cart);

            this._messageService.AddMessage(message);
        }
    }
}
