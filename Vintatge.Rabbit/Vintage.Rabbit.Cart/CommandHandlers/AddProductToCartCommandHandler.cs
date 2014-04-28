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
    public class AddProductToCartCommand
    {
        public Guid OwnerId { get; private set; }

        public int Quantity { get; private set; }

        public Product Product { get; private set; }

        public AddProductToCartCommand(Guid ownerId, int quantity, Product product)
        {
            this.OwnerId = ownerId;
            this.Quantity = quantity;
            this.Product = product;
        }
    }

    internal class AddProductToCartCommandHandler : ICommandHandler<AddProductToCartCommand>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public AddProductToCartCommandHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(AddProductToCartCommand command)
        {
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(command.OwnerId));

            cart.AddProduct(command.Quantity, command.Product);

            this._commandDispatcher.Dispatch(new SaveCartCommand(cart));
        }
    }
}
