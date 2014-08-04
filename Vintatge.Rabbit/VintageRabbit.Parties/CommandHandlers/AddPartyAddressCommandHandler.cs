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
    public class AddPartyAddressCommand
    {
        public IOrder Order { get; private set; }

        public IActionBy ActionBy { get; private set; }

        public Address Address { get; set; }

        public AddPartyAddressCommand(IOrder order, Address address, IActionBy actionBy)
        {
            this.Order = order;
            this.Address = address;
            this.ActionBy = actionBy;
        }
    }

    internal class AddPartyAddressCommandHandler : ICommandHandler<AddPartyAddressCommand>
    {
        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;

        public AddPartyAddressCommandHandler(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(AddPartyAddressCommand command)
        {
            Party party = this._queryDispatcher.Dispatch<Party, GetPartyByOrderGuidQuery>(new GetPartyByOrderGuidQuery(command.Order.Guid));

            if (party == null)
            {
                throw new Exception("Cannot find party to add address to");
            }

            party.DropoffAddress = command.Address.Guid;
            party.PickupAddress = command.Address.Guid;

            this._commandDispatcher.Dispatch(new SavePartyCommand(party, command.ActionBy));
        }
    }
}
