﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Payment.Entities;

namespace Vintage.Rabbit.Payment.Messaging.Messages
{
    public class PayPalErrorMessage : IMessage
    {
        public PayPalPayment PayPalPayment { get; private set; }

        public PayPalErrorMessage(PayPalPayment payPalPayment)
        {
            this.PayPalPayment = payPalPayment;
        }
    }
}
