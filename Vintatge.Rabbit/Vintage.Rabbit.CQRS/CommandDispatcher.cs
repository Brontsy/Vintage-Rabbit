using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Ioc;

namespace Vintage.Rabbit.CQRS
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private IComponentContext _container;

        public CommandDispatcher(IComponentContext container)
        {
            this._container = container;
        }

        public void Dispatch<TCommand>(TCommand command)
        {
            var handler = this._container.Resolve<ICommandHandler<TCommand>>();

            handler.Handle(command);
        }
    }
}
