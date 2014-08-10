using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Payment.Entities;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Payment.Repository;
using Vintage.Rabbit.Payment.Entities.Eway;
using Vintage.Rabbit.Payment.QueryHandlers;

namespace Vintage.Rabbit.Payment.CommandHandlers
{
    public class EwayPaymentProcessedCommand
    {
        public EwayPaymentResponse EwayPaymentResponse { get; private set; }

        public IOrder Order { get; private set; }

        public EwayPaymentProcessedCommand(IOrder order, EwayPaymentResponse ewayPaymentResponse)
        {
            this.Order = order;
            this.EwayPaymentResponse = ewayPaymentResponse;
        }
    }

    internal class EwayPaymentProcessedCommandHandler : ICommandHandler<EwayPaymentProcessedCommand>
    {
        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;

        public EwayPaymentProcessedCommandHandler(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(EwayPaymentProcessedCommand command)
        {
            EWayPayment payment =  this._queryDispatcher.Dispatch<EWayPayment, GetEwayPaymentByAccessCodeQuery>(new GetEwayPaymentByAccessCodeQuery(command.EwayPaymentResponse.AccessCode));

            payment.PaymentProcessed(command.EwayPaymentResponse);

            this._commandDispatcher.Dispatch(new SaveEwayPaymentCommand(payment));
        }
    }
}
