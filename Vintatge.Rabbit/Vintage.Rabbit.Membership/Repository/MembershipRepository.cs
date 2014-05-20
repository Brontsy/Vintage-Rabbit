using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Serialization;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Messaging.Messages;

namespace Vintage.Rabbit.Membership.Repository
{
    internal interface IMembershipRepository
    {
        Member GetMember(Guid memberId);

        Member GetMemberByEmail(string email);
    }

    internal class MembershipRepository : IMembershipRepository, IMessageHandler<SaveMemberMessage>
    {
        private ISerializer _serializer;

        public MembershipRepository(ISerializer serializer)
        {
            this._serializer = serializer;
        }

        public Member GetMember(Guid memberId)
        {
            return new Member();
        }

        public Member GetMemberByEmail(string email)
        {
            return new Member();
        }

        public void Handle(SaveMemberMessage message)
        {
            string serialized = this._serializer.Serialize(message);
        }
    }
}
