using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Products.Repository;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Common.Enums;

namespace Vintage.Rabbit.Products.QueryHandlers
{
    public class GetProductsByCategoryQuery
    {
        public Category Category { get; private set; }

        public ProductType ProductType { get; private set; }

        public GetProductsByCategoryQuery(Category category, ProductType productType)
        {
            this.Category = category;
            this.ProductType = productType;
        }
    }

    internal class GetProductsByCategoryQueryHandler : IQueryHandler<IList<Product>, GetProductsByCategoryQuery>
    {
        private ICacheService _cacheService;
        private IProductRepository _productRepository;
        private IQueryDispatcher _queryDispatcher;

        public GetProductsByCategoryQueryHandler(ICacheService cacheService, IQueryDispatcher queryDispatcher, IProductRepository productRepository)
        {
            this._cacheService = cacheService;
            this._productRepository = productRepository;
            this._queryDispatcher = queryDispatcher;
        }

        public IList<Product> Handle(GetProductsByCategoryQuery query)
        {
            IList<Product> products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByTypeQuery>(new GetProductsByTypeQuery(query.ProductType));

            return products.Where(o => o.Categories.Any(x => x.Name == query.Category.Name || x.Children.Any(y => y.Name == query.Category.Name))).ToList();
        }
    }
}
