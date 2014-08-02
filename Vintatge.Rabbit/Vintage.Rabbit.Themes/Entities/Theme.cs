using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Products;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Themes.Entities
{
    public class Theme : IPurchaseable
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string IncludedItems { get; set; }

        public IList<ThemeImage> Images { get; set; }

        public decimal Cost { get; set; }

        public ProductType Type
        {
            get { return ProductType.Theme; }
        }

        public Theme(Guid guid)
        {
            this.Guid = guid;
            this.Images = new List<ThemeImage>();
        }
    }
}
