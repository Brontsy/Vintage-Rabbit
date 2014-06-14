using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Interfaces.Products;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Orders.Entities
{
    public class OrderItem : IOrderItem
    {
        public Guid Id { get; private set; }

        public IPurchaseable Product { get; private set; }

        public int Quantity { get; private set; }

        public decimal Total
        {
            get { return this.Product.Cost * this.Quantity; }
        }

        public Dictionary<string, object> Properties { get; private set; }

        public OrderItem()
        {
            this.Id = Guid.NewGuid();
            this.Properties = new Dictionary<string, object>();
        }

        public OrderItem(CartItem cartItem)
            : this()
        {
            this.Product = cartItem.Product;
            this.Properties = cartItem.Properties;
            this.Quantity = cartItem.Quantity;
        }

        public OrderItem(Delivery delivery)
            : this()
        {
            this.Product = delivery;
            this.Quantity = 1;
        }
    }
}
