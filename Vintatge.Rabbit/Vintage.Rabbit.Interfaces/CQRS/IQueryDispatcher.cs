using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Interfaces.CQRS
{
    public interface IQueryDispatcher
    {
        TResponse Dispatch<TResponse, TQuery>(TQuery query);
    }
}
