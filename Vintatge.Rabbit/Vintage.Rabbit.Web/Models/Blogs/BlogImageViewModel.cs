using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Blogs.Entities;

namespace Vintage.Rabbit.Web.Models.Blogs
{
    public class BlogImageViewModel
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public string Thumbnail { get; set; }

        public BlogImageViewModel() {}

        public BlogImageViewModel(BlogImage image)
        {
            this.Id = image.Id;
            this.Url = image.Url;
            this.Thumbnail = image.Thumbnail;
        }
    }
}
