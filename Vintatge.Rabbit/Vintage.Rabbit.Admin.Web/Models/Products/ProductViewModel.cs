using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Admin.Web.Models.Categories;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Products
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public Guid ProductGuid { get; set; }

        [Display(Name = "Code*")]
        [Required(ErrorMessage = "Please enter a product code")]
        public string Code { get; set; }

        [Display(Name = "Product Type*")]
        [Required(ErrorMessage = "Please choose the product type")]
        public ProductType? Type { get; set; }

        [Display(Name = "Title*")]
        [Required(ErrorMessage = "Please enter the products title")]
        public string Title { get; set; }

        [Display(Name = "Description*")]
        [Required(ErrorMessage = "Please enter the products description")]
        public string Description { get; set; }

        [Display(Name = "Keywords")]
        public string Keywords { get; set; }

        [Display(Name = "Price*")]
        [Required(ErrorMessage = "Please enter the products price")]
        public decimal? Cost { get; set; }

        [Display(Name = "Feature this product on the homepage?")]
        public bool IsFeatured { get; set; }

        public IList<CategoryViewModel> Categories { get; set; }

        [Display(Name = "Images")]
        public IList<ProductImageViewModel> ImageUrls { get; set; }

        public string UrlTitle
        {
            get { return this.Title.ToUrl(); }
        }

        public IList<SelectListItem> Types
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "Buy", Value = "Buy"},
                    new SelectListItem() { Text = "Hire", Value = "Hire"}
                };
            }
        }

        public ProductViewModel()
        {
            this.Categories = new List<CategoryViewModel>();
            this.ImageUrls = new List<ProductImageViewModel>();
        }

        public ProductViewModel(Product product, IList<Category> categories)
            : this()
        {
            this.ProductId = product.Id;
            this.ProductGuid = product.Guid;
            this.Code = product.Code;
            this.Title = product.Title;
            this.Description = product.Description;
            this.Keywords = product.Keywords;
            this.Cost = product.Cost;
            this.Type = product.Type;
            this.IsFeatured = product.IsFeatured;
            this.ImageUrls = product.Images.Select(o => new ProductImageViewModel(o)).ToList();

            foreach (Category category in categories.OrderBy(o => o.DisplayName))
            {
                CategoryViewModel categoryViewModel = new CategoryViewModel(category);
                categoryViewModel.Selected = product.Categories.Any(o => o.Id == category.Id);

                foreach(var child in category.Children)
                {
                    categoryViewModel.Children.First(o => o.Id == child.Id).Selected = product.Categories.Any(o => o.Id == category.Id && o.Children.Any(x => x.Id == child.Id));
                }

                this.Categories.Add(categoryViewModel);
            }
        }
    }
}