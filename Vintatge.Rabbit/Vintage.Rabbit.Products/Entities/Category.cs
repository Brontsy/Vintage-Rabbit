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

        public ProductType ProductType { get; set; }

        public IList<Category> Children { get; set; }

        public string Description { get; set; }

        public string SEOTitle { get; set; }

        public string SEODescription { get; set; }

        public string SEOKeywords { get; set; }

        public Category()
        {
            this.Children = new List<Category>();
        }
    }
}
