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
	}
}