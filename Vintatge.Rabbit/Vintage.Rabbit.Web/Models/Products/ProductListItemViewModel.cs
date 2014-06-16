﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Web.Models.Categories;

namespace Vintage.Rabbit.Web.Models.Products
{
    public class ProductListItemViewModel
    {
        public int Id { get; private set; }

        public ProductImageViewModel Image { get; private set; }

        public string Cost { get; private set; }

        public string Title { get; private set; }

        public IList<CategoryViewModel> Categories { get; private set; }

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
            this.Cost = product.Cost.ToString("C");
            this.Title = product.Title;
            this.Categories = product.Categories.Select(o => new CategoryViewModel(o)).ToList();
        }
    }
}