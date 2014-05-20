using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Membership.Entities
{
    public class Address
    {
        public Guid Id { get; private set; }

        public string Line1 { get; internal set; }

        public string Line2 { get; internal set; }

        public string Town { get; internal set; }

        public string State { get; internal set; }

        public int Postcode { get; internal set; }

        public Address()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
