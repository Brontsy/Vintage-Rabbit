using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Web.Models.Categories
{
    public class CategoryViewModel
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public string DisplayName { get; private set; }

        public IList<CategoryViewModel> Children { get; private set; }

        public bool Selected { get; set; }

        public CategoryViewModel(Category category)
        {
            this.Id = category.Id;
            this.Name = category.Name;
            this.DisplayName = category.DisplayName;
            this.Children = category.Children.Select(o => new CategoryViewModel(o)).ToList();
        }
    }
}