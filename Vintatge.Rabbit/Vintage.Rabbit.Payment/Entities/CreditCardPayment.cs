using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Payment.Enums;

namespace Vintage.Rabbit.Payment.Entities
{
    public class CreditCardPayment
    {
        public int Id { get; private set; }

        public Guid Guid { get; private set; }

        public Guid OrderGuid { get; internal set; }

        public string CreditCardNumber { get; internal set; }

        public string Name { get; internal set; }

        public int ExpiryMonth { get; internal set; }

        public int ExpiryYear { get; internal set; }

        public string CCV { get; internal set; }

        public CreditCardPaymentStatus Status { get; internal set; }

        public string ErrorMessage { get; internal set; }

        public CreditCardPayment(IOrder order)
        {
            this.Guid = Guid.NewGuid();
            this.OrderGuid = order.Guid;
        }

        internal void Error(string errorMessage)
        {
            this.Status = CreditCardPaymentStatus.Error;
            this.ErrorMessage = errorMessage;
        }

        internal void Completed()
        {
            this.Status = CreditCardPaymentStatus.Completed;
            this.ErrorMessage = null;
        }

        public void AddCreditCardDetails(string name, string creditCardNumber, int expiryMonth, int expiryYear, string ccv)
        {
            this.Name = name;
            this.CreditCardNumber = creditCardNumber;
            this.ExpiryMonth = expiryMonth;
            this.ExpiryYear = expiryYear;
            this.CCV = ccv;
        }
    }
}
