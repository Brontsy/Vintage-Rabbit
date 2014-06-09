using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Membership;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Products.Messaging.Messages
{
    public class SaveProductMessage : IMessage
    {
        public Product Product { get; private set; }

        public IActionBy ActionBy { get; private set; }

        public SaveProductMessage(Product product, IActionBy actionBy)
        {
            this.Product = product;
            this.ActionBy = actionBy;
        }
    }
}
