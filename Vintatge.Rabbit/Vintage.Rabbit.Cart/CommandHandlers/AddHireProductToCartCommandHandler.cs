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

namespace Vintage.Rabbit.Carts.CommandHandlers
{
    public class AddHireProductToCartCommand
    {
        public Guid OwnerId { get; private set; }

        public Product Product { get; private set; }

        public int Quantity { get; private set; }

        public DateTime PartyDate { get; private set; }

        public AddHireProductToCartCommand(Guid ownerId, int quantity, Product product, DateTime partyDate)
        {
            this.OwnerId = ownerId;
            this.Product = product;
            this.Quantity = quantity;
            this.PartyDate = partyDate;
        }
    }

    internal class AddHireProductToCartCommandHandler : ICommandHandler<AddHireProductToCartCommand>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;

        public AddHireProductToCartCommandHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IMessageService messageService)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
            this._messageService = messageService;
        }

        public void Handle(AddHireProductToCartCommand command)
        {
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(command.OwnerId));

            int availableInventory = this._queryDispatcher.Dispatch<int, GetInventoryCountCanAddToCartQuery>(new GetInventoryCountCanAddToCartQuery(command.OwnerId, command.Product.Guid, command.PartyDate));
            int quantity = command.Quantity;

            if(availableInventory < command.Quantity)
            {
                quantity = availableInventory;
            }

            cart.AddProduct(quantity, command.Product);

            this._commandDispatcher.Dispatch(new SaveCartCommand(cart));

            this._messageService.AddMessage(new ProductAddedToCart(cart, command.Product));
        }
    }
}
