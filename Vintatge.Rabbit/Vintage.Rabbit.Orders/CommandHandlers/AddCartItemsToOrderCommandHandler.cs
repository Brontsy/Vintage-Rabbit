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
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.QueryHandlers;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Parties.CommandHandlers;

namespace Vintage.Rabbit.Orders.CommandHandlers
{
    public class AddCartItemsToOrderCommand
    {
        public Member Member { get; private set; }

        public Guid OrderGuid { get; private set; }

        public Cart Cart { get; private set; }

        public DateTime? PartyDate { get; private set; }

        public AddCartItemsToOrderCommand(Guid orderGuid, Member member, Cart cart, DateTime? partyDate = null)
        {
            this.Member = member;
            this.Cart = cart;
            this.PartyDate = partyDate;
        }
    }

    internal class AddCartItemsToOrderCommandHandler : ICommandHandler<AddCartItemsToOrderCommand>
    {
        private ICommandDispatcher _commandDispatcher;

        public AddCartItemsToOrderCommandHandler(ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(AddCartItemsToOrderCommand command)
        {
            Order order = new Order(command.OrderGuid, command.Member.Guid);

            order.Clear();
            foreach(var cartItem in command.Cart.Items)
            {
                order.AddProduct(cartItem);
            }

            if (command.Cart.Items.Any(o => o.Product.Type == ProductType.Buy))
            {
                order.AddDelivery(new Delivery("Delivery", 9.95M));
            }

            this._commandDispatcher.Dispatch<SaveOrderCommand>(new SaveOrderCommand(order));

            if(order.Items.Any(o => o.Product.Type == ProductType.Hire) || order.Items.Any(o => o.Product.Type == ProductType.Theme))
            {
                if(command.PartyDate.HasValue)
                {
                    this._commandDispatcher.Dispatch(new CreatePartyCommand(order, command.PartyDate.Value, command.Member));
                }
                else
                {
                    // TODO: TRYING TO CREATE A ORDER WHEN NO PARTY DATE HAS BEEN SET
                }
            }
        }
    }
}
