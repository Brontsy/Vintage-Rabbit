using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Interfaces.CQRS
{
    public interface IQueryHandler<TResponse, TQuery>
    {
        TResponse Handle(TQuery query);
    }
}
