using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Ioc;
using Vintage.Rabbit.Interfaces.Messaging;

namespace Vintage.Rabbit.Messaging.Services
{
    internal class SyncMessageService : IMessageService
    {
        private IComponentContext _container;

        public SyncMessageService(IComponentContext container)
        {
            this._container = container;
        }

        public void AddMessage<TMessage>(TMessage message)
        {
            IList<IMessageHandler<TMessage>> messageHandlers = null;

            if (this._container.TryResolve(out messageHandlers))
            {
                foreach (IMessageHandler<TMessage> messageHandler in messageHandlers)
                {
                    messageHandler.Handle(message);
                }
            }
        }
    }
}
