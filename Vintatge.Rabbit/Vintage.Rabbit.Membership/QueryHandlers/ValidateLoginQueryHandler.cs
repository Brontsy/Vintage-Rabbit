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
    public class ValidateLoginQuery
    {
        public string Email { get; private set; }

        public string Password { get; private set; }

        public ValidateLoginQuery(string email, string password)
        {
            this.Email = email; 
            this.Password = password;
        }
    }

    internal class ValidateLoginQueryHandler : IQueryHandler<bool, ValidateLoginQuery>
    {
        private IMembershipRepository _membershipRepository;

        public ValidateLoginQueryHandler(IMembershipRepository membershipRepository)
        {
            this._membershipRepository = membershipRepository;
        }

        public bool Handle(ValidateLoginQuery query)
        {
            if(query.Email == "brontsy@gmail.com" && query.Password == "password")
            {
                return true;
            }

            return false;
        }
    }
}
