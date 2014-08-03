using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Themes.Entities
{
    public class ThemeProduct
    {
        public Guid Guid { get; set; }
        /// <summary>
        /// The X position this product sits on the main image of the theme
        /// </summary>
        public decimal X { get; set; }

        /// <summary>
        /// The Y position this product sits on the main image of the theme
        /// </summary>
        public decimal Y { get; set; }

        public int Qty { get; set; }

        public Guid ProductGuid { get; set; }

        public ThemeProduct() 
        {
            this.Guid = Guid.NewGuid();
        }

        public ThemeProduct(Guid guid, Guid productGuid, int qty, decimal x, decimal y)
        {
            this.Guid = guid;
            this.ProductGuid = productGuid;
            this.Qty = qty;
            this.X = x;
            this.Y = y;
        }
    }
}
