using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Search.CommandHandlers;
using Vintage.Rabbit.Search.QueryHandlers;

namespace Vintage.Rabbit.Search.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BuildLuceneIndexCommandHandler>().As<ICommandHandler<BuildLuceneIndexCommand>>();

            builder.RegisterType<SearchQueryHandler>().As<IQueryHandler<IList<Product>, SearchQuery>>();
        }
    }
}
