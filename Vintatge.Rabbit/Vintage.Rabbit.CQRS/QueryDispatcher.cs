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
    public class QueryDispatcher : IQueryDispatcher
    {
        private IComponentContext _container;

        public QueryDispatcher(IComponentContext container) //IList<IQueryHandler<>> dispatchers)//IResolver resolver)
        {
            this._container = container;
        }

        public TResponse Dispatch<TResponse, TQuery>(TQuery query)
        {
            var handler = this._container.Resolve<IQueryHandler<TResponse, TQuery>>();

            return handler.Handle(query);
        }
    }
}
