using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Carts.Entities
{
    public class ProductCartItem
    {
        public int Id { get; internal set; }

        public string Title { get; internal set; }

        public string Description { get; internal set; }

        public decimal Cost { get; internal set; }

        public IList<ProductImage> Images { get; internal set; }

        public IList<Category> Categories { get; internal set; }

        public ProductCartItem(Product product)
        {
            this.Id = product.Id;
            this.Title = product.Title;
            this.Description = product.Description;
            this.Cost = product.Cost;
            this.Images = product.Images;
            this.Categories = product.Categories;
        }

        public ProductCartItem() { }
    }
}
