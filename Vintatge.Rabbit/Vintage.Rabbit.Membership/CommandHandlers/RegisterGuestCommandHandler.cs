using System;
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
    public class RegisterGuestCommand
    {
        public Guid MemberGuid { get; private set; }

        public RegisterGuestCommand(Guid memberGuid)
        {
            this.MemberGuid = memberGuid;
        }
    }

    internal class RegisterGuestCommandHandler : ICommandHandler<RegisterGuestCommand>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public RegisterGuestCommandHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(RegisterGuestCommand command)
        {
            Member member = new Member(command.MemberGuid);
            member.Roles.Add(Enums.Role.Guest);

            this._commandDispatcher.Dispatch(new SaveMemberCommand(member));
        }
    }
}
