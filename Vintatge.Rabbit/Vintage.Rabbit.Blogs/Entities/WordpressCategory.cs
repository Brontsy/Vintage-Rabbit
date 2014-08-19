using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Blogs.Entities
{
    internal class WordpressCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        [JsonProperty("post_count")]
        public int PostCount { get; set; }
    }
}
