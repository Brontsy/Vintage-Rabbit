using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Products.Entities
{
    public class ProductImage
    {
        public Guid Id { get; internal set; }

        public string Url { get; internal set; }

        public ProductImage()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
