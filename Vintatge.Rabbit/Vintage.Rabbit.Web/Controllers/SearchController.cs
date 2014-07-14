using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Search.CommandHandlers;
using Vintage.Rabbit.Search.QueryHandlers;
using Vintage.Rabbit.Web.Models.Products;

namespace Vintage.Rabbit.Web.Controllers
{
    public class SearchController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public SearchController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher; 
            this._commandDispatcher = commandDispatcher;
        }

        public ActionResult Index(string query)
        {
            PagedResult<Product> products = this._queryDispatcher.Dispatch<PagedResult<Product>, SearchQuery>(new SearchQuery(query));

            ProductListViewModel viewModel = new ProductListViewModel(products);

            return View("Index", viewModel);
        }

        public ActionResult RebuildIndex()
        {
            this._commandDispatcher.Dispatch(new BuildLuceneIndexCommand());

            return this.RedirectToRoute(Routes.Search.Index);
        }
	}
}