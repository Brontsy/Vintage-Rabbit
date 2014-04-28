﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Carts.Entities;

namespace Vintage.Rabbit.Web.Models.ShoppingCart
{
    public class CartItemViewModel
    {
        public string Id { get; private set; }

        public string Title { get; private set; }

        public string Key { get; private set; }

        public int Quantity { get; private set; }

        public string Cost { get; private set; }

        public string Total { get; private set; }

        public int ProductId { get; private set; }

        public CartItemViewModel(CartItem item)
        {
            this.Id = item.Id.ToString();
            this.Title = item.Product.Title;
            this.Cost = item.Product.Cost.ToString("C2");
            this.Total = item.Total.ToString("C2");
            this.Quantity = item.Quantity;
            this.Key = item.Product.Title.Replace(" ", "-").ToLower();
            this.ProductId = item.Product.Id;
        }
    }
}