using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Inventory;

namespace Vintage.Rabbit.Products.Messaging.Messages
{
    internal class ProductHiredMessage : IProductHiredMessage
    {
        public Guid ProductGuid { get; set; }

        public int Qty { get; set; }

        public DateTime PartyDate { get; set; }
    }
}
