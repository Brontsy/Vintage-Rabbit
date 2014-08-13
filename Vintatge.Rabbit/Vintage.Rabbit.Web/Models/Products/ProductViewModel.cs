using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Web.Models.Breadcrumbs;
using Vintage.Rabbit.Web.Models.Categories;
using Vintage.Rabbit.Web.Models.Products;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Web.Models.Hire;
using Vintage.Rabbit.Products.Helpers;

namespace Vintage.Rabbit.Web.Models.Products
{
    public class ProductViewModel
    {
        public int Id { get; private set; }

        public Guid Guid { get; private set; }

        public IList<ProductImageViewModel> Images { get; private set; }

        public string Cost { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public bool IsAvailable { get; protected set; }

        public ProductType Type { get; private set; }

        public string SeoKeywords { get; private set; }

        public bool IsCustomisableInvitation { get; private set; }

        public string UrlTitle
        {
            get { return this.Title.ToUrl(); }
        }

        public IList<CategoryViewModel> Categories { get; private set; }

        public ProductViewModel(Product product)
        {
            this.Id = product.Id;
            this.Guid = product.Guid;
            this.Images = product.Images.Select(o => new ProductImageViewModel(o)).ToList();
            this.Cost = product.Cost.ToString("C");
            this.Title = product.Title;
            this.Description = product.Description;
            this.IsAvailable = product.Inventory > 0;
            this.Categories = product.Categories.Select(o => new CategoryViewModel(o)).ToList();
            this.Type = product.Type;
            this.SeoKeywords = product.Keywords;
            this.IsCustomisableInvitation = ProductHelper.IsCustomisableInvitation(product);
        }
    }
}