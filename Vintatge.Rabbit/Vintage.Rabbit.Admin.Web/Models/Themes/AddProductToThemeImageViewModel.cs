using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Themes.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Themes
{
    public class AddProductToThemeImageViewModel
    {
        public Guid ThemeProductGuid { get; set; }

        public Guid ThemeImageGuid { get; set; }

        [Required(ErrorMessage = "Please enter the X coordinate (%) for the location on the image")]
        public decimal? X { get; set; }

        [Required(ErrorMessage = "Please enter the Y coordinate (%) for the location on the image")]
        public decimal? Y { get; set; }

        [Required(ErrorMessage = "Please choose a product to add")]
        public Guid ProductGuid { get; set; }

        public string Url { get; set; }

        public IList<ThemeProductViewModel> Products { get; set; }

        public Guid? SelectedProductGuid { get; set; }

        public bool IsTallImage { get; set; }

        public AddProductToThemeImageViewModel() { }

        public AddProductToThemeImageViewModel(ThemeImage themeImage, IList<Product> products) 
        {
            this.ThemeProductGuid = Guid.NewGuid();

            this.ThemeImageGuid = themeImage.Guid;
            this.Url = themeImage.Url;
            this.IsTallImage = themeImage.IsTallImage;
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