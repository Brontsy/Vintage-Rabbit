using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Products.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public decimal Cost { get; set; }

        public IList<ProductImage> Images { get; set; }

        public IList<Category> Categories { get; set; }

        public Product()
        {
            this.Images = new List<ProductImage>();
            this.Categories = new List<Category>();
        }
    }
}
