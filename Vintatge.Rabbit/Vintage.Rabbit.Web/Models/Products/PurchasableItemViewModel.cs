using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Interfaces.Products;

namespace Vintage.Rabbit.Web.Models.Products
{
    public class PurchasableItemViewModel
    {
        public Guid Guid { get; set; }

        public ProductType Type { get; set; }

        public string Title { get; set; }

        public string UrlTitle
        {
            get { return this.Title.ToUrl(); }
        }
        public decimal Cost { get; set; }

        public PurchasableItemViewModel(IPurchaseable item)
        {
            this.Guid = item.Guid;
            this.Type = item.Type;
            this.Title = item.Title;
            this.Cost = item.Cost;
        }
    }
}