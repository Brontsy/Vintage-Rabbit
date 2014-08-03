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
    public class CreatePartyCommand
    {
        public IOrder Order { get; private set; }

        public DateTime PartyDate { get; private set; }

        public IActionBy ActionBy { get; private set; }

        public Address Address { get; set; }

        public string ChildsName { get; set; }

        public string Age { get; set; }

        public string PartyTime { get; set; }

        public string PartyAddress { get; set; }

        public string RSVPDetails { get; set; }

        public CreatePartyCommand(IOrder order, DateTime partyDate, IActionBy actionBy)
        {
            this.Order = order;
            this.PartyDate = partyDate;
            this.ActionBy = actionBy;
        }
    }

    internal class CreatePartyCommandHandler : ICommandHandler<CreatePartyCommand>
    {
        private IPartyRepository _partyRepository;
        private IQueryDispatcher _queryDispatcher;

        public CreatePartyCommandHandler(IPartyRepository partyRepository, IQueryDispatcher queryDispatcher)
        {
            this._partyRepository = partyRepository;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(CreatePartyCommand command)
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

            if(command.Address != null)
            {
                party.DropoffAddress = command.Address.Guid;
                party.PickupAddress = command.Address.Guid;
            }

            this._partyRepository.SaveParty(party, command.ActionBy);
        }
    }
}
