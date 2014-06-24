using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Web.Models.Products;
using Vintage.Rabbit.Web.Models.Themes;

namespace Vintage.Rabbit.Web.Controllers
{
    public class StyleController : Controller
    {        
        private IQueryDispatcher _queryDispatcher;

        public StyleController(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult WizardOfOz()
        {
            return View();
        }

        public ActionResult Carnival()
        {
            return View("Carnival", new ThemeViewModel());
        }

        public ActionResult Product(string name, int productId)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));

            if (product.Type == Common.Enums.ProductType.Buy)
            {
                return this.PartialView("BuyProduct", new ProductViewModel(product));
            }

            return this.PartialView("HireProduct", new ProductViewModel(product));
        }
	}
}