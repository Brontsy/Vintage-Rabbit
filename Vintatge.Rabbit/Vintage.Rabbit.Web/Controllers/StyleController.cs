using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Themes.QueryHandlers;
using Vintage.Rabbit.Web.Models.Products;
using Vintage.Rabbit.Web.Models.Themes;
using Vintage.Rabbit.Common.Extensions;

namespace Vintage.Rabbit.Web.Controllers
{
    public class StyleController : Controller
    {        
        private IQueryDispatcher _queryDispatcher;

        public StyleController(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public ActionResult Hire()
        {
            return PartialView("Hire");
        }

        public ActionResult Index()
        {
            var themes = this._queryDispatcher.Dispatch<IList<Theme>, GetThemesQuery>(new GetThemesQuery());

            IList<ThemeListItemViewModel> viewModel = themes.Where(o => o.Images.Any()).Select(o => new ThemeListItemViewModel(o)).ToList();

            return View("Index", viewModel);
        }

        public ActionResult Theme(string themeName)
        {
            var theme = this._queryDispatcher.Dispatch<IList<Theme>, GetThemesQuery>(new GetThemesQuery()).First(o => o.Title.ToUrl() == themeName);

            IList<Guid> productGuids = new List<Guid>();
            
            foreach(var image in theme.Images)
            {
                foreach(var product in image.Products)
                {
                    productGuids.Add(product.ProductGuid);
                }
            }
            
            var products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByGuidsQuery>(new GetProductsByGuidsQuery(productGuids));

            return this.View("Theme", new ThemeViewModel(theme, products));
        }

        public ActionResult Image(string themeName, Guid imageGuid)
        {
            var theme = this._queryDispatcher.Dispatch<IList<Theme>, GetThemesQuery>(new GetThemesQuery()).First(o => o.Title.ToUrl() == themeName);

            IList<Guid> productGuids = new List<Guid>();

            foreach (var image in theme.Images)
            {
                foreach (var product in image.Products)
                {
                    productGuids.Add(product.ProductGuid);
                }
            }
            
            var products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByGuidsQuery>(new GetProductsByGuidsQuery(productGuids));

            ThemeImageViewModel viewModel = new ThemeImageViewModel(theme.Images.First(o => o.Guid == imageGuid), products);

            return this.PartialView("Image", viewModel);
        }


        public ActionResult WizardOfOz()
        {
            return View();
        }
        
        public ActionResult Carnival()
        {
            return View("Carnival");//, new ThemeViewModel());
        }

        public ActionResult TeddyBearsPicnic()
        {
            return View("TeddyBearsPicnic");//, new ThemeViewModel());
        }

        public ActionResult CustomStyling()
        {
            return this.View("CustomStyling");
        }

        public ActionResult Product(string name, int productId)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));

            if (product.Type == Common.Enums.ProductType.Buy)
            {
                return this.PartialView("BuyProduct", new ProductViewModel(product));
            }

            return this.PartialView("HireProduct", new ProductViewModel(product));
        }
	}
}