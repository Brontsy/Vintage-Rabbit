using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Web.Models.Products
{
    public class AddProductToCartViewModel
    {
        public int Qty { get; set; }

        public IList<SelectListItem> InventoryCount { get; set; }

        public int TotalInventoryAvailable { get; set; }

        public bool InCart { get; set; }

        public string ProductTitle { get; set; }

        public AddProductToCartViewModel(Product product, int totalInventoryAvailable, bool inCart)
        {
            this.TotalInventoryAvailable = totalInventoryAvailable;
            this.InCart = inCart;
            this.ProductTitle = product.Title;

            this.InventoryCount = new List<SelectListItem>();
            for(int i = 1; i <= totalInventoryAvailable; i++)
            {
                this.InventoryCount.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
        }
    }
}