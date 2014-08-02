﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Common.Enums;
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
        public ActionResult Index(int page = 1)
        {
            PagedResult<Product> products = this._queryDispatcher.Dispatch<PagedResult<Product>, GetProductsByTypeQuery>(new GetProductsByTypeQuery(ProductType.Buy, page, 20));

            ProductListViewModel viewModel = new ProductListViewModel(products);
            viewModel.Pagination = new PaginationViewModel(products.PageNumber, products.TotalResults, products.ItemsPerPage, Routes.Buy.IndexPaged);

            return View("Index", viewModel);
        }

        public ActionResult PartySupplies()
        {
            IList<Guid> guids = new List<Guid>()
            {
                new Guid("F239E8BF-46FE-4024-8583-23D8C9AEFB36"),
                new Guid("BFF8602A-5BD7-4974-A887-4D87823EC823"),
                new Guid("661A7261-7037-4E1F-8364-CC62F12401D2"),
                new Guid("34B46ABB-98FB-4E40-8EBC-30034419181F"),
                new Guid("87F6D8B3-ABF6-4E6E-81B2-780A3BB7F9DC"),
                new Guid("E51F547D-1471-4F95-84E1-907E8FABE50D"),
                new Guid("62079AE2-40EF-4A81-B1FA-67C5EDDBFE23"),
                new Guid("1628D929-14B4-4CEF-8456-C263BC3CD1EB"),
            };

            IList<Product> products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByGuidsQuery>(new GetProductsByGuidsQuery(guids));

            ProductListViewModel viewModel = new ProductListViewModel(products);

            return View("PartySupplies", viewModel);
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

            return View("Index", viewModel);
        }

        public ActionResult Categories(Category category, string childCategoryName)
        {
            IList<Category> categories = this._queryDispatcher.Dispatch<IList<Category>, GetCategoriesQuery>(new GetCategoriesQuery()).Where(o => o.ProductTypes.Contains(ProductType.Buy)).ToList();

            var viewModel = categories.Select(o => new CategoryViewModel(o)).ToList();

            if (!string.IsNullOrEmpty(childCategoryName))
            {
                var parent = viewModel.First(o => o.Name == category.Name);

                parent.Children.First(x => x.Name == childCategoryName).Selected = true;
            }
            else if (category != null)
            {
                viewModel.First(o => o.Name == category.Name).Selected = true;
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