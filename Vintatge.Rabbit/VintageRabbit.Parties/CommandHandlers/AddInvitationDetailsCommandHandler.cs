using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Membership;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Parties.QueryHandlers;
using Vintage.Rabbit.Parties.Repositories;

namespace Vintage.Rabbit.Parties.CommandHandlers
{
    public class AddInvitationDetailsCommand
    {
        public IOrder Order { get; private set; }

        public DateTime PartyDate { get; private set; }

        public IActionBy ActionBy { get; private set; }

        public string ChildsName { get; private set; }

        public string Age { get; private set; }

        public string PartyTime { get; private set; }

        public string PartyAddress { get; private set; }

        public string RSVPDetails { get; private set; }

        public AddInvitationDetailsCommand(IOrder order, DateTime partyDate, string childsName, string age, string partyTime, string partyAddress, string rsvpDetails, IActionBy actionBy)
        {
            this.Order = order;
            this.PartyDate = partyDate;
            this.ChildsName = childsName;
            this.Age = age;
            this.PartyTime = partyTime;
            this.PartyAddress = partyAddress;
            this.RSVPDetails = rsvpDetails;
            this.ActionBy = actionBy;
        }
    }

    internal class AddInvitationDetailsCommandHandler : ICommandHandler<AddInvitationDetailsCommand>
    {
        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;

        public AddInvitationDetailsCommandHandler(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(AddInvitationDetailsCommand command)
        {
            Party party = this._queryDispatcher.Dispatch<Party, GetPartyByOrderGuidQuery>(new GetPartyByOrderGuidQuery(command.Order.Guid));

            if (party == null)
            {
                party = new Party(command.Order, command.PartyDate);
            }

            party.ChildsName = command.ChildsName;
            party.Age = command.Age;
            party.PartyTime = command.PartyTime;
            party.PartyAddress = command.PartyAddress;
            party.RSVPDetails = command.RSVPDetails;

            this._commandDispatcher.Dispatch(new SavePartyCommand(party, command.ActionBy));
        }
    }
}
