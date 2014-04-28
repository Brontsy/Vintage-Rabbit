using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Serialization;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.Messaging.Messages;

namespace Vintage.Rabbit.Products.Repository
{
    internal interface IProductRepository
    {
        Product GetProduct(int productId);

        IList<Product> GetFeaturedProducts();

        IList<Product> GetProducts(int page);

        IList<Product> GetProductsByCategory(Category category);
    }

    internal class ProductRepository : IProductRepository, IMessageHandler<SaveProductMessage>
    {
        private ISerializer _serializer;

        public ProductRepository(ISerializer serializer)
        {
            this._serializer = serializer;
        }

        public Product GetProduct(int productId)
        {
            return this.GetProducts().First(o => o.Id == productId);
        }

        public void Handle(SaveProductMessage message)
        {
            string serialized = this._serializer.Serialize(message);

            string sql = "Insert Into Query.Product";
        }

        public IList<Product> GetFeaturedProducts()
        {
            return this.GetProducts().Take(6).ToList();
        }

        public IList<Product> GetProducts(int page)
        {
            return this.GetProducts().ToList();
        }

        public IList<Product> GetProductsByCategory(Category category)
        {
            return this.GetProducts().Where(o => o.Categories.Any(x => x.Name == category.Name)).ToList();
        }

        private IList<Product> GetProducts()
        {
            return new List<Product>()
            {
                new Product() { Id = 632881, Title = "Rainbow Stripe Paper Cups", Cost = 6.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/rainbow-cups---hr.jpg" }}},
                new Product() { Id = 254676, Title = "Tall Milk Bottle", Cost = 9.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/age-05-102-spot.jpg" }}},
                new Product() { Id = 642333, Title = "Bottle of lollies", Cost = 7.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/lollies_1.jpg" }}},
                new Product() { Id = 818274, Title = "Rainbow Stripe Paper Cups", Cost = 4.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/sambellina_rainbow_treat_box__23750.1377144079.1280.1280.jpg" }}},
                new Product() { Id = 798133, Title = "Confetti Party in a Box", Cost = 14.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/confetti-party-in-a-box.jpg" }}},
                new Product() { Id = 140289, Title = "Party Confetti", Cost = 8.00M, Categories = new List<Category>() { new Category() { Id = 1, Name = "games" } }, Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/il_570xn.445330436_5xg8.jpg" }}},

                
                new Product() { Id = 135566, Title = "Red Stripe Vintage Candy Store Bags (set of 10)", Cost = 4.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "Our Vintage Candy Store range features sweet paper cups and bags in nostalgic styles for a look you'll love! \r\n These traditional paper bags are perfect to fill with sweets and treats. \r\n18x9 cms with a 5.5cm gusset.", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/candyland-party-ideas-for-kids.jpg" }}},
                new Product() { Id = 54353, Title = "Retro Bobble DIY Gumball Machine", Cost = 19.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/redbobblelores2.jpg" }}},
                new Product() { Id = 343432, Title = "Lilac Spot Vintage Candy Store Baking Cups (set of 25)", Cost = 4.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/cakecup5lores_2.jpg" }}},
                new Product() { Id = 745445, Title = "Felt Party Crowns", Cost = 6.95M, Description = "", Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/crowns_3.jpg" }}},
                new Product() { Id = 2423424, Title = "Tiger Woodland Mask", Cost = 9.95M, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/pm12361_tiger-woodland-mask-3.jpg" }}},
                new Product() { Id = 727888, Title = "Fox Woodland Mask", Cost = 9.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/pm12363_fox-woodland-mask-3.jpg" }}},
                new Product() { Id = 32432, Title = "Bear Woodland Mask", Cost = 9.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "games" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/pm12331_bear-woodland-mask-3.jpg" }}},
                new Product() { Id = 645645, Title = "Little Woodland Toadstool Lights", Cost = 24.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/file_63_9.jpg" }}},
                new Product() { Id = 3426, Title = "10 Paper Party Cups (red dotty)", Cost = 5.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/cup20lores.jpg" }}},
                new Product() { Id = 7755, Title = "Lemon & Peach Large Cake Stand", Cost = 79.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "gifts" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/cakestand_4.jpg" }}},
                new Product() { Id = 349809, Title = "Sweet Collage Paper Cups (set of 6)", Cost = 6.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "gifts" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/file_38_1.jpg" }}},
                new Product() { Id = 87653, Title = "Regency Paper Plates (Set of 8)", Cost = 5.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/23944.jpg" }}},
                new Product() { Id = 54452, Title = "Hope & Greenwood Baking Cases (pack of 12)", Cost = 8.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/baking-cases2.jpg" }}},
                new Product() { Id = 218, Title = "Frills and Frosting Cake Toppers", Cost = 9.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "games" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/20130226172444_1.jpg" }}},
                new Product() { Id = 3577, Title = "Mason Drinking Jar with Daisy Lid", Cost = 8.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/mason2lores_1.jpg" }}},
                new Product() { Id = 8972231, Title = "Yellow Diagonal Stripe Paper Party Bags (pack of 10)", Cost = 4.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/bag37lores.jpg" }}},
                new Product() { Id = 21378, Title = "Carnival Stripe Ceramic Lemonade Bottle (set of 2)", Cost = 12.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/rg-may2013_1293-copy.jpg" }}},
                new Product() { Id = 21345, Title = "Make Lemonade Sign", Cost = 89.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<ProductImage>() { new ProductImage() { Url = "/content/images/products/age-04-125.jpg" }}},
            };
        }
    }
}
