using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Orders.Messaging.Messages
{
    public class SaveOrderMessage : IMessage
    {
        public Order Order { get; private set; }

        public SaveOrderMessage(Order Order)
        {
            this.Order = Order;
        }
    }
}
