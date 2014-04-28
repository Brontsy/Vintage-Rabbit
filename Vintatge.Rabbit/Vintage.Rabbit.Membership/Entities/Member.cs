using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Membership.Entities
{
    public class Member
    {
        public Guid Id { get; private set; }

        public string Email { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public Member()
        {
            this.Id = Guid.NewGuid();
        }

        public Member(Guid id)
        {
            this.Id = id;
        }
    }
}
