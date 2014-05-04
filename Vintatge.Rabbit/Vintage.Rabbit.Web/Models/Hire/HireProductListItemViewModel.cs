using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Web.Models.Products;

namespace Vintage.Rabbit.Web.Models.Hire
{
    public class HireProductListItemViewModel
    {
        public int Id { get; private set; }

        public ProductImageViewModel Image { get; private set; }

        public string Cost { get; private set; }

        public string Title { get; private set; }

        public string UrlTitle
        {
            get { return this.Title.Replace(" ", "-").ToLower(); }
        }

        public HireProductListItemViewModel(HireProduct product)
        {
            this.Id = product.Id;
            this.Image = new ProductImageViewModel(product.Images.First());
            this.Cost = product.Cost.ToString("C");
            this.Title = product.Title;
        }
    }
}