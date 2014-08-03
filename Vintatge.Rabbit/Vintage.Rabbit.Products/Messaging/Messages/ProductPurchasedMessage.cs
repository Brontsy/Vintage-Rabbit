using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Interfaces.Orders;

namespace Vintage.Rabbit.Products.Messaging.Messages
{
    internal class ProductPurchasedMessage : IProductPurchasedMessage
    {
        public IOrderItem OrderItem { get; set; }
    }
}
