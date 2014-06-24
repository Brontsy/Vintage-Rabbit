using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Blogs.Entities;

namespace Vintage.Rabbit.Web.Models.Blogs
{
    public class BlogViewModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Key { get; set; }

        [Required(ErrorMessage = "Please enter a title for the blog entry")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter the content for the blog entry")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        public IList<BlogImageViewModel> Images { get; set; }

        [Required(ErrorMessage = "Please enter the author for the blog entry")]
        [Display(Name = "Author")]
        public string Author { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateLastModified { get; set; }

        public string LastModifiedBy { get; set; }

        public BlogViewModel() { }

        public BlogViewModel(Blog blog)
        {
            this.Id = blog.Id;
            this.Guid = blog.Guid;
            this.Key = blog.Key;
            this.Title = blog.Title;
            this.Content = blog.Content;
            this.Images = blog.Images.Select(o => new BlogImageViewModel(o)).ToList();
            this.Author = blog.Author;
            this.DateCreated = blog.DateCreated;
            this.DateLastModified = blog.DateLastModified;
            this.LastModifiedBy = blog.LastModifiedBy;
        }
    }
}