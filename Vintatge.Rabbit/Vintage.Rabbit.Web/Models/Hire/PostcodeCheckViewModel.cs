using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Web.Models.Products;

namespace Vintage.Rabbit.Web.Models.Hire
{
    public class PostcodeCheckViewModel
    {
        [Required(ErrorMessage = "Please enter a postcode")]
        [Display(Name = "Postcode")]
        public string PostcodeCheck { get; private set; }

        public ProductViewModel Product { get; private set; }

        public PostcodeCheckViewModel() { }

        public PostcodeCheckViewModel(Product product)
        {
            this.Product = new ProductViewModel(product);
        }
    }
}