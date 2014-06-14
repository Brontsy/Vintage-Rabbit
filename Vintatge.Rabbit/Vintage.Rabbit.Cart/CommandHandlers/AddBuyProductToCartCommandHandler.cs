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
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Carts.CommandHandlers
{
    public class AddBuyProductToCartCommand
    {
        public Guid OwnerId { get; private set; }

        public int Quantity { get; private set; }

        public Product Product { get; private set; }

        public AddBuyProductToCartCommand(Guid ownerId, int quantity, Product product)
        {
            this.OwnerId = ownerId;
            this.Quantity = quantity;
            this.Product = product;
        }
    }

    internal class AddBuyProductToCartCommandHandler : ICommandHandler<AddBuyProductToCartCommand>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public AddBuyProductToCartCommandHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(AddBuyProductToCartCommand command)
        {
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(command.OwnerId));
            IList<InventoryItem> inventory = this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(command.Product.Guid));

            int availableInventory = inventory.Count(o => o.IsAvailable());
            int quantity = command.Quantity;
            if (availableInventory < command.Quantity)
            {
                quantity = availableInventory;
            }

            cart.AddProduct(quantity, command.Product);

            this._commandDispatcher.Dispatch(new SaveCartCommand(cart));
        }
    }
}
