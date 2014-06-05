using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Products
{
    public class ProductImageViewModel
    {
        public Guid Id { get; private set; }
        public string Url { get; private set; }

        public string Thumbnail { get; private set; }

        public ProductImageViewModel(ProductImage image)
        {
            this.Id = image.Id;
            this.Url = image.Url;
            this.Thumbnail = image.Thumbnail;
        }

        public ProductImageViewModel() { }
    }
}