using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Admin.Web.Models.LoyaltyCards;
using Vintage.Rabbit.Admin.Web.Models.Orders;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;
using Vintage.Rabbit.Orders.CommandHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.Enums;
using Vintage.Rabbit.Orders.QueryHandlers;

namespace Vintage.Rabbit.Admin.Web.Controllers
{
    public class LoyaltyCardsController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public LoyaltyCardsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }
        
        public ActionResult Index()
        {
            IList<LoyaltyCard> loyaltyCards = this._queryDispatcher.Dispatch<IList<LoyaltyCard>, GetLoyaltyCardsQuery>(new GetLoyaltyCardsQuery());

            IList<LoyaltyCardViewModel> viewModel = loyaltyCards.Select(o => new LoyaltyCardViewModel(o)).ToList();

            return this.View("Index", viewModel);
        }

        public ActionResult Add()
        {
            LoyaltyCardViewModel viewModel = new LoyaltyCardViewModel();

            return this.View("Edit", viewModel);
        }

        public ActionResult View(Guid guid)
        {
            LoyaltyCard loyaltyCard = this._queryDispatcher.Dispatch<LoyaltyCard, GetLoyaltyCardByGuidQuery>(new GetLoyaltyCardByGuidQuery(guid));

            return this.View("View", new LoyaltyCardViewModel(loyaltyCard));
        }

        public ActionResult Edit(Guid guid, bool saved = false)
        {
            LoyaltyCard loyaltyCard = this._queryDispatcher.Dispatch<LoyaltyCard, GetLoyaltyCardByGuidQuery>(new GetLoyaltyCardByGuidQuery(guid));
            ViewBag.Saved = saved;

            return this.View("Edit", new LoyaltyCardViewModel(loyaltyCard));
        }

        public ActionResult Delete(Guid guid)
        {
            this._commandDispatcher.Dispatch(new DeleteLoyaltyCardCommand(guid));

            return this.RedirectToRoute(Routes.LoyaltyCards.Index);
        }

        public ActionResult Save(LoyaltyCardViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                LoyaltyCard loyaltyCard = this._queryDispatcher.Dispatch<LoyaltyCard, GetLoyaltyCardByGuidQuery>(new GetLoyaltyCardByGuidQuery(viewModel.Guid));

                if (loyaltyCard == null)
                {
                    loyaltyCard = new LoyaltyCard(viewModel.Guid);
                }

                loyaltyCard.Number = viewModel.Number;
                loyaltyCard.Discount = viewModel.Discount.Value;
                loyaltyCard.LoyaltyCardType = viewModel.LoyaltyCardType.Value;
                loyaltyCard.Title = viewModel.Title;

                this._commandDispatcher.Dispatch(new SaveLoyaltyCardCommand(loyaltyCard));

                return this.RedirectToRoute(Routes.LoyaltyCards.Edit, new { guid = viewModel.Guid, saved = true });
            }

            return this.RedirectToRoute(Routes.LoyaltyCards.Edit, new { guid = viewModel.Guid });
        }
    }
}