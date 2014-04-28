using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Carts.Entities;

namespace Vintage.Rabbit.Web.Models.ShoppingCart
{
    public class CartViewModel
    {
        public IList<CartItemViewModel> Items { get; private set; }

        public string Total { get; private set; }

        public bool IsOpen { get; private set; }

        public CartViewModel(Cart cart, bool isOpen)
        {
            this.Items = cart.Items.Select(o => new CartItemViewModel(o)).ToList(); ;
            this.Total = cart.Total.ToString("C2");
            this.IsOpen = isOpen;
        }
    }
}