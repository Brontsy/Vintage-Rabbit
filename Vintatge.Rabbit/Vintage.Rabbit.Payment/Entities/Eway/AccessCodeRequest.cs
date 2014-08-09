using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Orders;

namespace Vintage.Rabbit.Payment.Entities.Eway
{
    public class AccessCodeRequest
    {
        public Payment Payment { get; set; }

        public string RedirectUrl { get; set; }

        public string Method
        {
            get { return "ProcessPayment"; }
        }

        public string TransactionType { get; set; }

        public AccessCodeRequest(IOrder order)
        {
            this.Payment = new Payment(order);
            this.TransactionType = "Purchase";
        }
    }


}
//{
//    "Payment": {
//       "TotalAmount": 112
//    },
//    "RedirectUrl": "http://www.dev.vintagerabbit.com.au",
//    "Method": "ProcessPayment",
//    "TransactionType": "Purchase"
//}