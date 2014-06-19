using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Categories
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool Selected { get; set; }

        public IList<CategoryViewModel> Children { get; private set; }

        public IList<ProductType> ProductTypes { get; private set; }
        
        public CategoryViewModel() 
        {
            this.Children = new List<CategoryViewModel>();
        }

        public CategoryViewModel(Category category)
        {
            this.Id = category.Id;
            this.Name = category.Name;
            this.DisplayName = category.DisplayName;
            this.ProductTypes = category.ProductTypes;
            this.Children = category.Children.Select(o => new CategoryViewModel(o)).ToList();
        }
    }
}