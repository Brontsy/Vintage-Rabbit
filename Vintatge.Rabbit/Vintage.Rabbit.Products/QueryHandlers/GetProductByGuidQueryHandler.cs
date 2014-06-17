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

namespace Vintage.Rabbit.Products.QueryHandlers
{
    public class GetProductByGuidQuery
    {
        public Guid ProductGuid { get; private set; }

        public GetProductByGuidQuery(Guid productGuid)
        {
            this.ProductGuid = productGuid;
        }
    }

    internal class GetProductByGuidQueryHandler : IQueryHandler<Product, GetProductByGuidQuery>
    {
        private ICacheService _cacheService;
        private IProductRepository _productRepository;

        public GetProductByGuidQueryHandler(ICacheService cacheService, IProductRepository productRepository)
        {
            this._cacheService = cacheService;
            this._productRepository = productRepository;
        }

        public Product Handle(GetProductByGuidQuery query)
        {
            string cacheKey = CacheKeyHelper.Product.ByGuid(query.ProductGuid);

            if (this._cacheService.Exists(cacheKey))
            {
                return this._cacheService.Get<Product>(cacheKey);
            }

            Product product = this._productRepository.GetProductByGuid(query.ProductGuid);

            this._cacheService.Add(CacheKeyHelper.Product.ById(product.Id), product);
            this._cacheService.Add(CacheKeyHelper.Product.ByGuid(product.Guid), product);

            return product;
        }
    }
}
