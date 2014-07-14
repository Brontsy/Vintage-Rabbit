﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Web.Models.Breadcrumbs;
using Vintage.Rabbit.Web.Models.Categories;
using Vintage.Rabbit.Web.Models.Hire;
using Vintage.Rabbit.Web.Models.Products;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Web.Models.Pagination;

namespace Vintage.Rabbit.Web.Controllers
{
    public class HireController : Controller
    {
        private IQueryDispatcher _queryDispatcher;

        public HireController(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public ActionResult Index(int page = 1)
        {
            PagedResult<Product> products = this._queryDispatcher.Dispatch<PagedResult<Product>, GetProductsByTypeQuery>(new GetProductsByTypeQuery(ProductType.Hire, page, 20));

            ProductListViewModel viewModel = new ProductListViewModel(products);
            viewModel.Pagination = new PaginationViewModel(products.PageNumber, products.TotalResults, products.ItemsPerPage, Routes.Hire.IndexPaged);

            return View("Index", viewModel);
        }

        public ActionResult Subnav()
        {
            IList<Category> categories = this._queryDispatcher.Dispatch<IList<Category>, GetCategoriesQuery>(new GetCategoriesQuery()).Where(o => o.ProductTypes.Contains(ProductType.Hire)).ToList();

            return this.PartialView("Subnav", categories.Select(o => new CategoryViewModel(o)).ToList());
        }

        public ActionResult Category(string categoryName, int page = 1)
        {
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName));
            PagedResult<Product> products = this._queryDispatcher.Dispatch<PagedResult<Product>, GetProductsByCategoryQuery>(new GetProductsByCategoryQuery(category, Common.Enums.ProductType.Hire, page, 20));

            ProductListViewModel viewModel = new ProductListViewModel(products, category);
            viewModel.Pagination = new PaginationViewModel(products.PageNumber, products.TotalResults, products.ItemsPerPage, Routes.Hire.CategoryPaged);

            return View("Index", viewModel);
        }

        public ActionResult Categories(string categoryName)
        {
            IList<Category> categories = this._queryDispatcher.Dispatch<IList<Category>, GetCategoriesQuery>(new GetCategoriesQuery()).Where(o => o.ProductTypes.Contains(ProductType.Hire)).ToList();

            var viewModel = categories.Select(o => new CategoryViewModel(o)).ToList();

            if (!string.IsNullOrEmpty(categoryName))
            {
                viewModel.First(o => o.Name == categoryName).Selected = true;
            }

            return this.PartialView("CategoryList", viewModel);
        }

        public ActionResult Preview(int productId, string name)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));

            return this.PartialView("Product", new ProductViewModel(product));
        }

        public ActionResult Product(int productId, string name)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));

            return this.View("Product", new ProductViewModel(product));
        }

        public ActionResult CheckProductAvailability(Guid productGuid, HireDatesViewModel hireDates)
        {
            if (hireDates.PartyDate.HasValue)
            {
                bool available = this._queryDispatcher.Dispatch<bool, IsProductAvailableForHireQuery>(new IsProductAvailableForHireQuery(productGuid, 1, this.GetHireStartDate(hireDates.PartyDate.Value), this.GetHireEndDate(hireDates.PartyDate.Value)));

                return this.Json(new { Available = available }, JsonRequestBehavior.AllowGet);
            }

            return this.Json(new { Available = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AvailabilityCheck(Guid productGuid, HireDatesViewModel hireDates, HireAvailabilityViewModel hireAvailability, bool postcodeChecked = false)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByGuidQuery>(new GetProductByGuidQuery(productGuid));

            if (!hireAvailability.IsValidPostcode)
            {
                if (string.IsNullOrEmpty(hireAvailability.Postcode))
                {
                    return this.PartialView("PostcodeCheck", new PostcodeCheckViewModel(product));
                }

                return this.PartialView("HireUnavailable");
            }

            if (hireDates.PartyDate.HasValue) 
            {
                bool available = this._queryDispatcher.Dispatch<bool, IsProductAvailableForHireQuery>(new IsProductAvailableForHireQuery(product.Guid, 1, this.GetHireStartDate(hireDates.PartyDate.Value), this.GetHireEndDate(hireDates.PartyDate.Value)));

                AvailabilityCheckViewModel viewModel = new AvailabilityCheckViewModel(new ProductViewModel(product), available, hireDates);

                if(available)
                {
                    return this.PartialView("AddToCart", viewModel);
                }
            }

            ViewBag.PostcodeChecked = postcodeChecked;

            return this.PartialView("AvailabilityCheck", new AvailabilityCheckViewModel(new ProductViewModel(product), null, hireDates));
        }

        public ActionResult ListBreadcrumbs(Category category)
        {
            BreadcrumbsViewModel breadCrumbs = new BreadcrumbsViewModel();
            breadCrumbs.Add(Url.RouteUrl(Routes.Home), "Home");
            breadCrumbs.Add(Url.RouteUrl(Routes.Hire.Index), "Hire");

            if (category != null)
            {
                breadCrumbs.Add(Url.RouteUrl(Routes.Hire.Category, new { categoryName = category.Name }), category.DisplayName, true);
            }

            return this.PartialView("Breadcrumbs", breadCrumbs);
        }

        public ActionResult DetailsBreadcrumbs(int productId, Category category)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));

            if (category == null)
            {
                category = product.Categories.First();
            }

            BreadcrumbsViewModel breadCrumbs = new BreadcrumbsViewModel();
            breadCrumbs.Add(Url.RouteUrl(Routes.Home), "Home");
            breadCrumbs.Add(Url.RouteUrl(Routes.Hire.Index), "Hire");
            breadCrumbs.Add(Url.RouteUrl(Routes.Hire.Category, new { categoryName = category.Name }), category.DisplayName);
            breadCrumbs.Add(Url.RouteUrl(Routes.Hire.Product, new { productId = product.Id, name = product.Title.ToUrl() }), product.Title, true);

            return this.PartialView("Breadcrumbs", breadCrumbs);
        }


        private DateTime GetHireStartDate(DateTime partyDate)
        {
            DateTime date = partyDate;
            while (date.DayOfWeek != DayOfWeek.Friday)
            {
                date = date.AddDays(-1);
            }

            return date;
        }
        private DateTime GetHireEndDate(DateTime partyDate)
        {
            DateTime date = partyDate;
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(1);
            }

            return date;
        }
	}
}