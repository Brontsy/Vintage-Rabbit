using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Products.Messaging.Messages
{
    public class SaveProductMessage : IMessage
    {
        public Product Product { get; private set; }

        public SaveProductMessage(Product product)
        {
            this.Product = product;
        }
    }
}
