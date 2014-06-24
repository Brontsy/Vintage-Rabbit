using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Blogs.Entities
{
    public class Blog
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; internal set; }

        public DateTime DateLastModified { get; internal set; }
    }
}
