
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Vintage.Rabbit.Products.Entities;
//using Vintage.Rabbit.Themes.Entities;

//namespace Vintage.Rabbit.Admin.Web.Models.Themes
//{
//    public class ThemeImageViewModel
//    {
//        public Guid Guid { get; set; }

//        public string Url { get; set; }

//        public string ThumbnailUrl { get; set; }

//        public IList<ThemeProductListItemViewModel> Products { get; set; }

//        public ThemeImageViewModel(ThemeImage image, IList<Product> products)
//        {
//            this.Guid = image.Guid;
//            this.Url = image.Url;
//            this.ThumbnailUrl = image.ThumbnailUrl;
//            this.Products = new List<ThemeProductListItemViewModel>();

//            foreach (var themeProduct in image.Products)
//            {
//                if (products.Any(o => o.Guid == themeProduct.ProductGuid))
//                {
//                    this.Products.Add(new ThemeProductListItemViewModel(themeProduct, products.First(o => o.Guid == themeProduct.ProductGuid)));
//                }
//            }
//        }
//    }
//}