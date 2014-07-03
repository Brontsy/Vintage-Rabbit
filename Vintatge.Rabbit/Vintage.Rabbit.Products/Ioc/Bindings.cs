using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Products.CommandHandlers;
using Vintage.Rabbit.Products.Entities;
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

            //builder.RegisterType<ProductRepository>().As<IMessageHandler<SaveProductMessage>>();

            builder.RegisterType<GetFeaturedProductsQueryHandler>().As<IQueryHandler<IList<Product>, GetFeaturedProductsQuery>>();


            builder.RegisterType<GetProductByGuidQueryHandler>().As<IQueryHandler<Product, GetProductByGuidQuery>>();
            builder.RegisterType<GetProductByIdQueryHandler>().As<IQueryHandler<Product, GetProductByIdQuery>>();
            builder.RegisterType<GetProductsQueryHandler>().As<IQueryHandler<IList<Product>, GetProductsQuery>>();
            builder.RegisterType<GetProductsByCategoryQueryHandler>().As<IQueryHandler<IList<Product>, GetProductsByCategoryQuery>>();
            builder.RegisterType<GetProductsByTypeQueryHandler>().As<IQueryHandler<IList<Product>, GetProductsByTypeQuery>>();
            builder.RegisterType<GetProductsByIdsQueryHandler>().As<IQueryHandler<IList<Product>, GetProductsByIdsQuery>>();

            

            builder.RegisterType<GetCategoriesQueryHandler>().As<IQueryHandler<IList<Category>, GetCategoriesQuery>>();
            builder.RegisterType<GetCategoryQueryHandler>().As<IQueryHandler<Category, GetCategoryQuery>>();

            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();


            builder.RegisterType<AzureBlobStorage>().As<IFileStorage>();
            builder.RegisterType<UploadProductImageService>().As<IUploadProductImageService>();
        }
    }
}
