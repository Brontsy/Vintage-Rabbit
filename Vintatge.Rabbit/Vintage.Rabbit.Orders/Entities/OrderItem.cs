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
        public int Id { get; internal set; }

        public Guid Guid { get; internal set; }

        public IPurchaseable Product { get; internal set; }

        public int Quantity { get; internal set; }

        public DateTime DateCreated { get; internal set; }

        public decimal Total
        {
            get { return this.Product.Cost * this.Quantity; }
        }

        public Dictionary<string, object> Properties { get; internal set; }

        public OrderItem()
        {
            this.Guid = Guid.NewGuid();
            this.DateCreated = DateTime.Now;
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

        public OrderItem(LoyaltyCard loyaltyCard)
            : this()
        {
            this.Product = loyaltyCard;
            this.Quantity = 1;
        }
    }
}
