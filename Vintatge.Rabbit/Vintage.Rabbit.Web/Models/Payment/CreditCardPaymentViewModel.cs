using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Payment.Entities;
using Vintage.Rabbit.Payment.Enums;

namespace Vintage.Rabbit.Web.Models.Payment
{
    public class CreditCardPaymentViewModel
    {
        public Guid Id { get; private set; }

        public CreditCardPaymentStatus Status { get; private set; }

        public string ErrorMessage { get; private set; }

        public CreditCardPaymentViewModel(CreditCardPayment creditCardPayment)
        {
            this.Id = creditCardPayment.Id;
            this.Status = creditCardPayment.Status;
            this.ErrorMessage = creditCardPayment.ErrorMessage;
        }
    }
}