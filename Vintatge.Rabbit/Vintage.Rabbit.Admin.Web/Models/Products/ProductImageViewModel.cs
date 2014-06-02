using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Products
{
    public class ProductImageViewModel
    {
        public string Url { get; private set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public ProductImageViewModel(ProductImage image)
        {
            this.Url = image.Url;
        }
    }
}