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

namespace Vintage.Rabbit.Payment.CommandHandlers
{
    public class SaveEwayPaymentCommand
    {
        public EWayPayment EwayPayment { get; private set; }

        public SaveEwayPaymentCommand(EWayPayment ewayPayment)
        {
            this.EwayPayment = ewayPayment;
        }
    }

    internal class SaveEwayPaymentCommandHandler : ICommandHandler<SaveEwayPaymentCommand>
    {
        private IEWayPaymentRepository _creditCardRepository;
        private IMessageService _messageService;

        public SaveEwayPaymentCommandHandler(IEWayPaymentRepository creditCardRepository, IMessageService messageService)
        {
            this._creditCardRepository = creditCardRepository;
            this._messageService = messageService;
        }

        public void Handle(SaveEwayPaymentCommand command)
        {
            this._creditCardRepository.SaveEwayPayment(command.EwayPayment);
        }
    }
}
