using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Orders.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }

        public Product Product { get; private set; }

        public OrderItem()
        {
            this.Id = Guid.NewGuid();
        }

        public OrderItem(Product product) : this()
        {
            this.Product = product;
        }
    }
}
