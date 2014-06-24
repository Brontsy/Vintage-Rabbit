using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Blogs.Entities
{
    public class BlogImage
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public string Thumbnail { get; set; }

        public BlogImage()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
