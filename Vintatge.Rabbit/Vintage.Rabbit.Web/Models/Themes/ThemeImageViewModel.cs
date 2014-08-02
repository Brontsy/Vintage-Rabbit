using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Themes.Entities;

namespace Vintage.Rabbit.Web.Models.Themes
{
    public class ThemeImageViewModel
    {
        public Guid Guid { get; set; }

        public string Url { get; set; }

        public string ThumbnailUrl { get; set; }

        public IList<ThemeProductViewModel> Products { get; set; }

        public bool IsTallImage { get; set; }

        public ThemeImageViewModel(ThemeImage themeImage, IList<Product> products)
        {
            this.Guid = themeImage.Guid;
            this.Url = themeImage.Url;
            this.IsTallImage = themeImage.IsTallImage;
            this.ThumbnailUrl = themeImage.ThumbnailUrl;
            this.Products = new List<ThemeProductViewModel>();


            foreach (var themeProduct in themeImage.Products)
            {
                if (products.Any(o => o.Guid == themeProduct.ProductGuid))
                {
                    this.Products.Add(new ThemeProductViewModel(themeProduct, products.First(o => o.Guid == themeProduct.ProductGuid)));
                }
            }

        }
        
    }
}