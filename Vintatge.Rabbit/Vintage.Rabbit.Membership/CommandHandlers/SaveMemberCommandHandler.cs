using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Messaging.Messages;
using Vintage.Rabbit.Membership.Repository;

namespace Vintage.Rabbit.Membership.CommandHandlers
{
    public class SaveMemberCommand
    {
        public Member Member { get; private set; }

        public SaveMemberCommand(Member member)
        {
            this.Member = member;
        }
    }

    internal class SaveMemberCommandHandler : ICommandHandler<SaveMemberCommand>
    {
        private ICacheService _cacheService;
        private IMessageService _messageService;

        public SaveMemberCommandHandler(ICacheService cacheService, IMessageService messageService)
        {
            this._cacheService = cacheService;
            this._messageService = messageService;
        }

        public void Handle(SaveMemberCommand command)
        {
            this._cacheService.Add(CacheKeyHelper.Member.ByGuid(command.Member.Guid), command.Member);
            this._cacheService.Add(CacheKeyHelper.Member.ByEmail(command.Member.Email), command.Member);

            SaveMemberMessage message = new SaveMemberMessage(command.Member);

            this._messageService.AddMessage(message);
        }
    }
}
