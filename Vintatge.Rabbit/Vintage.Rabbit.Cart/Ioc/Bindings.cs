using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Carts.CommandHandlers;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.Messaging.Handlers;
using Vintage.Rabbit.Carts.Messaging.Messages;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Carts.Repository;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;

namespace Vintage.Rabbit.Carts.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GetCartQueryHandler>().As<IQueryHandler<Cart, GetCartQuery>>();
            builder.RegisterType<GetCartByOwnerIdQueryHandler>().As<IQueryHandler<Cart, GetCartByOwnerIdQuery>>();
            builder.RegisterType<GetUnavailableCartItemsQueryHandler>().As<IQueryHandler<IList<CartItem>, GetUnavailableCartItemsQuery>>();
            builder.RegisterType<GetInventoryCountCanAddToCartQueryHandler>().As<IQueryHandler<int, GetInventoryCountCanAddToCartQuery>>();

            builder.RegisterType<AddBuyProductToCartCommandHandler>().As<ICommandHandler<AddBuyProductToCartCommand>>();
            builder.RegisterType<AddHireProductToCartCommandHandler>().As<ICommandHandler<AddHireProductToCartCommand>>();
            builder.RegisterType<AddThemeToCartCommandHandler>().As<ICommandHandler<AddThemeToCartCommand>>();
            builder.RegisterType<RemoveCartItemCommandHandler>().As<ICommandHandler<RemoveCartItemCommand>>();
            builder.RegisterType<SaveCartCommandHandler>().As<ICommandHandler<SaveCartCommand>>();
            builder.RegisterType<ChangeCartsMemberGuidCommandHandler>().As<ICommandHandler<ChangeCartsMemberGuidCommand>>();
            builder.RegisterType<UpdateQuantityCommandHandler>().As<ICommandHandler<UpdateQuantityCommand>>();


            builder.RegisterType<CartRepository>().As<IMessageHandler<SaveCartMessage>>();

            builder.RegisterType<CartRepository>().As<ICartRepository>();

            builder.RegisterType<OrderPaidMessageHandler>().As<IMessageHandler<IOrderPaidMessage>>();
        }
    }
}
