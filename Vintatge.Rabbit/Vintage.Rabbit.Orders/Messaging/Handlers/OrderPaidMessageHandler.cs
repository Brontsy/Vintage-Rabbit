
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Orders.CommandHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.Messaging.Messages;
using Vintage.Rabbit.Orders.QueryHandlers;
using Vintage.Rabbit.Payment.Messaging.Messages;

namespace Vintage.Rabbit.Orders.Messaging.Handlers
{
    internal class OrderPaidMessageHandler : IMessageHandler<PaymentCompleteMessage>
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

        public void Handle(PaymentCompleteMessage message)
        {
            Order order = this._queryDispatcher.Dispatch<Order, GetOrderQuery>(new GetOrderQuery(message.Order.Guid));
            order.Paid();

            this._commandDispatcher.Dispatch(new SaveOrderCommand(order));

            this._messageService.AddMessage<IOrderPaidMessage>(new OrderPaidMessage(order));
        }
    }
}
