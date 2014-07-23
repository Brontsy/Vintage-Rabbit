
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
    internal class PayPalPaymentCreatedHandler : IMessageHandler<PayPalPaymentCreatedMessage>
    {
        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;

        public PayPalPaymentCreatedHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(PayPalPaymentCreatedMessage message)
        {
            Order order = this._queryDispatcher.Dispatch<Order, GetOrderQuery>(new GetOrderQuery(message.PayPalPayment.OrderGuid));
            order.PaymentMethod = Payment.Enums.PaymentMethod.PayPal;

            this._commandDispatcher.Dispatch(new SaveOrderCommand(order));
        }
    }
}
