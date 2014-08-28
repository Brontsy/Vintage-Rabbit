using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintage.Rabbit.Competitions.Entities;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Membership;
using Vintage.Rabbit.Competitions.Repositories;

namespace Vintage.Rabbit.Competitions.CommandHandlers
{
    public class SaveCompetitionEntryCommand
    {
        public string Name { get; private set; }

        public string CompetitionName { get; private set; }

        public DateTime DateOfBirth { get; private set; }

        public string Email { get; private set; }

        public string PhoneNumber { get; private set; }

        public string EntryText { get; private set; }

        public SaveCompetitionEntryCommand(string competitionName, string name, DateTime dateOfBirth, string email, string phoneNumber, string entryText)
        {
            this.CompetitionName = competitionName;
            this.Name = name;
            this.DateOfBirth = dateOfBirth;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.EntryText = entryText;
        }
    }

    internal class SaveCompetitionEntryCommandHandler : ICommandHandler<SaveCompetitionEntryCommand>
    {
        private ICompeitionRepository _compeitionRepository;

        public SaveCompetitionEntryCommandHandler(ICompeitionRepository compeitionRepository)
        {
            this._compeitionRepository = compeitionRepository;
        }

        public void Handle(SaveCompetitionEntryCommand command)
        {
            CompetitionEntry entry = new CompetitionEntry(command.CompetitionName, command.Name, command.DateOfBirth, command.Email, command.PhoneNumber, command.EntryText);

            this._compeitionRepository.SaveCompeitionEntry(entry);
        }
    }
}
