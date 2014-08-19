using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Blogs.Entities
{
    internal class WordpressCategoryCollection
    {
        public int Found { get; set; }

        public IList<WordpressCategory> Categories { get; set; }
    }
}
