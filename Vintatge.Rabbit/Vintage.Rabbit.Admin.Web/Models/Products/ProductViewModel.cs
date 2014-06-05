using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Admin.Web.Models.Categories;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Products
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        [Display(Name = "Code")]
        [Required(ErrorMessage = "Please enter a product code")]
        public string Code { get; set; }

        [Display(Name = "Product Type")]
        [Required(ErrorMessage = "Please choose the product type")]
        public string Type { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Please enter the products title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Please enter the products description")]
        public string Description { get; set; }

        [Display(Name = "Description")]
        public string Keywords { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Please enter the products price")]
        public decimal? Cost { get; set; }

        [Display(Name = "Inventory")]
        [Required(ErrorMessage = "Please enter the products inventory count")]
        public int? Inventory { get; set; }

        public IList<CategoryViewModel> Categories { get; set; }

        [Display(Name = "Images")]
        public IList<ProductImageViewModel> ImageUrls { get; set; }

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
            this.Code = product.Code;
            this.Title = product.Title;
            this.Description = product.Description;
            this.Keywords = product.Keywords;
            this.Cost = product.Cost;
            this.Inventory = (product is BuyProduct ? ((BuyProduct)product).InventoryCount : 1);
            this.Type = (product is BuyProduct ? "Buy" : "Hire");
            this.ImageUrls = product.Images.Select(o => new ProductImageViewModel(o)).ToList();

            foreach (Category category in categories)
            {
                CategoryViewModel categoryViewModel = new CategoryViewModel(category);
                if (product.Categories.Any(o => o.Id == category.Id))
                {
                    categoryViewModel.Selected = true;
                }

                this.Categories.Add(categoryViewModel);
            }
        }
    }
}