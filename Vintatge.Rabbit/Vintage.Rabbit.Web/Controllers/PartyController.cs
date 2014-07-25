using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Parties.QueryHandlers;
using Vintage.Rabbit.Web.Models.Party;
using Vintage.Rabbit.Web.Models.Payment;

namespace Vintage.Rabbit.Web.Controllers
{
    public class PartyController : Controller
    {
        private IQueryDispatcher _queryDispatcher;

        public PartyController(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public ActionResult PartyHireInformation(Order order)
        {
            if(order.ContainsHireProducts())
            {
                Party party = this._queryDispatcher.Dispatch<Party, GetPartyByOrderGuidQuery>(new GetPartyByOrderGuidQuery(order.Guid));
                if (party != null)
                {
                    PartyViewModel viewModel = new PartyViewModel(party);

                    if(party.DropoffAddress.HasValue)
                    {
                        var address = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(party.DropoffAddress.Value));
                        viewModel.DropoffAddress = new DeliveryAddressViewModel(address, order);
                    }

                    if(party.PickupAddress.HasValue)
                    {
                        var address = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(party.PickupAddress.Value));
                        viewModel.PickupAddress = new DeliveryAddressViewModel(address, order);
                    }

                    return this.PartialView("PartyHireInformation", viewModel);
                }
            }

            return null;
        }
	}
}