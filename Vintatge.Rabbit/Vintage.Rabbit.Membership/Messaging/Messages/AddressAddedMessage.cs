using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Membership.Entities;

namespace Vintage.Rabbit.Membership.Messaging.Messages
{
    public class AddressAddedMessage : IMessage
    {
        public Address Address { get; private set; }

        public AddressAddedMessage(Address address)
        {
            this.Address = address;
        }
    }
}
