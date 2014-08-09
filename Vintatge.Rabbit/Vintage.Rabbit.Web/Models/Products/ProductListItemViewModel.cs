using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Web.Models.Categories;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Products.Helpers;

namespace Vintage.Rabbit.Web.Models.Products
{
    public class ProductListItemViewModel
    {
        public int Id { get; private set; }

        public Guid Guid { get; private set; }

        public ProductImageViewModel Image { get; private set; }

        public string Cost { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public ProductType Type { get; private set; }

        public IList<CategoryViewModel> Categories { get; private set; }

        public bool IsCustomisableInvitation { get; private set; }

        public string UrlTitle
        {
            get { return this.Title.ToUrl(); }
        }

        public ProductListItemViewModel(Product product)
        {
            this.Id = product.Id;
            this.Guid = product.Guid;
            if (product.Images.Any())
            {
                this.Image = new ProductImageViewModel(product.Images.First());
            }
            this.Cost = product.Cost.ToString("C");
            this.Title = product.Title;
            this.Description = product.Description;
            this.Type = product.Type;
            this.Categories = product.Categories.Select(o => new CategoryViewModel(o)).ToList();
            this.IsCustomisableInvitation = ProductHelper.IsCustomisableInvitation(product);
        }
    }
}