using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Carts.CommandHandlers;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.Messaging.Messages;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Carts.Repository;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;

namespace Vintage.Rabbit.Carts.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GetCartQueryHandler>().As<IQueryHandler<Cart, GetCartQuery>>();
            builder.RegisterType<GetCartByOwnerIdQueryHandler>().As<IQueryHandler<Cart, GetCartByOwnerIdQuery>>();
            //builder.RegisterType<GetUnavailableCartItemsQueryHandler>().As<IQueryHandler<IList<CartItem>, GetUnavailableCartItemsQuery>>();

            builder.RegisterType<AddBuyProductToCartCommandHandler>().As<ICommandHandler<AddBuyProductToCartCommand>>();
            builder.RegisterType<AddHireProductToCartCommandHandler>().As<ICommandHandler<AddHireProductToCartCommand>>();
            builder.RegisterType<RemoveCartItemCommandHandler>().As<ICommandHandler<RemoveCartItemCommand>>();
            builder.RegisterType<SaveCartCommandHandler>().As<ICommandHandler<SaveCartCommand>>();


            builder.RegisterType<CartRepository>().As<IMessageHandler<SaveCartMessage>>();

            builder.RegisterType<CartRepository>().As<ICartRepository>();
        }
    }
}
