using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Web.Models.Hire
{
    public class HireAddProductToCartViewModel
    {
        public int Qty { get; set; }

        public IList<SelectListItem> InventoryCount { get; set; }

        public int TotalInventoryAvailable { get; set; }

        public bool InCart { get; set; }

        public string ProductTitle { get; set; }

        public Guid ProductGuid { get; set; }

        public int ProductId { get; set; }

        public string UrlTitle
        {
            get { return this.ProductTitle.ToUrl(); }
        }

        public DateTime PartyDate { get; set; }

        public HireAddProductToCartViewModel(Product product, int totalInventoryAvailable, bool inCart, DateTime partyDate)
        {
            this.TotalInventoryAvailable = totalInventoryAvailable;
            this.InCart = inCart;
            this.ProductTitle = product.Title;
            this.ProductGuid = product.Guid;
            this.ProductId = product.Id;
            this.PartyDate = partyDate;

            this.InventoryCount = new List<SelectListItem>();
            for(int i = 1; i <= totalInventoryAvailable; i++)
            {
                this.InventoryCount.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
        }
    }
}