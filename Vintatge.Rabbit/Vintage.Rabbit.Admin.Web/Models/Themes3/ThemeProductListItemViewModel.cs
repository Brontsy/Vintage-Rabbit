//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Vintage.Rabbit.Admin.Web.Models.Products;
//using Vintage.Rabbit.Products.Entities;
//using Vintage.Rabbit.Themes.Entities;

//namespace Vintage.Rabbit.Admin.Web.Models.Themes
//{
//    public class ThemeProductListItemViewModel
//    {
//        public Guid Guid { get; set; }
//        /// <summary>
//        /// The X position this product sits on the main image of the theme
//        /// </summary>
//        public decimal X { get; set; }

//        /// <summary>
//        /// The Y position this product sits on the main image of the theme
//        /// </summary>
//        public decimal Y { get; set; }

//        public Guid ProductGuid { get; set; }

//        public ProductViewModel Product { get; set; }

//        public ThemeProductListItemViewModel(ThemeProduct themeProduct, Product product)
//        {
//            this.Guid = themeProduct.Guid;
//            this.X = themeProduct.X;
//            this.Y = themeProduct.Y;
//            this.ProductGuid = themeProduct.ProductGuid;
//            this.Product = new ProductViewModel(product, new List<Category>());
//        }
//    }
//}