using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Web.Models.Products
{
    public class ProductListItemViewModel
    {
        public int Id { get; private set; }

        public ProductImageViewModel Image { get; private set; }

        public string Cost { get; private set; }

        public string Title { get; private set; }

        public string UrlTitle
        {
            get { return this.Title.Replace(" ", "-").ToLower(); }
        }

        public ProductListItemViewModel(Product product)
        {
            this.Id = product.Id;
            this.Image = new ProductImageViewModel(product.Images.First());
            this.Cost = product.Cost.ToString("C");
            this.Title = product.Title;
        }
    }
}