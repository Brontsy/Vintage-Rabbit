using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Blogs.Repository;
using Vintage.Rabbit.Blogs.QueryHandlers;
using Vintage.Rabbit.Blogs.Entities;
using Vintage.Rabbit.Blogs.CommandHandlers;

namespace Vintage.Rabbit.Blogs.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SaveBlogCommandHandler>().As<ICommandHandler<SaveBlogCommand>>();

            builder.RegisterType<GetBlogsQueryHandler>().As<IQueryHandler<IList<Blog>, GetBlogsQuery>>();
            builder.RegisterType<GetBlogByIdQueryHandler>().As<IQueryHandler<Blog, GetBlogByIdQuery>>();
            builder.RegisterType<GetBlogByKeyQueryHandler>().As<IQueryHandler<Blog, GetBlogByKeyQuery>>();

            builder.RegisterType<BlogRepository>().As<IBlogRepository>();
        }
    }
}
