using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Web.Models.Products;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Models.Themes
{
    public class ThemeProductViewModel
    {
        public Guid Guid { get; set; }
        /// <summary>
        /// The X position this product sits on the main image of the theme
        /// </summary>
        public decimal X { get; set; }

        /// <summary>
        /// The Y position this product sits on the main image of the theme
        /// </summary>
        public decimal Y { get; set; }

        public Guid ProductGuid { get; set; }

        public int ProductId { get; set; }

        public string UrlTitle
        {
            get { return this.Title.ToUrl(); }
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Cost { get; private set; }

        public int Qty { get; private set; }

        public string SeoKeywords { get; private set; }

        public IList<ProductImageViewModel> Images { get; private set; }

        public ThemeProductViewModel(ThemeProduct themeProduct, Product product)
        {
            this.Guid = themeProduct.Guid;
            this.X = themeProduct.X;
            this.Y = themeProduct.Y;
            this.ProductGuid = themeProduct.ProductGuid;
            this.Cost = product.Cost.ToString("C");
            this.Title = product.Title;
            this.ProductId = product.Id;
            this.Description = product.Description;
            this.Qty = themeProduct.Qty;
            this.SeoKeywords = product.Keywords;
            this.Images = product.Images.Select(o => new ProductImageViewModel(o)).ToList();
        }
    }
}