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

namespace Vintage.Rabbit.Payment.CommandHandlers
{
    public class InitialiseEwayCreditCardPaymentCommand
    {
        public AccessCodeResponse AccessCodeResponse { get; private set; }

        public IOrder Order { get; private set; }

        public InitialiseEwayCreditCardPaymentCommand(IOrder order, AccessCodeResponse accessCodeResponse)
        {
            this.Order = order;
            this.AccessCodeResponse = accessCodeResponse;
        }
    }

    internal class InitialiseEwayCreditCardPaymentCommandHandler : ICommandHandler<InitialiseEwayCreditCardPaymentCommand>
    {
        private ICommandDispatcher _commandDispatcher;

        public InitialiseEwayCreditCardPaymentCommandHandler(ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(InitialiseEwayCreditCardPaymentCommand command)
        {
            EWayPayment payment = new EWayPayment(command.Order, command.AccessCodeResponse);

            this._commandDispatcher.Dispatch(new SaveEwayPaymentCommand(payment));
        }
    }
}
