using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Competitions.CommandHandlers;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Web.Models.Competitions;

namespace Vintage.Rabbit.Web.Controllers
{
    public class CompetitionsController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public CompetitionsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public ActionResult Index(bool saved = false)
        {
            ViewBag.Saved = saved;

            return View("Index", new CompetitionEntryViewModel());
        }

        public ActionResult SaveEntry(CompetitionEntryViewModel entry)
        {
            if(this.ModelState.IsValid)
            {
                this._commandDispatcher.Dispatch(new SaveCompetitionEntryCommand("Favourite childhood birthday memory", entry.Name, entry.DateOfBirth.Value, entry.Email, entry.PhoneNumber, entry.EntryText));
            }

            return this.RedirectToRoute(Routes.Competition.Index, new { saved = true });
        }
	}
}