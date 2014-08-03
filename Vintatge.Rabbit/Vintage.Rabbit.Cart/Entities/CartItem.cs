using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Products;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Themes.Entities;

namespace Vintage.Rabbit.Carts.Entities
{
    public class CartItem
    {
        public Guid Id { get; private set; }

        public IPurchaseable Product { get; private set; }

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

        public CartItem(Theme theme, DateTime partyDate)
            : this()
        {
            this.Quantity = 1;
            this.Product = theme;
            this.Properties["PartyDate"] = partyDate;
        }

        internal void ChangeQuantity(int quantity)
        {
            this.Quantity = quantity;
        }
    }
}
