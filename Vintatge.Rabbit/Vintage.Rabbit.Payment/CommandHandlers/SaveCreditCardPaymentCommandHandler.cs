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
    public class SaveCreditCardPaymentCommand
    {
        public CreditCardPayment CreditCard { get; private set; }

        public SaveCreditCardPaymentCommand(CreditCardPayment creditCard)
        {
            this.CreditCard = creditCard;
        }
    }

    internal class SaveCreditCardPaymentCommandHandler : ICommandHandler<SaveCreditCardPaymentCommand>
    {
        private ICreditCardRepository _creditCardRepository;
        private IMessageService _messageService;

        public SaveCreditCardPaymentCommandHandler(ICreditCardRepository creditCardRepository, IMessageService messageService)
        {
            this._creditCardRepository = creditCardRepository;
            this._messageService = messageService;
        }

        public void Handle(SaveCreditCardPaymentCommand command)
        {
            // cache etc
            CreditCardPayment creditCardPayment = this._creditCardRepository.SaveCreditCardPayment(command.CreditCard);
        }
    }
}
