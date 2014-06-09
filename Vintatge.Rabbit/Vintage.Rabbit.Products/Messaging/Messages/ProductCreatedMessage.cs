using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Products;

namespace Vintage.Rabbit.Products.Messaging.Messages
{
    public class ProductCreatedMessage : IProductCreatedMessage
    {
        public IProduct Product { get; set; }

        public ProductCreatedMessage(IProduct product)
        {
            this.Product = product;
        }
    }
}
