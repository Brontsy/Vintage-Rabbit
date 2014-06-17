using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Web.Models.Categories;

namespace Vintage.Rabbit.Web.Models.Products
{
    public class ProductLinkViewModel
    {
        public int Id { get; private set; }

        public string Title { get; private set; }

        public ProductType Type { get; private set; }

        public string UrlTitle
        {
            get { return this.Title.ToUrl(); }
        }

        public CategoryViewModel Category { get; private set; }

        public ProductLinkViewModel(Product product, Category category)
        {
            this.Id = product.Id;
            this.Title = product.Title;
            this.Category = new CategoryViewModel(category);
            this.Type = product.Type;
        }
    }
}