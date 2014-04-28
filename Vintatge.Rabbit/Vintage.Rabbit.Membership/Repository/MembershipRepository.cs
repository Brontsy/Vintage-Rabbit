using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Membership.Entities;

namespace Vintage.Rabbit.Membership.Repository
{
    internal interface IMembershipRepository
    {
        Member GetMember(Guid memberId);
    }

    internal class MembershipRepository : IMembershipRepository
    {
        public Member GetMember(Guid memberId)
        {
            return new Member();
        }
    }
}
