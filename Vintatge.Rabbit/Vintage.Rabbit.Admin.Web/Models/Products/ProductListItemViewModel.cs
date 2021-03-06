﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Common.Extensions;

namespace Vintage.Rabbit.Admin.Web.Models.Products
{
    public class ProductListItemViewModel
    {
        public int Id { get; private set; }

        public ProductImageViewModel Image { get; private set; }

        public string Cost { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public string UrlTitle
        {
            get { return this.Title.ToUrl(); }
        }

        public ProductListItemViewModel(Product product)
        {
            this.Id = product.Id;
            if (product.Images.Any())
            {
                this.Image = new ProductImageViewModel(product.Images.First());
            }
            else
            {
                this.Image = new ProductImageViewModel();
            }
            this.Cost = product.Cost.ToString("C");
            this.Title = product.Title;
            this.Description = product.Description;
        }
    }
}