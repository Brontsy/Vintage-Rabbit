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
    public class GetMemberByIdQuery
    {
        public Guid Id { get; private set; }

        public GetMemberByIdQuery(Guid id)
        {
            this.Id = id;
        }
    }

    internal class GetMemberByIdQueryHandler : IQueryHandler<Member, GetMemberByIdQuery>
    {
        private IMembershipRepository _membershipRepository;
        private ICacheService _cacheService;

        public GetMemberByIdQueryHandler(ICacheService cacheService, IMembershipRepository membershipRepository)
        {
            this._membershipRepository = membershipRepository;
            this._cacheService = cacheService;
        }

        public Member Handle(GetMemberByIdQuery query)
        {
            string cacheKey = CacheKeyHelper.Member.ById(query.Id);

            if(this._cacheService.Exists(cacheKey))
            {
                return this._cacheService.Get<Member>(cacheKey);
            }

            Member member = this._membershipRepository.GetMember(query.Id);

            if (member != null)
            {
                this._cacheService.Add(cacheKey, member);
                this._cacheService.Add(CacheKeyHelper.Member.ById(member.Id), member);

                if (!string.IsNullOrEmpty(member.Email))
                {
                    this._cacheService.Add(CacheKeyHelper.Member.ByEmail(member.Email), member);
                }

                return member;
            }

            return new Member(query.Id);
        }
    }
}
