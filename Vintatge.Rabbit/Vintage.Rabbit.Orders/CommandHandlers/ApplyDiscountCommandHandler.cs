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
using Vintage.Rabbit.Orders.QueryHandlers;
using Vintage.Rabbit.Orders.Enums;

namespace Vintage.Rabbit.Orders.CommandHandlers
{
    public class ApplyDiscountCommand
    {
        public Order Order { get; private set; }

        public LoyaltyCard LoyaltyCard { get; private set; }

        public ApplyDiscountCommand(Order order, LoyaltyCard loyaltyCard)
        {
            this.Order = order;
            this.LoyaltyCard = loyaltyCard;
        }
    }

    internal class ApplyDiscountCommandHandler : ICommandHandler<ApplyDiscountCommand>
    {
        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;

        public ApplyDiscountCommandHandler(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(ApplyDiscountCommand command)
        {
            Order order = command.Order;

            if(command.LoyaltyCard.Status == LoyaltyCardStatus.Available)
            {
                order.AddloyaltyCard(command.LoyaltyCard);
                this._commandDispatcher.Dispatch<SaveOrderCommand>(new SaveOrderCommand(order));
            }
        }
    }
}
