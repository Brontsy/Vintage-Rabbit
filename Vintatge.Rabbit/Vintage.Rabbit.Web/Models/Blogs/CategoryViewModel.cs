using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Blogs.Entities;

namespace Vintage.Rabbit.Web.Models.Blogs
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UrlTitle { get; set; }

        public int PostCount { get; set; }


        public CategoryViewModel() { }

        public CategoryViewModel(Category category)
        {
            this.Id = category.Id;
            this.Name = category.Name;
            this.UrlTitle = category.UrlTitle;
            this.PostCount = category.PostCount;
        }
    }
}