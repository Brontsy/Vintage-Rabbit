using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Admin.Web.Models.Themes;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
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
	}
}