using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Admin.Web.Models.Membership;
using Vintage.Rabbit.Admin.Web.Models.Parties;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.QueryHandlers;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Parties.QueryHandlers;

namespace Vintage.Rabbit.Admin.Web.Controllers
{
    public class PartiesController : Controller
    {
        private IQueryDispatcher _queryDispatcher;

        public PartiesController(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        //
        // GET: /Parties/
        public ActionResult Index(Guid? orderGuid)
        {
            PagedResult<Party> parties = this._queryDispatcher.Dispatch<PagedResult<Party>, GetPartiesQuery>(new GetPartiesQuery(1, 100));

            IList<PartyListItemViewModel> viewModel = new List<PartyListItemViewModel>();

            foreach(var party in parties)
            {
                var order = this._queryDispatcher.Dispatch<Order, GetOrderQuery>(new GetOrderQuery(party.OrderGuid));
                var billingAddress = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(order.BillingAddressId.Value));

                var partyViewModel = new PartyListItemViewModel(party, billingAddress.FirstName + " " + billingAddress.LastName);

                if (party.DropoffAddress.HasValue)
                {
                    var address = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(party.DropoffAddress.Value));
                    partyViewModel.DropoffAddress = new DeliveryAddressViewModel(address);
                }

                if (party.PickupAddress.HasValue)
                {
                    var address = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(party.PickupAddress.Value));
                    partyViewModel.PickupAddress = new DeliveryAddressViewModel(address);
                }

                if(orderGuid.HasValue && orderGuid == order.Guid)
                {
                    ViewBag.SelectedParty = orderGuid;
                }

                viewModel.Add(partyViewModel);
            }

            return View("Index", viewModel);
        }

        public ActionResult Party(Guid orderGuid)
        {
            Party party = this._queryDispatcher.Dispatch<Party, GetPartyByOrderGuidQuery>(new GetPartyByOrderGuidQuery(orderGuid));
            var order = this._queryDispatcher.Dispatch<Order, GetOrderQuery>(new GetOrderQuery(party.OrderGuid));
            var billingAddress = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(order.BillingAddressId.Value));

            PartyViewModel viewModel = new PartyViewModel(party, billingAddress.FirstName + " " + billingAddress.LastName);

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

            return this.View("PartyDetails", viewModel);
        }
	}
}