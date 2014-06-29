using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Themes.Entities
{
    public class ThemeImage
    {
        public Guid Guid { get; set; }

        public string Url { get; set; }

        public string ThumbnailUrl { get; set; }
    }
}
