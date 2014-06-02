using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Membership.Entities
{
    public class Address
    {
        public Guid MemberId { get; private set; }

        public Guid Id { get; private set; }

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
            this.Id = Guid.NewGuid();
        }
        public Address(Guid memberId, Guid addressId, string address, string suburbCity, string state, int postcode, string firstName, string lastName, string companyName = null)
        {
            this.MemberId = memberId;
            this.Id = addressId;
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
