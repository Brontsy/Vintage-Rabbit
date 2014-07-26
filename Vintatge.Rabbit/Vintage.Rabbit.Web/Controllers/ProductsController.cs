using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Products;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Web.Models.Products;

namespace Vintage.Rabbit.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IQueryDispatcher _queryDispatcher;

        public ProductsController(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Featured()
        {
            IList<Product> featuredProducts = this._queryDispatcher.Dispatch<IList<Product>, GetFeaturedProductsQuery>(new GetFeaturedProductsQuery());

            return this.PartialView("Featured", featuredProducts.Select(o => new ProductListItemViewModel(o)).ToList());
        }

        private IList<Product> GetFeaturedProducts()
        {
            return new List<Product>()
            {
                new Product() { Id = 632881, Title = "Rainbow Stripe Paper Cups", Cost = 6.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/new/gifts-and-games/Doggie hand puppet 3605.jpg" }}},
                new Product() { Id = 254676, Title = "Tall Milk Bottle", Cost = 9.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/new/gifts-and-games/Hemp twine variegated 3578.jpg" }}},
                new Product() { Id = 642333, Title = "Bottle of lollies", Cost = 7.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/new/gifts-and-games/Hopscotch pink 3591.jpg" }}},
                new Product() { Id = 818274, Title = "Rainbow Stripe Paper Cups", Cost = 4.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/new/gifts-and-games/Pin the tail on the donkey.jpg" }}},
                new Product() { Id = 798133, Title = "Confetti Party in a Box", Cost = 14.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/new/gifts-and-games/Elastics Red 3587.jpg" }}},
                new Product() { Id = 140289, Title = "Party Confetti", Cost = 8.00M, Categories = new List<Category>() { new Category() { Id = 1, Name = "games" } }, Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/new/gifts-and-games/Matryoshka doll 1234.jpg" }}},

                
                new Product() { Id = 135566, Title = "Red Stripe Vintage Candy Store Bags (set of 10)", Cost = 4.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "Our Vintage Candy Store range features sweet paper cups and bags in nostalgic styles for a look you'll love! \r\n These traditional paper bags are perfect to fill with sweets and treats. \r\n18x9 cms with a 5.5cm gusset.", Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/new/gifts-and-games/Picnic.jpg" }}},
                new Product() { Id = 54353, Title = "Retro Bobble DIY Gumball Machine", Cost = 19.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/new/gifts-and-games/Little red riding hood puppet show 3652.jpg" }}},
                new Product() { Id = 343432, Title = "Lilac Spot Vintage Candy Store Baking Cups (set of 25)", Cost = 4.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/new/party-supplies/wooden cutlery 3653.jpg" }}},
                new Product() { Id = 745445, Title = "Felt Party Crowns", Cost = 6.95M, Description = "", Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/new/party-supplies/bicycle invites 3565.jpg" }}},
                new Product() { Id = 2423424, Title = "Tiger Woodland Mask", Cost = 9.95M, Description = "", Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/new/gifts-and-games/Hand Puppet giraffe 3629.jpg" }}},
                new Product() { Id = 727888, Title = "Fox Woodland Mask", Cost = 9.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/new/gifts-and-games/Pick up sticks 3594.jpg" }}},
                new Product() { Id = 32432, Title = "Bear Woodland Mask", Cost = 9.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "games" } }, Description = "", Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/pm12331_bear-woodland-mask-3.jpg" }}},
                new Product() { Id = 645645, Title = "Little Woodland Toadstool Lights", Cost = 24.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/file_63_9.jpg" }}},
                new Product() { Id = 3426, Title = "10 Paper Party Cups (red dotty)", Cost = 5.95M, Categories = new List<Category>() { new Category() { Id = 1, Name = "party-supplies" } }, Description = "", Images = new List<IProductImage>() { new ProductImage() { Url = "/content/images/products/cup20lores.jpg" }}}
            };
        }
	}
}