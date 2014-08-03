
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Parties.CommandHandlers;
using Vintage.Rabbit.Parties.Entities;

namespace Vintage.Rabbit.Parties.Messaging.Handlers
{
    internal class OrderPaidMessageHandler : IMessageHandler<IOrderPaidMessage>
    {
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;
        private IQueryDispatcher _queryDispatcher;

        public OrderPaidMessageHandler(IMessageService messageService, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._messageService = messageService;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(IOrderPaidMessage message)
        {
            if(message.Order.Items.Any(o => o.Product.Type == Common.Enums.ProductType.Hire))
            {
                //Party party = new Party(message.Order);

                //this._commandDispatcher.Dispatch(new SavePartyCommand(party, new SystemUpdater()));
            }
        }
    }
}
