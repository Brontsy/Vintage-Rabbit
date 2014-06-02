using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Admin.Web.Models.Buy;
using Vintage.Rabbit.Admin.Web.Models.Hire;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;

namespace Vintage.Rabbit.Admin.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IQueryDispatcher _queryDispatcher;

        public ProductsController(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public ActionResult BuyList()
        {
            IList<BuyProduct> products = this._queryDispatcher.Dispatch<IList<BuyProduct>, GetBuyProductsQuery>(new GetBuyProductsQuery(1)).OrderBy(o => o.Title).ToList();

            BuyProductListViewModel viewModel = new BuyProductListViewModel(products);

            return View("BuyList", viewModel);
        }
        public ActionResult HireList()
        {
            IList<HireProduct> products = this._queryDispatcher.Dispatch<IList<HireProduct>, GetHireProductsQuery>(new GetHireProductsQuery()).OrderBy(o => o.Title).ToList();

            HireProductListViewModel viewModel = new HireProductListViewModel(products);

            return View("HireList", viewModel);
        }
	}
}