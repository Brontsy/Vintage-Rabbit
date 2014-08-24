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

        public string Description { get; set; }

        public string SEOTitle { get; set; }

        public string SEODescription { get; set; }

        public string SEOKeywords { get; set; }

        public CategoryViewModel(Category category)
        {
            this.Id = category.Id;
            this.Name = category.Name;
            this.DisplayName = category.DisplayName;
            this.Description = category.Description;
            this.Children = category.Children.Select(o => new CategoryViewModel(o)).ToList();
            this.SEOTitle = category.SEOTitle;
            this.SEODescription = category.SEODescription;
            this.SEOKeywords = category.SEOKeywords;
        }
    }
}