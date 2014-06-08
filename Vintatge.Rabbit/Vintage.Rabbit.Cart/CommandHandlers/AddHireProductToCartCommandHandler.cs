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
    public class AddHireProductToCartCommand
    {
        public Guid OwnerId { get; private set; }

        public Product Product { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public AddHireProductToCartCommand(Guid ownerId, Product product, DateTime startDate, DateTime endDate)
        {
            this.OwnerId = ownerId;
            this.Product = product;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }
    }

    internal class AddHireProductToCartCommandHandler : ICommandHandler<AddHireProductToCartCommand>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public AddHireProductToCartCommandHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(AddHireProductToCartCommand command)
        {
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(command.OwnerId));

            cart.AddProduct(command.Product, command.StartDate, command.EndDate);

            this._commandDispatcher.Dispatch(new SaveCartCommand(cart));
        }
    }
}
