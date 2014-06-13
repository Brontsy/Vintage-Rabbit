using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Orders.Messaging.Messages
{
    public class OrderPaidMessage : IOrderPaidMessage
    {
        public IOrder Order { get; private set; }

        public OrderPaidMessage(IOrder order)
        {
            this.Order = order;
        }
    }
}
