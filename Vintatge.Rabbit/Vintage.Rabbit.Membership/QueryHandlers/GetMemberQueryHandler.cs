using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Repository;

namespace Vintage.Rabbit.Membership.QueryHandlers
{
    public class GetMemberQuery
    {
        public Guid MemberId { get; private set; }
    }

    internal class GetMemberQueryHandler : IQueryHandler<Member, GetMemberQuery>
    {
        private IMembershipRepository _membershipRepository;

        public GetMemberQueryHandler(IMembershipRepository membershipRepository)
        {
            this._membershipRepository = membershipRepository;
        }

        public Member Handle(GetMemberQuery query)
        {
            return this._membershipRepository.GetMember(query.MemberId);
        }
    }
}
