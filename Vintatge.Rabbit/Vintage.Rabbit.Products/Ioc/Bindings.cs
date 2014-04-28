using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Products.Repository;

namespace Vintage.Rabbit.Products.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GetFeaturedProductsQueryHandler>().As<IQueryHandler<IList<Product>, GetFeaturedProductsQuery>>();
            builder.RegisterType<GetProductQueryHandler>().As<IQueryHandler<Product, GetProductQuery>>();
            builder.RegisterType<GetProductsQueryHandler>().As<IQueryHandler<IList<Product>, GetProductsQuery>>();
            builder.RegisterType<GetProductsByCategoryQueryHandler>().As<IQueryHandler<IList<Product>, GetProductsByCategoryQuery>>();

            builder.RegisterType<GetCategoriesQueryHandler>().As<IQueryHandler<IList<Category>, GetCategoriesQuery>>();
            builder.RegisterType<GetCategoryQueryHandler>().As<IQueryHandler<Category, GetCategoryQuery>>();

            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
        }
    }
}
