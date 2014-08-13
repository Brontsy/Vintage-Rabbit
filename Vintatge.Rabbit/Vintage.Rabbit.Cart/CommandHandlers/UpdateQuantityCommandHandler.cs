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

        public Guid CartItemGuid { get; private set; }

        public int Quantity { get; private set; }

        public UpdateQuantityCommand(Cart cart, Guid cartItemGuid, int quantity)
        {
            this.Cart = cart;
            this.CartItemGuid = cartItemGuid;
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
            CartItem cartItem = command.Cart.Items.FirstOrDefault(o => o.Id == command.CartItemGuid);

            if (cartItem != null)
            {
                Product product = this._queryDispatcher.Dispatch<Product, GetProductByGuidQuery>(new GetProductByGuidQuery(cartItem.Product.Guid));
                bool available = false;

                if (product.Type == Common.Enums.ProductType.Hire)
                {
                    var partyDate = (DateTime)cartItem.Properties["PartyDate"];
                    available = this._queryDispatcher.Dispatch<bool, IsProductAvailableForHireQuery>(new IsProductAvailableForHireQuery(cartItem.Product.Guid, command.Quantity, partyDate));
                }
                else if (product.Type == Common.Enums.ProductType.Buy)
                {
                    available = this._queryDispatcher.Dispatch<bool, IsProductAvailableQuery>(new IsProductAvailableQuery(cartItem.Product.Guid, command.Quantity));
                }

                if (available)
                {
                    cartItem.ChangeQuantity(command.Quantity);
                }

                this._commandDispatcher.Dispatch(new SaveCartCommand(command.Cart));
            }
        }
    }
}
