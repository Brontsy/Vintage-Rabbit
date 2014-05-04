﻿using System;
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

        public ProductCartItem Product { get; private set; }

        public int Quantity { get; private set; }

        public decimal Total
        {
            get { return this.Product.Cost * this.Quantity; }
        }

        public CartItem()
        {
            this.Id = Guid.NewGuid();
        }

        public CartItem(int quantity, ProductCartItem product)
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
