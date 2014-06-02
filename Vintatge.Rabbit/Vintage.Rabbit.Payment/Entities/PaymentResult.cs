using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Payment.Entities
{
    public class PaymentResult
    {
        public bool Successful { get; private set; }

        public string ErrorMessage { get; private set; }

        private PaymentResult() { }

        internal static PaymentResult Success()
        {
            return new PaymentResult() { Successful = true };
        }

        internal static PaymentResult Error(string errorMessage)
        {
            return new PaymentResult() { Successful = false, ErrorMessage = errorMessage };
        }
    }
}
