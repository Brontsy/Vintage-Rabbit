using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Orders.CommandHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.QueryHandlers;
using Vintage.Rabbit.Web.Models.Orders;

namespace Vintage.Rabbit.Web.Controllers
{
    public class OrderController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public OrderController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher; 
            this._commandDispatcher = commandDispatcher;
        }

        public ActionResult Summary(Order order)
        {
            OrderViewModel viewModel = new OrderViewModel(order);

            return this.PartialView("Summary", viewModel);
        }

        public ActionResult ApplyDiscount(Order order, string number)
        {
            LoyaltyCard loyaltyCard = this._queryDispatcher.Dispatch<LoyaltyCard, GetLoyaltyCardQuery>(new GetLoyaltyCardQuery(number));

            if (loyaltyCard != null)
            {
                if (loyaltyCard.Status == Orders.Enums.LoyaltyCardStatus.Available)
                {
                    this._commandDispatcher.Dispatch<ApplyDiscountCommand>(new ApplyDiscountCommand(order, loyaltyCard));
                    return this.Json(new { Status = "Applied" });
                }

                if (loyaltyCard.Status == Orders.Enums.LoyaltyCardStatus.Expired)
                {
                    return this.Json(new { Status = "Expired" });
                }

                if (loyaltyCard.Status == Orders.Enums.LoyaltyCardStatus.Used)
                {
                    return this.Json(new { Status = "Used" });
                }
            }

            return this.Json(new { Status = "Not Found" });
        }
	}
}