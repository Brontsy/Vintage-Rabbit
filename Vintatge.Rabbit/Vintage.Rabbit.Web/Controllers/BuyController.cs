using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Web.Models.Breadcrumbs;
using Vintage.Rabbit.Web.Models.Categories;
using Vintage.Rabbit.Web.Models.Pagination;
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
        public ActionResult PartySuppliesSubnav()
        {
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery("party-supplies"));

            return this.PartialView("PartySuppliesSubnav", category.Children.Select(o => new CategoryViewModel(o)).ToList());
        }

        public ActionResult Preview(int productId, string name, string categoryName)
        {
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName));
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));

            return this.PartialView("Product", new ProductViewModel(product));
        }

        public ActionResult Category(Category category, string childCategoryName, int page = 1)
        {
            bool isChildCategory = false;
            if(!string.IsNullOrEmpty(childCategoryName))
            {
                isChildCategory = true;
                category = category.Children.First(o => o.Name == childCategoryName);
            }

            PagedResult<Product> products = this._queryDispatcher.Dispatch<PagedResult<Product>, GetProductsByCategoryQuery>(new GetProductsByCategoryQuery(category, Common.Enums.ProductType.Buy, page, 20));

            ProductListViewModel viewModel = new ProductListViewModel(products, category);
            viewModel.Pagination = new PaginationViewModel(products.PageNumber, products.TotalResults, products.ItemsPerPage, isChildCategory ? Routes.Buy.CategoryChildPaged : Routes.Buy.CategoryPaged);

            return View("ProductList", viewModel);
        }

        public ActionResult Categories(Category category, string childCategoryName)
        {
            var viewModel = new CategoryViewModel(category);

            if (!string.IsNullOrEmpty(childCategoryName))
            {
                viewModel.Children.First(o => o.Name == childCategoryName).Selected = true;
            }

            return this.PartialView("CategoryList", viewModel);
        }

        public ActionResult Product(int productId, string name, string categoryName)
        {
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName));
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));

            return View("Product", new ProductViewModel(product));
        }

        public ActionResult ListBreadcrumbs(Category category, string childCategoryName)
        {
            BreadcrumbsViewModel breadCrumbs = new BreadcrumbsViewModel();
            breadCrumbs.Add(Url.RouteUrl(Routes.Home), "Home");
            breadCrumbs.Add(Url.RouteUrl(Routes.Buy.Index), "Buy");

            if (category != null)
            {
                breadCrumbs.Add(Url.RouteUrl(Routes.Buy.Category, new { categoryName = category.Name }), category.DisplayName, (string.IsNullOrEmpty(childCategoryName)? true : false));
            }

            if (!string.IsNullOrEmpty(childCategoryName))
            {
                Category childCategory = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(childCategoryName));
                breadCrumbs.Add(Url.RouteUrl(Routes.Buy.Category, new { categoryName = childCategory.Name }), childCategory.DisplayName, true);
            }

            return this.PartialView("Breadcrumbs", breadCrumbs);
        }

        public ActionResult DetailsBreadcrumbs(int productId, Category category)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));

            if(category == null)
            {
                category = product.Categories.First();
            }

            BreadcrumbsViewModel breadCrumbs = new BreadcrumbsViewModel();
            breadCrumbs.Add(Url.RouteUrl(Routes.Home), "Home");
            breadCrumbs.Add(Url.RouteUrl(Routes.Buy.Index), "Buy");
            breadCrumbs.Add(Url.RouteUrl(Routes.Buy.Category, new { categoryName = category.Name }), category.DisplayName);
            breadCrumbs.Add(Url.RouteUrl(Routes.Buy.Product, new { productId = product.Id, name = product.Title.ToUrl() }), product.Title, true);

            return this.PartialView("Breadcrumbs", breadCrumbs);
        }
	}
}