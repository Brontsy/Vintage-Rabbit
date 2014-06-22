using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Products;

namespace Vintage.Rabbit.Products.Entities
{
    public class ProductImage : IProductImage
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
