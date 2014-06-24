using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Blogs.Entities
{
    internal class WordpressPost
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public string Excerpt { get; set; }

        public WordpressAuthor Author { get; set; }

        public DateTime Date { get; set; }

        public DateTime Modified { get; set; }
    }
}
