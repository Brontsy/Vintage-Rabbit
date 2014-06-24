using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Blogs.Entities
{
    internal class WordpressBlog
    {
        public IList<WordpressPost> Posts { get; set; }
    }
}
