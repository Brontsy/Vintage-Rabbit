﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Encryption;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Messaging.Messages;

namespace Vintage.Rabbit.Membership.CommandHandlers
{
    public class RegisterCommand
    {
        public Guid MemberGuid { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public RegisterCommand(Guid memberGuid, string email, string password)
        {
            this.MemberGuid = memberGuid;
            this.Email = email;
            this.Password = password;
        }
    }

    internal class RegisterCommandHandler : ICommandHandler<RegisterCommand>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public RegisterCommandHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(RegisterCommand command)
        {
            Member member = new Member(command.MemberGuid, command.Email, SimpleHash.ComputeHash(command.Password, "MD5"));
            member.Roles.Add(Enums.Role.Member);

            this._commandDispatcher.Dispatch(new SaveMemberCommand(member));
        }
    }
}
