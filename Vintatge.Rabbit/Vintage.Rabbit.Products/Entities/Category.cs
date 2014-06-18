using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Enums;

namespace Vintage.Rabbit.Products.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public IList<ProductType> ProductTypes { get; set; }

        public IList<Category> Children { get; set; }

        public Category()
        {
            this.ProductTypes = new List<ProductType>();
            this.Children = new List<Category>();
        }
    }
}
