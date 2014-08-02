using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Themes.Entities
{
    public class ThemeImage
    {
        public Guid Guid { get; set; }

        public string Url { get; set; }

        public string ThumbnailUrl { get; set; }

        public bool IsTallImage { get; set; }

        public IList<ThemeProduct> Products { get; set; }

        public ThemeImage()
        {
            this.Products = new List<ThemeProduct>();
        }

        internal void AddProduct(Guid themeProductGuid, Guid productGuid, decimal x, decimal y)
        {
            var existingProducts = this.Products.Where(o => o.ProductGuid == productGuid).ToList();

            foreach (var existingProduct in existingProducts)
            {
                var themeProduct = this.Products.Remove(existingProduct);
            }

            this.Products.Add(new ThemeProduct(themeProductGuid, productGuid, x, y));
        }
    }
}
