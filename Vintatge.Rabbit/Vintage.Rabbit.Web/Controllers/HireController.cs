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
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.CommandHandlers;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Membership.Entities;

namespace Vintage.Rabbit.Web.Controllers
{
    public class HireController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public HireController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public ActionResult Hire()
        {
            IList<Guid> guids = new List<Guid>()
            {
                new Guid("B95E1BB5-E2A7-4592-A498-354121F16C4C"),
                new Guid("566E3B0B-9353-4551-A423-B0D92B21B840"),
                new Guid("8F2E76D0-01AD-4AFD-8A50-585D145EC670"),
                new Guid("9D782C02-8BD9-497A-BD5D-B2DFE9EBDDFC"),
                new Guid("F3A1D970-08BE-43D3-91B8-927BF9B18D8F"),
                new Guid("16ED5BD7-2554-428B-8265-24A63E0436E4"),
                new Guid("CD6D2690-C199-4C01-9CF1-7C0DA50EA3AE"),
                new Guid("76FD2692-24F7-408B-9B75-0334579FB2AB")
            };

            IList<Product> products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByGuidsQuery>(new GetProductsByGuidsQuery(guids));

            ProductListViewModel viewModel = new ProductListViewModel(products);

            return View("Hire", viewModel);
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
            IList<Category> categories = this._queryDispatcher.Dispatch<IList<Category>, GetCategoriesQuery>(new GetCategoriesQuery()).Where(o => o.ProductType == ProductType.Hire).ToList();

            return this.PartialView("Subnav", categories.Select(o => new CategoryViewModel(o)).ToList());
        }

        public ActionResult Category(string categoryName, int page = 1)
        {
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName, ProductType.Hire));
            PagedResult<Product> products = this._queryDispatcher.Dispatch<PagedResult<Product>, GetProductsByCategoryQuery>(new GetProductsByCategoryQuery(category, Common.Enums.ProductType.Hire, page, 20));

            ProductListViewModel viewModel = new ProductListViewModel(products, category);
            viewModel.Pagination = new PaginationViewModel(products.PageNumber, products.TotalResults, products.ItemsPerPage, Routes.Hire.CategoryPaged);

            return View("Index", viewModel);
        }

        public ActionResult Categories(string categoryName)
        {
            IList<Category> categories = this._queryDispatcher.Dispatch<IList<Category>, GetCategoriesQuery>(new GetCategoriesQuery()).Where(o => o.ProductType == ProductType.Hire).ToList();

            var viewModel = categories.Select(o => new CategoryViewModel(o)).ToList();

            if (!string.IsNullOrEmpty(categoryName))
            {
                viewModel.First(o => o.Name == categoryName).Selected = true;
            }

            return this.PartialView("CategoryList", viewModel);
        }

        public ActionResult Product(int productId)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));

            ProductViewModel viewModel = new ProductViewModel(product);

            if(this.Request.IsAjaxRequest())
            {
                return this.PartialView("Product", viewModel);
            }

            return this.View("Product", viewModel);
        }


        public ActionResult CheckProductAvailability(Member member, Guid productGuid, HireDatesViewModel hireDates)
        {
            if (hireDates.PartyDate.HasValue)
            {
                bool available = this._queryDispatcher.Dispatch<int, GetInventoryCountCanAddToCartQuery>(new GetInventoryCountCanAddToCartQuery(member.Guid, productGuid, hireDates.PartyDate.Value)) > 0;

                return this.Json(new { Available = available }, JsonRequestBehavior.AllowGet);
            }

            return this.Json(new { Available = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AvailabilityCheck(Member member, Cart cart, Guid productGuid, HireDatesViewModel hireDates, HireAvailabilityViewModel hireAvailability, bool postcodeChecked = false)
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
                int availableInventory = this._queryDispatcher.Dispatch<int, GetInventoryCountCanAddToCartQuery>(new GetInventoryCountCanAddToCartQuery(member.Guid, productGuid, hireDates.PartyDate.Value));
                bool inCart = cart.Items.Any(o => o.Product.Guid == productGuid);

                return this.PartialView("AddToCart", new HireAddProductToCartViewModel(product, availableInventory, inCart, hireDates.PartyDate.Value));
            }

            ViewBag.PostcodeChecked = postcodeChecked;

            return this.PartialView("AvailabilityCheck", new HireProductViewModel(product, hireDates));
        }

        public ActionResult ListBreadcrumbs(string categoryName)
        {
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName, ProductType.Hire));

            BreadcrumbsViewModel breadCrumbs = new BreadcrumbsViewModel();
            breadCrumbs.Add(Url.RouteUrl(Routes.Home), "Home");
            breadCrumbs.Add(Url.RouteUrl(Routes.Hire.Index), "Hire");

            if (category != null)
            {
                breadCrumbs.Add(Url.RouteUrl(Routes.Hire.Category, new { categoryName = category.Name }), category.DisplayName, true);
            }

            return this.PartialView("Breadcrumbs", breadCrumbs);
        }

        public ActionResult DetailsBreadcrumbs(int productId, string categoryName)
        {
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName, ProductType.Hire));
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

        public ActionResult ChangePartyDate(HireDatesViewModel partyDate, Cart cart)
        {
            // get unavailable products
            // submit command handler
            if (partyDate.PartyDate.HasValue)
            {
                IList<CartItem> cartItems = this._queryDispatcher.Dispatch<IList<CartItem>, GetUnavailableCartItemsQuery>(new GetUnavailableCartItemsQuery(cart, partyDate.PartyDate.Value));

                if (cartItems.Any())
                {
                    foreach (CartItem cartItem in cartItems)
                    {
                        this._commandDispatcher.Dispatch(new RemoveCartItemCommand(cart.MemberId, cartItem.Id));
                    }

                    IList<PurchasableItemViewModel> viewModel = cartItems.Select(o => new PurchasableItemViewModel(o.Product)).ToList();

                    return this.View("ProductsRemoved", viewModel);
                }
            }

            return this.Redirect(this.Request.UrlReferrer.ToString());
        }
	}
}