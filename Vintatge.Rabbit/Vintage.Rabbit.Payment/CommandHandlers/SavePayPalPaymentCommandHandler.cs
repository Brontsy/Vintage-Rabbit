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
    public class SavePayPalPaymentCommand
    {
        public PayPalPayment PayPalPayment { get; private set; }

        public SavePayPalPaymentCommand(PayPalPayment payPalPayment)
        {
            this.PayPalPayment = payPalPayment;
        }
    }

    internal class SavePayPalPaymentCommandHandler : ICommandHandler<SavePayPalPaymentCommand>
    {
        private IPayPalRepository _payPalRepository;

        public SavePayPalPaymentCommandHandler(IPayPalRepository payPalRepository)
        {
            this._payPalRepository = payPalRepository;
        }

        public void Handle(SavePayPalPaymentCommand command)
        {
            this._payPalRepository.SavePayPalPayment(command.PayPalPayment);
        }
    }
}
