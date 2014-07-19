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
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;

namespace Vintage.Rabbit.Carts.CommandHandlers
{
    public class UpdateQuantityCommand
    {
        public Cart Cart { get; private set; }

        public CartItem CartItem { get; private set; }

        public int Quantity { get; private set; }

        public UpdateQuantityCommand(Cart cart, CartItem cartItem, int quantity)
        {
            this.Cart = cart;
            this.CartItem = cartItem;
            this.Quantity = quantity;
        }
    }

    internal class UpdateQuantityCommandHandler : ICommandHandler<UpdateQuantityCommand>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public UpdateQuantityCommandHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher; 
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(UpdateQuantityCommand command)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByGuidQuery>(new GetProductByGuidQuery(command.CartItem.Product.Guid));

            int quantity = command.Quantity;
            int availableInventory = product.Inventory;

            if (product.Type == Common.Enums.ProductType.Hire)
            {
                var partyDate = (DateTime)command.CartItem.Properties["PartyDate"];

                IList<InventoryItem> inventory = this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(product.Guid));

                availableInventory = inventory.Count(o => o.IsAvailable(partyDate));
            }

            if (availableInventory <= command.Quantity)
            {
                quantity = availableInventory;
            }
           

            command.CartItem.ChangeQuantity(quantity);

            this._commandDispatcher.Dispatch(new SaveCartCommand(command.Cart));
        }
    }
}
