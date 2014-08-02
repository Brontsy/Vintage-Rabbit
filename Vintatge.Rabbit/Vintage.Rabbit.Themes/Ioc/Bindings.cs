using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Themes.CommandHandlers;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Themes.QueryHandlers;
using Vintage.Rabbit.Themes.Repository;

namespace Vintage.Rabbit.Themes.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SaveThemeCommandHandler>().As<ICommandHandler<SaveThemeCommand>>();
            builder.RegisterType<AddProductToThemeCommandHandler>().As<ICommandHandler<AddProductToThemeCommand>>();
            builder.RegisterType<RemoveProductFromThemeCommandHandler>().As<ICommandHandler<RemoveProductFromThemeCommand>>();
            

            builder.RegisterType<GetThemesQueryHandler>().As<IQueryHandler<IList<Theme>, GetThemesQuery>>();
            builder.RegisterType<GetThemeByGuidQueryHandler>().As<IQueryHandler<Theme, GetThemeByGuidQuery>>();


            builder.RegisterType<ThemeRepository>().As<IThemeRepository>();
        }
    }
}
