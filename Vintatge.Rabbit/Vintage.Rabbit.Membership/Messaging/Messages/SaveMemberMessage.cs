using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Membership.Entities;

namespace Vintage.Rabbit.Membership.Messaging.Messages
{
    public class SaveMemberMessage : IMessage
    {
        public Member Member { get; private set; }

        public SaveMemberMessage(Member member)
        {
            this.Member = member;
        }
    }
}
