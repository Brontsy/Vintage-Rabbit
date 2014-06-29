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

        public IList<ThemeProduct> Products { get; set; }

        public ThemeImage MainImage { get; set; }

        public IList<ThemeImage> Images { get; set; }

        public decimal Cost { get; set; }

        public ProductType Type
        {
            get { return ProductType.Theme; }
        }
    }
}
