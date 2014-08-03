using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Orders;

namespace Vintage.Rabbit.Interfaces.Inventory
{
    public interface IProductPurchasedMessage
    {
        IOrderItem OrderItem { get; }
    }
}
