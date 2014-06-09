﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Web.Models.Products
{
    public class ProductImageViewModel
    {
        public string Url { get; private set; }

        public string Thumbnail { get; private set; }


        public ProductImageViewModel(ProductImage image)
        {
            this.Url = image.Url;
            this.Thumbnail = image.Thumbnail;
        }
    }
}