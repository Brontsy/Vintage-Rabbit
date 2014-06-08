using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Encryption;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Enums;
using Vintage.Rabbit.Membership.Repository;

namespace Vintage.Rabbit.Membership.QueryHandlers
{
    public class ValidateLoginQuery
    {
        public string Email { get; private set; }

        public string Password { get; private set; }

        public Role Role { get; private set; }

        public ValidateLoginQuery(string email, string password, Role role = Role.Member)
        {
            this.Email = email;
            this.Password = password;
            this.Role = role;
        }
    }

    internal class ValidateLoginQueryHandler : IQueryHandler<bool, ValidateLoginQuery>
    {
        private IMembershipRepository _membershipRepository;
        private IQueryDispatcher _queryDispatcher;

        public ValidateLoginQueryHandler(IMembershipRepository membershipRepository, IQueryDispatcher queryDispatcher)
        {
            this._membershipRepository = membershipRepository;
            this._queryDispatcher = queryDispatcher;
        }

        public bool Handle(ValidateLoginQuery query)
        {
            try
            {
                Member member = this._queryDispatcher.Dispatch<Member, GetMemberByEmailQuery>(new GetMemberByEmailQuery(query.Email));

                if (member != null)
                {
                    string encryptedPassword = SimpleHash.ComputeHash(query.Password, "MD5");

                    if (member != null && SimpleHash.VerifyHash(query.Password, "MD5", member.Password) && member.Roles.Contains(query.Role))
                    {
                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                // log
            }

            return false;
        }
    }
}
