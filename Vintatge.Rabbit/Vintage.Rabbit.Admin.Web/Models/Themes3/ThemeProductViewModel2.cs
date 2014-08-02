
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;
//using Vintage.Rabbit.Themes.Entities;

//namespace Vintage.Rabbit.Admin.Web.Models.Themes
//{
//    public class ThemeProductViewModel
//    {
//        public Guid ThemeProductGuid { get; set; }

//        [Required(ErrorMessage = "Please enter the X coordinate (%) for the location on the image")]
//        public decimal? X { get; set; }

//        [Required(ErrorMessage = "Please enter the Y coordinate (%) for the location on the image")]
//        public decimal? Y { get; set; }

//        [Required(ErrorMessage = "Please choose a product to add")]
//        public Guid ProductGuid { get; set; }

//        public ThemeProductViewModel() { }

//        public ThemeProductViewModel(ThemeProduct product)
//        {
//            this.ThemeProductGuid = product.Guid;
//            this.X = product.X;
//            this.Y = product.Y;
//            this.ProductGuid = product.ProductGuid;
//        }
//    }
//}