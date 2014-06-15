using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Carts.Messaging.Messages
{
    public class ProductAddedToCart : IMessage
    {
        public Cart Cart { get; set; }

        public Product Product { get; set; }

        public ProductAddedToCart() { }

        public ProductAddedToCart(Cart cart, Product product)
        {
            this.Cart = cart;
            this.Product = product;
        }
    }
}
