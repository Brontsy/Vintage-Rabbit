using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Admin.Web.Models.Products;
using Vintage.Rabbit.Admin.Web.Models.Themes;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Themes.CommandHandlers;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Themes.QueryHandlers;

namespace Vintage.Rabbit.Admin.Web.Controllers
{
    public class ThemesController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public ThemesController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public ActionResult Index()
        {
            IList<Theme> themes = this._queryDispatcher.Dispatch<IList<Theme>, GetThemesQuery>(new GetThemesQuery());
            ThemesViewModel viewModel = new ThemesViewModel(themes);

            return View("Index", viewModel);
        }

        public ActionResult Add()
        {
            return this.View("Add", new ThemeViewModel());
        }

        public ActionResult Edit(Guid guid)
        {
            Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(guid));

            return this.View("Add", new ThemeViewModel(theme));
        }

        public ActionResult Save(ThemeViewModel viewModel, Member member)
        {
            if(this.ModelState.IsValid)
            {
                Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(viewModel.Guid));

                if(theme==null)
                {
                    theme = new Theme(viewModel.Guid);
                }

                theme.Title = viewModel.Title;
                theme.Description = viewModel.Description;
                theme.Cost = viewModel.Cost.Value;

                this._commandDispatcher.Dispatch(new SaveThemeCommand(theme, member));

                return this.RedirectToRoute(Routes.Themes.Edit, new { themeGuid = viewModel.Guid });
            }

            return this.View("Add", viewModel);
        }

        public ActionResult Products(Guid guid)
        {
            Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(guid));
            ThemeViewModel viewModel = new ThemeViewModel(theme);

            return this.View("Products", viewModel);
        }

        [HttpGet]
        public ActionResult AddProduct(Guid guid)
        {
            Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(guid));

            ViewBag.MainImageUrl = theme.MainImage.Url;

            return this.PartialView("AddProduct", new ThemeProductViewModel(new ThemeProduct()));
        }

        [HttpGet]
        public ActionResult EditProduct(Guid guid, Guid themeProductGuid)
        {
            Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(guid));
            var product = theme.Products.FirstOrDefault(o => o.Guid == themeProductGuid);

            if(product != null)
            {
                var p = this._queryDispatcher.Dispatch<Product, GetProductByGuidQuery>(new GetProductByGuidQuery(product.ProductGuid));
                ViewBag.Search = p.Title;
            }

            return this.PartialView("AddProduct", new ThemeProductViewModel(product));
        }

        [HttpPost]
        public ActionResult SaveProduct(Guid guid, ThemeProductViewModel viewModel, Member member)
        {
            this._commandDispatcher.Dispatch(new AddProductToThemeCommand(guid, viewModel.ThemeProductGuid, viewModel.ProductGuid, viewModel.X.Value, viewModel.Y.Value, member));

            return this.RedirectToRoute(Routes.Themes.Products);
        }

        public ActionResult ProductList(Guid guid)
        {
            Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(guid));

            PagedResult<Product> products = this._queryDispatcher.Dispatch<PagedResult<Product>, GetProductsQuery>(new GetProductsQuery(1, 5000));

            IList<ThemeProductListItemViewModel> viewModel = new List<ThemeProductListItemViewModel>();

            foreach(var themeProduct in theme.Products)
            {
                var product = products.FirstOrDefault(o => o.Guid == themeProduct.ProductGuid);
                if(product != null)
                {
                    viewModel.Add(new ThemeProductListItemViewModel(themeProduct, product));
                }
            }

            return this.PartialView("ProductList", viewModel);
        }

        public ActionResult MainImage(Guid guid, Guid themeProductGuid)
        {
            Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(guid));

            ThemeMainImageViewModel viewModel = new ThemeMainImageViewModel(theme, themeProductGuid);

            return this.PartialView("MainImage", viewModel);
        }
	}
}