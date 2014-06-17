using System;
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

namespace Vintage.Rabbit.Web.Controllers
{
    public class HireController : Controller
    {
        private IQueryDispatcher _queryDispatcher;

        public HireController(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public ActionResult Index()
        {
            IList<Product> products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByTypeQuery>(new GetProductsByTypeQuery(ProductType.Hire));

            BreadcrumbsViewModel breadCrumbs = new BreadcrumbsViewModel();
            breadCrumbs.Add(Url.RouteUrl(Routes.Home), "Home");
            breadCrumbs.Add(Url.RouteUrl(Routes.Hire.Index), "Hire");

            ProductListViewModel viewModel = new ProductListViewModel(products);

            return View("Index", viewModel);
        }

        public ActionResult Category(string categoryName)
        {
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName));
            IList<Product> products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByCategoryQuery>(new GetProductsByCategoryQuery(category, Common.Enums.ProductType.Hire));

            BreadcrumbsViewModel breadCrumbs = new BreadcrumbsViewModel();
            breadCrumbs.Add(Url.RouteUrl(Routes.Home), "Home");
            breadCrumbs.Add(Url.RouteUrl(Routes.Hire.Index), "Hire");
            breadCrumbs.Add(Url.RouteUrl(Routes.Hire.Category, new { categoryName = categoryName }), category.DisplayName);

            ProductListViewModel viewModel = new ProductListViewModel(products);

            return View("Index", viewModel);
        }

        public ActionResult Categories(string categoryName)
        {
            IList<Category> categories = this._queryDispatcher.Dispatch<IList<Category>, GetCategoriesQuery>(new GetCategoriesQuery()).Where(o => o.ProductTypes.Contains(ProductType.Hire)).ToList();

            return this.PartialView("CategoryList", categories.Select(o => new CategoryViewModel(o)).ToList());
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
            if (hireDates.StartDate.HasValue && hireDates.EndDate.HasValue)
            {
                bool available = this._queryDispatcher.Dispatch<bool, IsProductAvailableForHireQuery>(new IsProductAvailableForHireQuery(productGuid, 1, hireDates.StartDate.Value, hireDates.EndDate.Value));

                return this.Json(new { Available = available }, JsonRequestBehavior.AllowGet);
            }

            return this.Json(new { Available = false }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult AvailabilityCheck(int productId, HireDatesViewModel hireDates)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));
            bool? available = null;

            if (hireDates.StartDate.HasValue && hireDates.EndDate.HasValue)
            {
                available = this._queryDispatcher.Dispatch<bool, IsProductAvailableForHireQuery>(new IsProductAvailableForHireQuery(product.Guid, 1, hireDates.StartDate.Value, hireDates.EndDate.Value));
            }

            AvailabilityCheckViewModel viewModel = new AvailabilityCheckViewModel(new ProductViewModel(product), available, hireDates);

            return this.PartialView("AvailabilityCheck", viewModel);
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
	}
}