using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Inventory;
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

        public int Inventory { get { return 1; } }

        public ProductType Type
        {
            get { return ProductType.Theme; }
        }

        public Theme(Guid guid)
        {
            this.Guid = guid;
            this.Images = new List<ThemeImage>();
        }

        public bool IsAvailable(DateTime partyDate, IList<IInventoryItem> inventory)
        {
            foreach(var image in this.Images)
            {
                foreach(var product in image.Products)
                {
                    if(inventory.Where(o => o.ProductGuid == product.ProductGuid && o.IsAvailable(partyDate)).Count() < product.Qty)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Dictionary<Guid, int> GetProductGuids()
        {
            Dictionary<Guid, int> products = new Dictionary<Guid, int>();

            foreach (var image in this.Images)
            {
                foreach (var product in image.Products)
                {
                    if (!products.ContainsKey(product.ProductGuid))
                    {
                        products.Add(product.ProductGuid, product.Qty);
                    }
                }
            }

            return products;
        }
    }
}
