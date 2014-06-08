using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Membership;
using Vintage.Rabbit.Membership.Enums;

namespace Vintage.Rabbit.Membership.Entities
{
    public class Member : IActionBy
    {
        public Guid Id { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public IList<Role> Roles { get; internal set; }

        public IList<Address> ShippingAddresses { get; private set; }

        public IList<Address> BillingAddresses { get; private set; }

        public Member(Guid id)
        {
            this.Id = id;
            this.ShippingAddresses = new List<Address>();
            this.BillingAddresses = new List<Address>();
            this.Roles = new List<Role>();
        }

        public Member() : this(Guid.NewGuid())
        {
        }

        public Member(string email, string password) : this()
        {
            this.Email = email;
            this.Password = password;
        }

        internal void AddShippingAddress(Address address)
        {
            this.ShippingAddresses.Add(address);
        }

        internal void AddBillingAddress(Address address)
        {
            this.BillingAddresses.Add(address);
        }
    }
}
