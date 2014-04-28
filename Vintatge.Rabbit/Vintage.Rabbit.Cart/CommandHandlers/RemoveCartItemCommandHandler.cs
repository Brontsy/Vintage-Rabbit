using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.Messaging.Messages;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Carts.CommandHandlers
{
    public class RemoveCartItemCommand
    {
        public Guid OwnerId { get; private set; }

        public Guid CartItemId { get; private set; }

        public RemoveCartItemCommand(Guid ownerId, Guid cartItemId)
        {
            this.OwnerId = ownerId;
            this.CartItemId = cartItemId;
        }
    }

    internal class RemoveCartItemCommandHandler : ICommandHandler<RemoveCartItemCommand>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public RemoveCartItemCommandHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(RemoveCartItemCommand command)
        {
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(command.OwnerId));

            cart.RemoveProduct(command.CartItemId);

            this._commandDispatcher.Dispatch(new SaveCartCommand(cart));
        }
    }
}
