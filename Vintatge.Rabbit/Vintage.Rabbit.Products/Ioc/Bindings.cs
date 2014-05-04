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
            builder.RegisterType<GetBuyProductQueryHandler>().As<IQueryHandler<BuyProduct, GetBuyProductQuery>>();
            builder.RegisterType<GetBuyProductsQueryHandler>().As<IQueryHandler<IList<BuyProduct>, GetBuyProductsQuery>>();
            builder.RegisterType<GetBuyProductsByCategoryQueryHandler>().As<IQueryHandler<IList<BuyProduct>, GetBuyProductsByCategoryQuery>>();
            builder.RegisterType<GetHireProductsQueryHandler>().As<IQueryHandler<IList<HireProduct>, GetHireProductsQuery>>();
            builder.RegisterType<GetHireProductQueryHandler>().As<IQueryHandler<HireProduct, GetHireProductQuery>>();

            builder.RegisterType<GetCategoriesQueryHandler>().As<IQueryHandler<IList<Category>, GetCategoriesQuery>>();
            builder.RegisterType<GetCategoryQueryHandler>().As<IQueryHandler<Category, GetCategoryQuery>>();

            builder.RegisterType<IsHireProductAvailableQueryHandler>().As<IQueryHandler<bool, IsHireProductAvailableQuery>>();

            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
        }
    }
}
