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
using Vintage.Rabbit.Web.Models.Themes;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Carts.CommandHandlers;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Carts.QueryHandlers;

namespace Vintage.Rabbit.Web.Controllers
{
    public class StyleController : Controller
    {        
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public StyleController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
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

        public ActionResult Hire(string themeName, Models.Hire.HireDatesViewModel hireDates, Models.Hire.HireAvailabilityViewModel hireAvailability, Member member, bool postcodeChecked = false, bool changePostcode = false)
        {
            Theme theme = this._queryDispatcher.Dispatch<IList<Theme>, GetThemesQuery>(new GetThemesQuery()).First(o => o.Title.ToUrl() == themeName);

            if (!hireAvailability.IsValidPostcode)
            {
                if (string.IsNullOrEmpty(hireAvailability.Postcode))
                {
                    return this.PartialView("PostcodeCheck", new PostcodeCheckViewModel(theme));
                }

                return this.PartialView("HireUnavailable");
            }

            if (!hireDates.PartyDate.HasValue || changePostcode)
            {
                return this.PartialView("AvailabilityCheck", new PartyDatePickerViewModel(theme));
            }
            else
            {
                var products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByGuidsQuery>(new GetProductsByGuidsQuery(this.GetProductGuids(theme)));
                ThemeViewModel viewModel = new ThemeViewModel(theme, products);

                if (this._queryDispatcher.Dispatch<bool, CanAddThemeToCartQuery>(new CanAddThemeToCartQuery(member.Guid, theme.Guid, hireDates.PartyDate.Value)))
                {
                    ViewBag.PartyDate = hireDates.PartyDate.Value;

                    return this.PartialView("AddToCart", viewModel);
                }

                return this.PartialView("Unavailable", viewModel);

            }
        }
        

        public ActionResult CustomStyling()
        {
            return this.View("CustomStyling");
        }

        private IList<Guid> GetProductGuids(Theme theme)
        {
            IList<Guid> productGuids = new List<Guid>();

            foreach (var image in theme.Images)
            {
                foreach (var product in image.Products)
                {
                    productGuids.Add(product.ProductGuid);
                }
            }

            return productGuids;
        }



        public ActionResult ThemeLink(Guid themeGuid, bool includeHostName = false)
        {
            Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(themeGuid));

            return this.PartialView("Link", new ThemeLinkViewModel(theme, includeHostName));
        }

        public ActionResult ThemePreviewLink(Guid themeGuid)
        {
            Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(themeGuid));

            return this.PartialView("PreviewLink", new ThemeLinkViewModel(theme, false));
        }
	}
}