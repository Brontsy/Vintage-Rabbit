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

        public int Qty { get; private set; }

        public ProductType Type { get; private set; }

        public IList<SelectListItem> InventoryCount { get; private set; }

        public string UrlTitle
        {
            get { return this.Title.ToUrl(); }
        }

        public BreadcrumbsViewModel Breadcrumbs { get; private set; }

        public IList<CategoryViewModel> Categories { get; private set; }

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
            this.Categories = product.Categories.Select(o => new CategoryViewModel(o)).ToList();
            this.Type = product.Type;

            this.Qty = 1;
            this.InventoryCount = new List<SelectListItem>();
            for(int i = 1; i <= product.Inventory; i++)
            {
                this.InventoryCount.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
        }
    }
}