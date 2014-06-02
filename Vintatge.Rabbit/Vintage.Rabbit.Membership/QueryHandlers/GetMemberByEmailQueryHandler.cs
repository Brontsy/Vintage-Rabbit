using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Repository;

namespace Vintage.Rabbit.Membership.QueryHandlers
{
    public class GetMemberByEmailQuery
    {
        public string Email { get; private set; }

        public GetMemberByEmailQuery(string email)
        {
            this.Email = email;
        }
    }

    internal class GetMemberByEmailQueryHandler : IQueryHandler<Member, GetMemberByEmailQuery>
    {
        private IMembershipRepository _membershipRepository;
        private ICacheService _cacheService;

        public GetMemberByEmailQueryHandler(ICacheService cacheService, IMembershipRepository membershipRepository)
        {
            this._membershipRepository = membershipRepository;
            this._cacheService = cacheService;
        }

        public Member Handle(GetMemberByEmailQuery query)
        {
            string cacheKey = CacheKeyHelper.Member.ByEmail(query.Email);

            if(this._cacheService.Exists(cacheKey))
            {
                return this._cacheService.Get<Member>(cacheKey);
            }

            Member member = this._membershipRepository.GetMemberByEmail(query.Email);

            this._cacheService.Add(cacheKey, member);
            this._cacheService.Add(CacheKeyHelper.Member.ById(member.Id), member);
            this._cacheService.Add(CacheKeyHelper.Member.ByEmail(member.Email), member);

            return member;
        }
    }
}
