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

        public CreditCardPaymentStatus Status { get; internal set; }

        public string ErrorMessage { get; internal set; }

        public string AccessCode { get; internal set; }

        public CreditCardPayment(IOrder order)
        {
            this.Guid = Guid.NewGuid();
            this.OrderGuid = order.Guid;
            this.Status = CreditCardPaymentStatus.Initialised;
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
    }
}
