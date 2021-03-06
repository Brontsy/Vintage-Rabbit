﻿using System;
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

        public CreatePartyCommand(IOrder order, DateTime partyDate, IActionBy actionBy)
        {
            this.Order = order;
            this.PartyDate = partyDate;
            this.ActionBy = actionBy;
        }
    }

    internal class CreatePartyCommandHandler : ICommandHandler<CreatePartyCommand>
    {
        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;

        public CreatePartyCommandHandler(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(CreatePartyCommand command)
        {
            Party party = this._queryDispatcher.Dispatch<Party, GetPartyByOrderGuidQuery>(new GetPartyByOrderGuidQuery(command.Order.Guid));

            if (party == null)
            {
                party = new Party(command.Order, command.PartyDate);

                this._commandDispatcher.Dispatch(new SavePartyCommand(party, command.ActionBy));
            }

        }
    }
}
