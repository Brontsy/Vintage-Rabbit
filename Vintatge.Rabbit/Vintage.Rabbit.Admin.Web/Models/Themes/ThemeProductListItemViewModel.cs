using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Themes.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Themes
{
    public class ThemeProductListItemViewModel
    {
        public Guid ThemeProductGuid { get; private set; }
        public Guid ProductGuid { get; private set; }
        public string Thumbnail { get; private set; }

        public string Title { get; private set; }

        public string Cost { get; private set; }

        public ThemeProductListItemViewModel(ThemeProduct themeProduct, Product product) 
        {
            this.ThemeProductGuid = themeProduct.Guid;
            this.ProductGuid = product.Guid;
            this.Title = product.Title;
            this.Thumbnail = product.Images.First().Thumbnail;
            this.Cost = product.Cost.ToString("C");
        }
    }
}