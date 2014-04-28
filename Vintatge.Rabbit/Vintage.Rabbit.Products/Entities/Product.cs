using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Products.Entities
{
    public class Product
    {
        public int Id { get; internal set; }

        public string Title { get; internal set; }

        public string Description { get; internal set; }

        public decimal Cost { get; internal set; }

        public IList<ProductImage> Images { get; internal set; }

        public IList<Category> Categories { get; internal set; }

        public Product()
        {
            this.Images = new List<ProductImage>();
            this.Categories = new List<Category>();
        }
    }
}
