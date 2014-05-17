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
            IList<HireProduct> products = this._queryDispatcher.Dispatch<IList<HireProduct>, GetHireProductsQuery>(new GetHireProductsQuery());

            BreadcrumbsViewModel breadCrumbs = new BreadcrumbsViewModel();
            breadCrumbs.Add(Url.RouteUrl(Routes.Home), "Home");
            breadCrumbs.Add(Url.RouteUrl(Routes.Hire.Index), "Hire");

            HireProductListViewModel viewModel = new HireProductListViewModel(products, breadCrumbs);

            return View("Index", viewModel);
        }
        public ActionResult Product(int productId, string name, HireDatesViewModel hireDates)
        {
            HireProduct product = this._queryDispatcher.Dispatch<HireProduct, GetHireProductQuery>(new GetHireProductQuery(productId));
            bool? available = null;

            if(hireDates.StartDate.HasValue && hireDates.EndDate.HasValue)
            {
                available = this._queryDispatcher.Dispatch<bool, IsHireProductAvailableQuery>(new IsHireProductAvailableQuery(productId, hireDates.StartDate.Value, hireDates.EndDate.Value));
            }

            BreadcrumbsViewModel breadCrumbs = new BreadcrumbsViewModel();
            breadCrumbs.Add(Url.RouteUrl(Routes.Home), "Home");
            breadCrumbs.Add(Url.RouteUrl(Routes.Hire.Index), "Hire");
            breadCrumbs.Add(Url.RouteUrl(Routes.Hire.Product, new { productId = product.Id, name = product.Title.ToUrl() }), product.Title, true);

            return View("Product", new HireProductViewModel(product, available, hireDates, breadCrumbs));
        }

        public ActionResult CheckProductAvailability(int productId, HireDatesViewModel hireDates)
        {
            if (hireDates.StartDate.HasValue && hireDates.EndDate.HasValue)
            {
                bool available = this._queryDispatcher.Dispatch<bool, IsHireProductAvailableQuery>(new IsHireProductAvailableQuery(productId, hireDates.StartDate.Value, hireDates.EndDate.Value));

                return this.Json(new { Available = available }, JsonRequestBehavior.AllowGet);
            }

            return this.Json(new { Available = false }, JsonRequestBehavior.AllowGet);
        }
	}
}