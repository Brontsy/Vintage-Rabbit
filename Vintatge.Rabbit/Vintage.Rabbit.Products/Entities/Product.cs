using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Products;
using Vintage.Rabbit.Products.Enums;

namespace Vintage.Rabbit.Products.Entities
{
    public class Product : IProduct
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Code { get; set; }

        public ProductType Type { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public decimal Cost { get; set; }

        public bool IsFeatured { get; set; }

        public IList<ProductImage> Images { get; set; }

        public IList<Category> Categories { get; set; }

        public int Inventory { get; set; }

        public Product()
        {
            this.Images = new List<ProductImage>();
            this.Categories = new List<Category>();
        }
        public Product(Guid guid, int inventoryCount) : this()
        {
            this.Guid = guid;
            this.Inventory = inventoryCount;
        }
    }
}
