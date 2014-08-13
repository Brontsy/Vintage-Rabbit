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
            var themes = this._queryDispatcher.Dispatch<IList<Theme>, GetThemesQuery>(new GetThemesQuery());

            IList<ThemeListItemViewModel> viewModel = themes.Where(o => o.Images.Any()).Select(o => new ThemeListItemViewModel(o)).ToList();

            return View("Index", viewModel);
        }

        public ActionResult Add()
        {
            return this.View("Add", new ThemeViewModel());
        }

        public ActionResult Edit(Guid guid)
        {
            var theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(guid));

            IList<Guid> productGuids = new List<Guid>();

            foreach (var image in theme.Images)
            {
                foreach (var product in image.Products)
                {
                    productGuids.Add(product.ProductGuid);
                }
            }

            var products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByGuidsQuery>(new GetProductsByGuidsQuery(productGuids));

            return this.View("Add", new ThemeViewModel(theme, products));
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
                theme.IncludedItems = viewModel.IncludedItems;
                theme.Cost = viewModel.Cost.Value;

                this._commandDispatcher.Dispatch(new SaveThemeCommand(theme, member));

                return this.RedirectToRoute(Routes.Themes.Edit, new { themeGuid = viewModel.Guid });
            }

            return this.View("Add", viewModel);
        }

        public ActionResult Products(Guid guid)
        {
            var theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(guid));

            IList<Guid> productGuids = new List<Guid>();

            foreach (var image in theme.Images)
            {
                foreach (var product in image.Products)
                {
                    productGuids.Add(product.ProductGuid);
                }
            }

            var products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByGuidsQuery>(new GetProductsByGuidsQuery(productGuids));

            return this.View("Products", new ThemeViewModel(theme, products));
        }

        [HttpGet]
        public ActionResult AddProduct(Guid guid, Guid themeImageGuid)
        {
            Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(guid));
            ThemeImage themeImage = theme.Images.First(o => o.Guid == themeImageGuid);
            IList<Guid> productGuids = themeImage.Products.Select(o => o.ProductGuid).ToList();

            var products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByGuidsQuery>(new GetProductsByGuidsQuery(productGuids));

            AddProductToThemeImageViewModel viewModel = new AddProductToThemeImageViewModel(themeImage, products);


            return this.PartialView("AddProduct", viewModel);
        }

        [HttpGet]
        public ActionResult RemoveProduct(Guid guid, Guid themeImageGuid, Guid themeProductGuid, Member member)
        {
            this._commandDispatcher.Dispatch(new RemoveProductFromThemeCommand(guid, themeImageGuid, themeProductGuid, member));

            return this.RedirectToRoute(Routes.Themes.Products);
        }

        [HttpPost]
        public ActionResult SaveProduct(Guid guid, AddProductToThemeImageViewModel viewModel, Member member)
        {
            if (this.ModelState.IsValid)
            {
                this._commandDispatcher.Dispatch(new AddProductToThemeCommand(guid, viewModel.ThemeImageGuid, viewModel.ThemeProductGuid, viewModel.ProductGuid.Value, viewModel.Qty.Value, viewModel.X.Value, viewModel.Y.Value, member));
            }

            return this.RedirectToRoute(Routes.Themes.Products);
        }
	}
}