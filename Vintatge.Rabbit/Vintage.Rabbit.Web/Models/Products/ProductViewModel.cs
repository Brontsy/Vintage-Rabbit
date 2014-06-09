using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Web.Models.Breadcrumbs;
using Vintage.Rabbit.Web.Models.Products;

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

        public bool IsAvailable { get; private set; }

        public string UrlTitle
        {
            get { return this.Title.Replace(" ", "-").ToLower(); }
        }

        public BreadcrumbsViewModel Breadcrumbs { get; private set; }

        public ProductViewModel(Product product, BreadcrumbsViewModel breadcrumbs) 
        {
            this.Id = product.Id;
            this.Guid = product.Guid;
            this.Images = product.Images.Select(o => new ProductImageViewModel(o)).ToList();
            this.Cost = product.Cost.ToString("C");
            this.Title = product.Title;
            this.Description = product.Description;
            this.Breadcrumbs = breadcrumbs;
            this.IsAvailable = product.Inventory > 0;
        }
    }
}