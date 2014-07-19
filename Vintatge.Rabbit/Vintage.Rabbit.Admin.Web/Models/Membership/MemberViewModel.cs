using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Enums;

namespace Vintage.Rabbit.Admin.Web.Models.Membership
{
    public class MemberViewModel
    {
        public int Id { get; private set; }

        public Guid Guid { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public IList<Role> Roles { get; internal set; }


        public MemberViewModel(Member member)
        {
            this.Id = member.Id;
            this.Guid = member.Guid;
            this.Email = member.Email;
            this.Password = member.Password;
            this.FirstName = member.FirstName;
            this.LastName = member.LastName;
            this.Roles = member.Roles;
        }
    }
}