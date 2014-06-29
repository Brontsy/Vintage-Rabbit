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
        /// <summary>
        /// The X position this product sits on the main image of the theme
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The Y position this product sits on the main image of the theme
        /// </summary>
        public int Y { get; set; }

        public Guid ProductGuid { get; set; }
    }
}
