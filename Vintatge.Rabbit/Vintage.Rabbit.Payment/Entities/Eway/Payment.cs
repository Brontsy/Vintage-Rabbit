using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Orders;

namespace Vintage.Rabbit.Payment.Entities.Eway
{
    public class Payment
    {
        public int TotalAmount { get; set; }

        public string InvoiceNumber { get; set; }

        public Payment() { }

        public Payment(IOrder order)
        {
            this.TotalAmount = (int)(order.Total * 100);
            this.InvoiceNumber = order.Id.ToString("D5");
        }
    }
}
