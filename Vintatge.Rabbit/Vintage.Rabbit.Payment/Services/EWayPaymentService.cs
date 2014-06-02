using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Payment.Entities;

namespace Vintage.Rabbit.Payment.Services
{
    internal interface IPaymentGateway
    {
        CreditCardPaymentResult Pay(CreditCardPayment creditCardPayment);
    }

    internal class CreditCardPaymentResult
    {
        internal bool Successful { get; private set; }

        internal string ErrorMessage { get; private set; }

        private CreditCardPaymentResult() { }

        internal static CreditCardPaymentResult Success()
        {
            return new CreditCardPaymentResult() { Successful = true };
        }

        internal static CreditCardPaymentResult Error(string errorMessage)
        {
            return new CreditCardPaymentResult() { Successful = false, ErrorMessage = errorMessage };
        }
    }

    internal class EwayPaymentService : IPaymentGateway
    {
        public CreditCardPaymentResult Pay(CreditCardPayment creditCardPayment)
        {
            return CreditCardPaymentResult.Success();
        }
    }
}
