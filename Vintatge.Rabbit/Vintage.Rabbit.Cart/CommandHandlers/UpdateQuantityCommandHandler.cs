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
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;

namespace Vintage.Rabbit.Carts.CommandHandlers
{
    public class UpdateQuantityCommand
    {
        public Cart Cart { get; private set; }

        public Guid CartItemGuid { get; private set; }

        public int Quantity { get; private set; }

        public DateTime? PartyDate { get; private set; }

        public UpdateQuantityCommand(Cart cart, Guid cartItemGuid, int quantity, DateTime? partyDate = null)
        {
            this.Cart = cart;
            this.CartItemGuid = cartItemGuid;
            this.Quantity = quantity;
            this.PartyDate = partyDate;
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
            CartItem cartItem = command.Cart.Items.FirstOrDefault(o => o.Id == command.CartItemGuid);

            if (cartItem != null)
            {
                Product product = this._queryDispatcher.Dispatch<Product, GetProductByGuidQuery>(new GetProductByGuidQuery(cartItem.Product.Guid));

                // get the maximum number of inventory we can have
                int inventoryCount = this._queryDispatcher.Dispatch<int, GetInventoryCountCanAddToCartQuery>(new GetInventoryCountCanAddToCartQuery(command.Cart.MemberId, product.Guid, command.PartyDate));

                if (inventoryCount > 0)
                {
                    int quantity = command.Quantity;
                    if (command.Quantity > inventoryCount)
                    {
                        quantity = inventoryCount;
                    }

                    cartItem.ChangeQuantity(quantity);
                }

                this._commandDispatcher.Dispatch(new SaveCartCommand(command.Cart));
            }
        }
    }
}
