using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Serialization;

namespace Vintage.Rabbit.Messaging.Services
{
    internal class MessageService : IMessageService
    {
        private ISerializer _serializer;

        public MessageService(ISerializer serializer)
        {
            this._serializer = serializer;
        }

        public void AddMessage<TMessage>(TMessage message)
        {
            string s = this._serializer.Serialize(message);
        }
    }
}
