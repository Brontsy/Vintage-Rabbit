using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Carts.Entities
{
    public class CartItem
    {
        public Guid Id { get; private set; }

        public Product Product { get; private set; }

        public int Quantity { get; private set; }

        public Dictionary<string, object> Properties { get; private set; }

        public decimal Total
        {
            get { return this.Product.Cost * this.Quantity; }
        }

        public CartItem()
        {
            this.Id = Guid.NewGuid();
            this.Properties = new Dictionary<string, object>();
        }

        public CartItem(int quantity, Product product)
            : this()
        {
            this.Quantity = quantity;
            this.Product = product;
        }

        internal void ChangeQuantity(int quantity)
        {
            this.Quantity = quantity;
        }
    }
}
