using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Payment.CommandHandlers;
using Vintage.Rabbit.Payment.Entities;
using Vintage.Rabbit.Payment.Messaging.Messages;

namespace Vintage.Rabbit.Payment.Services
{
    public interface ICreditCardService
    {
        PaymentResult PayForOrder(IOrder order, string name, string creditCardNumber, int expiryMonth, int expiryYear, string ccv);
    }

    internal class CreditCardService : ICreditCardService
    {
        private IPaymentGateway _paymentGateway;
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;

        public CreditCardService(IPaymentGateway paymentGateway, ICommandDispatcher commandDispatcher, IMessageService messageService)
        {
            this._paymentGateway = paymentGateway;
            this._commandDispatcher = commandDispatcher;
            this._messageService = messageService;
        }

        public PaymentResult PayForOrder(IOrder order, string name, string creditCardNumber, int expiryMonth, int expiryYear, string ccv)
        {
            CreditCardPayment creditCardPayment = new CreditCardPayment(order);
            creditCardPayment.AddCreditCardDetails(name, creditCardNumber, expiryMonth, expiryYear, ccv);

            this._commandDispatcher.Dispatch(new SaveCreditCardPaymentCommand(creditCardPayment));

            CreditCardPaymentResult result = this._paymentGateway.Pay(creditCardPayment);

            if(result.Successful)
            {
                creditCardPayment.Completed();
                this._commandDispatcher.Dispatch(new SaveCreditCardPaymentCommand(creditCardPayment));

                this._messageService.AddMessage(new PaymentCompleteMessage(order));

                return PaymentResult.Success();
            }

            return PaymentResult.Error(result.ErrorMessage);
        }
    }
}
