using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Membership;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Parties.Repositories;

namespace Vintage.Rabbit.Parties.CommandHandlers
{
    public class SavePartyCommand
    {
        public Party Party { get; private set; }

        public IActionBy ActionBy { get; private set; }

        public SavePartyCommand(Party party, IActionBy actionBy)
        {
            this.Party = party;
            this.ActionBy = actionBy;
        }
    }

    internal class SavePartyCommandHandler : ICommandHandler<SavePartyCommand>
    {
        private IPartyRepository _partyRepository;

        public SavePartyCommandHandler(IPartyRepository partyRepository)
        {
            this._partyRepository = partyRepository;
        }

        public void Handle(SavePartyCommand command)
        {
            this._partyRepository.SaveParty(command.Party, command.ActionBy);
        }
    }
}
