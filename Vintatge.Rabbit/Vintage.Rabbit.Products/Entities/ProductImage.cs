using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Products.Entities
{
    public class ProductImage
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public string Thumbnail { get; set; }

        public ProductImage()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
