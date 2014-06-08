using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Products.Enums;

namespace Vintage.Rabbit.Products.Entities
{
    public class Product
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

        public IList<Inventory> Inventory { get; internal set; }

        public int InventoryCount { get; set; }

        public Product()
        {
            this.Images = new List<ProductImage>();
            this.Categories = new List<Category>();
            this.Inventory = new List<Inventory>();
        }
        public Product(Guid guid) : this()
        {
            this.Guid = guid;
        }

        internal bool IsAvailableForHire(DateTime startDate, DateTime endDate)
        {
            if (this.Type == ProductType.Hire)
            {
                return this.Inventory.Any(o => o.IsAvailable(startDate, endDate));
            }

            return false;
        }
    }
}
