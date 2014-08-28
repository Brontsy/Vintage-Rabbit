using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Enums;

namespace Vintage.Rabbit.Competitions.Entities
{
    public class CompetitionEntry
    {
        public int Id { get; internal set; }

        public Guid Guid { get; internal set; }

        public string CompetitionName { get; internal set; }

        public string Name { get; internal set; }

        public DateTime DateOfBirth { get; internal set; }

        public string Email { get; internal set; }

        public string PhoneNumber { get; internal set; }

        public string EntryText { get; internal set; }

        public DateTime DateCreated { get; internal set; }

        public CompetitionEntry() { }

        public CompetitionEntry(string competitionName, string name, DateTime dateOfBirth, string email, string phoneNumber, string entryText)
        {
            this.Guid = Guid.NewGuid();
            this.CompetitionName = competitionName;
            this.Name = name;
            this.DateOfBirth = dateOfBirth;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.EntryText = entryText;
        }
    }
}
