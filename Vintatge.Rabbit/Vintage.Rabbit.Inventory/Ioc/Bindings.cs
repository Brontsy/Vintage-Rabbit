using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Interfaces.Products;
using Vintage.Rabbit.Inventory.CommandHandlers;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Inventory.Messaging.Handlers;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Inventory.Repository;

namespace Vintage.Rabbit.Inventory.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GetInventoryForProductQueryHandler>().As<IQueryHandler<IList<InventoryItem>, GetInventoryForProductQuery>>();
            builder.RegisterType<GetInventoryForProductsQueryHandler>().As<IQueryHandler<IList<InventoryItem>, GetInventoryForProductsQuery>>();
            builder.RegisterType<IsProductAvailableForHireQueryHandler>().As<IQueryHandler<bool, IsProductAvailableForHireQuery>>();
            builder.RegisterType<IsProductAvailableQueryHandler>().As<IQueryHandler<bool, IsProductAvailableQuery>>();
            builder.RegisterType<GetInventoryByGuidQueryHandler>().As<IQueryHandler<InventoryItem, GetInventoryByGuidQuery>>();

            builder.RegisterType<SaveInventoryCommandHandler>().As<ICommandHandler<SaveInventoryCommand>>();
            builder.RegisterType<AddInventoryCommandHandler>().As<ICommandHandler<AddInventoryCommand>>();
            builder.RegisterType<DeleteInventoryCommandHandler>().As<ICommandHandler<DeleteInventoryCommand>>();

            builder.RegisterType<InventoryRepository>().As<IInventoryRepository>();
            builder.RegisterType<ProductCreatedMessageHandler>().As<IMessageHandler<IProductCreatedMessage>>();
            builder.RegisterType<ProductHiredMessageHandler>().As<IMessageHandler<IProductHiredMessage>>();
            builder.RegisterType<ProductPurchasedMessageHandler>().As<IMessageHandler<IProductPurchasedMessage>>();
        }
    }
}
