using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Products;

namespace Vintage.Rabbit.Interfaces.Orders
{
    public interface IOrderPaidMessage : IMessage
    {
        IOrder Order { get; }
    }
}
