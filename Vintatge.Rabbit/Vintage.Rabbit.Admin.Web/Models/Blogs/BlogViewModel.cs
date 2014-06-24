﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Blogs.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Blogs
{
    public class BlogViewModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Key { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateLastModified { get; set; }

        public BlogViewModel() { }

        public BlogViewModel(Blog blog)
        {
            this.Id = blog.Id;
            this.Guid = blog.Guid;
            this.Key = blog.Key;
            this.Title = blog.Title;
            this.Summary = blog.Summary;
            this.Content = blog.Content;
            this.Author = blog.Author;
            this.DateCreated = blog.DateCreated;
            this.DateLastModified = blog.DateLastModified;
        }
    }
}