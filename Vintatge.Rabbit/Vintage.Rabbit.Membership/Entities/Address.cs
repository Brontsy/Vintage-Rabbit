using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool IsShippingAddress { get; set; }

        public Address()
        {
            this.Guid = Guid.NewGuid();
        }
        public Address(Guid memberId, Guid addressGuid, string address, string suburbCity, string state, int postcode, string firstName, string lastName, string companyName = null)
        {
            this.MemberGuid = memberId;
            this.Guid = addressGuid;
            this.Line1 = address;
            this.SuburbCity = suburbCity;
            this.State = state;
            this.Postcode = postcode;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CompanyName = companyName;
        }
    }
}
