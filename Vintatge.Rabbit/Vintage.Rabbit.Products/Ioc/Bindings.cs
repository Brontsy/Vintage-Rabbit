﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Products.CommandHandlers;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.Messaging.Handlers;
using Vintage.Rabbit.Products.Messaging.Messages;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Products.Repository;
using Vintage.Rabbit.Products.Services;

namespace Vintage.Rabbit.Products.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SaveProductCommandHandler>().As<ICommandHandler<SaveProductCommand>>();
            builder.RegisterType<RemovePhotoCommandHandler>().As<ICommandHandler<RemovePhotoCommand>>();
            builder.RegisterType<CreateProductCommandHandler>().As<ICommandHandler<CreateProductCommand>>();

            builder.RegisterType<InventorySoldMessageHandler>().As<IMessageHandler<IInventorySoldMessage>>();
            builder.RegisterType<ProductHiredMessageHandler>().As<IMessageHandler<IOrderPaidMessage>>();
            builder.RegisterType<ProductPurchasedMessageHandler>().As<IMessageHandler<IOrderPaidMessage>>();

            builder.RegisterType<GetFeaturedProductsQueryHandler>().As<IQueryHandler<IList<Product>, GetFeaturedProductsQuery>>();


            builder.RegisterType<GetProductByGuidQueryHandler>().As<IQueryHandler<Product, GetProductByGuidQuery>>();
            builder.RegisterType<GetProductByIdQueryHandler>().As<IQueryHandler<Product, GetProductByIdQuery>>();
            builder.RegisterType<GetProductsQueryHandler>().As<IQueryHandler<PagedResult<Product>, GetProductsQuery>>();
            builder.RegisterType<GetProductsByCategoryQueryHandler>().As<IQueryHandler<PagedResult<Product>, GetProductsByCategoryQuery>>();
            builder.RegisterType<GetProductsByTypeQueryHandler>().As<IQueryHandler<PagedResult<Product>, GetProductsByTypeQuery>>();
            builder.RegisterType<GetProductsByGuidsQueryHandler>().As<IQueryHandler<IList<Product>, GetProductsByGuidsQuery>>();
            builder.RegisterType<IsValidHirePostcodeQueryHandler>().As<IQueryHandler<bool, IsValidHirePostcodeQuery>>();

            

            builder.RegisterType<GetCategoriesQueryHandler>().As<IQueryHandler<IList<Category>, GetCategoriesQuery>>();
            builder.RegisterType<GetCategoryQueryHandler>().As<IQueryHandler<Category, GetCategoryQuery>>();

            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
            builder.RegisterType<PostcodeRepository>().As<IPostcodeRepository>();


            builder.RegisterType<AzureBlobStorage>().As<IFileStorage>();
            builder.RegisterType<UploadProductImageService>().As<IUploadProductImageService>();
        }
    }
}
