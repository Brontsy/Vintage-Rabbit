using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Interfaces.CQRS
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand command);
    }
}
