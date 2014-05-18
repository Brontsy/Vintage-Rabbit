using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Web.Models.Breadcrumbs;
using Vintage.Rabbit.Web.Models.Buy;
using Vintage.Rabbit.Web.Models.Categories;
using Vintage.Rabbit.Web.Models.Products;

namespace Vintage.Rabbit.Web.Controllers
{
    public class BuyController : Controller
    {
        private IQueryDispatcher _queryDispatcher;

        public BuyController(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public ActionResult Index(int? page)
        {
            IList<Category> categories = this._queryDispatcher.Dispatch<IList<Category>, GetCategoriesQuery>(new GetCategoriesQuery());

            IList<CategoryViewModel> viewModel = categories.Select(o => new CategoryViewModel(o)).ToList();

            return View("Index", viewModel);
        }

        public ActionResult Preview(int productId, string name, string categoryName)
        {
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName));
            BuyProduct product = this._queryDispatcher.Dispatch<BuyProduct, GetBuyProductQuery>(new GetBuyProductQuery(productId));

            BreadcrumbsViewModel breadCrumbs = new BreadcrumbsViewModel();
            breadCrumbs.Add(Url.RouteUrl(Routes.Home), "Home");
            breadCrumbs.Add(Url.RouteUrl(Routes.Buy.Category, new { categoryName = category.Name }), category.DisplayName);
            breadCrumbs.Add(Url.RouteUrl(Routes.Buy.Product, new { productId = product.Id, name = product.Title.ToUrl() }), product.Title, true);


            return this.PartialView("Product", new BuyProductViewModel(product, breadCrumbs));
        }

        public ActionResult Category(string categoryName)
        {
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName));
            IList<BuyProduct> products = this._queryDispatcher.Dispatch<IList<BuyProduct>, GetBuyProductsByCategoryQuery>(new GetBuyProductsByCategoryQuery(category));

            BreadcrumbsViewModel breadCrumbs = new BreadcrumbsViewModel();
            breadCrumbs.Add(Url.RouteUrl(Routes.Home), "Home");
            breadCrumbs.Add(Url.RouteUrl(Routes.Buy.Category, new { categoryName = categoryName }), category.DisplayName);

            BuyProductListViewModel viewModel = new BuyProductListViewModel(products, breadCrumbs);

            return View("ProductList", viewModel);
        }

        public ActionResult Product(int productId, string name, string categoryName)
        {
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName));
            BuyProduct product = this._queryDispatcher.Dispatch<BuyProduct, GetBuyProductQuery>(new GetBuyProductQuery(productId));

            BreadcrumbsViewModel breadCrumbs = new BreadcrumbsViewModel();
            breadCrumbs.Add(Url.RouteUrl(Routes.Home), "Home");
            breadCrumbs.Add(Url.RouteUrl(Routes.Buy.Category, new { categoryName = category.Name }), category.DisplayName);
            breadCrumbs.Add(Url.RouteUrl(Routes.Buy.Product, new { productId = product.Id, name = product.Title.ToUrl() }), product.Title, true);


            return View("Product", new BuyProductViewModel(product, breadCrumbs));
        }
	}
}