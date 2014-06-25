using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Membership.Enums;

namespace Vintage.Rabbit.Membership.Entities
{
    public class Address
    {
        public int Id { get; private set; }

        public Guid Guid { get; private set; }

        public Guid MemberGuid { get; private set; }

        public string Line1 { get; internal set; }

        public string SuburbCity { get; internal set; }

        public string State { get; internal set; }

        public int Postcode { get; internal set; }

        public string CompanyName { get; internal set; }

        public string FirstName { get; internal set; }

        public string LastName { get; internal set; }

        public string Email { get; internal set; }

        public string PhoneNumber { get; internal set; }

        public AddressType Type { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateLastModified { get; set; }

        public Address()
        {
            this.Guid = Guid.NewGuid();
        }
        public Address(Guid memberId, AddressType type, Guid addressGuid, string address, string suburbCity, string state, int postcode, string firstName, string lastName, string email, string phoneNumber = null, string companyName = null)
        {
            this.MemberGuid = memberId;
            this.Type = type;
            this.Guid = addressGuid;
            this.Line1 = address;
            this.SuburbCity = suburbCity;
            this.State = state;
            this.Postcode = postcode;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.CompanyName = companyName;
        }
    }
}
