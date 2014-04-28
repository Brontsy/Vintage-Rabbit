using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Interfaces.Messaging
{
    public interface IMessageService
    {
        void AddMessage<TMessage>(TMessage message);
    }
}
