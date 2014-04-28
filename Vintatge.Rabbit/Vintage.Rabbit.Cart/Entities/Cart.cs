using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Data;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Carts.Entities
{
    public class Cart
    {
        public Guid Id { get; private set; }

        public Guid MemberId { get; private set; }

        public List<CartItem> Items { get; private set; }

        public decimal Total
        {
            get { return this.Items.Sum(o => o.Total); }
        }

        public Cart()
        {
            this.Id = Guid.NewGuid();
            this.Items = new List<CartItem>();
        }

        public Cart(Guid memberId) : this()
        {
            this.MemberId = memberId;
        }

        internal void AddProduct(int quantity, Product product)
        {
            if (this.Items.Any(o => o.Product.Id == product.Id))
            {
                CartItem cartItem = this.Items.FirstOrDefault(o => o.Product.Id == product.Id);
                cartItem.ChangeQuantity(cartItem.Quantity + quantity);
            }
            else
            {
                this.Items.Add(new CartItem(quantity, product));
            }
        }

        internal void RemoveProduct(Guid cartItemId)
        {
            CartItem cartItem = this.Items.FirstOrDefault(o => o.Id == cartItemId);

            if (cartItem != null)
            {
                this.Items.Remove(cartItem);
            }
        }

        internal void Clear()
        {
            this.Items = new List<CartItem>();
        }
    }
}
