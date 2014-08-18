using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.QueryHandlers;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Parties.QueryHandlers;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Web.Areas.Emails.Models.InvoiceEmail;
using Vintage.Rabbit.Web.Models.Payment;

namespace Vintage.Rabbit.Web.Areas.Emails.Controllers
{
    public class InvoiceEmailController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private string _websiteUrl;

        public InvoiceEmailController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            this._websiteUrl = System.Configuration.ConfigurationManager.AppSettings["Website_Url"];
        }

        public ActionResult Index(Guid orderGuid, int orderId)
        {
            Order order = this._queryDispatcher.Dispatch<Order, GetOrderQuery>(new GetOrderQuery(orderGuid));

            if (order == null || order.Id != orderId)
            {
                throw new Exception("Order Id and Order Guid do not match");
            }

            InvoiceEmailViewModel viewModel = new InvoiceEmailViewModel(order);
            ViewBag.WebsiteUrl = this._websiteUrl;

            return View("InvoiceEmail", viewModel);
        }

        public ActionResult PartyHireInformation(Guid orderGuid)
        {
            Order order = this._queryDispatcher.Dispatch<Order, GetOrderQuery>(new GetOrderQuery(orderGuid));

            if (order.ContainsHireProducts() || order.ContainsTheme())
            {
                Party party = this._queryDispatcher.Dispatch<Party, GetPartyByOrderGuidQuery>(new GetPartyByOrderGuidQuery(order.Guid));
                if (party != null)
                {
                    PartyViewModel viewModel = new PartyViewModel(party);

                    if (party.DropoffAddress.HasValue)
                    {
                        var address = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(party.DropoffAddress.Value));
                        viewModel.DropoffAddress = new DeliveryAddressViewModel(address);
                    }

                    if (party.PickupAddress.HasValue)
                    {
                        var address = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(party.PickupAddress.Value));
                        viewModel.PickupAddress = new DeliveryAddressViewModel(address);
                    }

                    return this.PartialView("PartyHireInformation", viewModel);
                }
            }

            return null;
        }
	}
}