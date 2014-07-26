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
    public class GetFeaturedProductsQuery
    {
    }

    internal class GetFeaturedProductsQueryHandler : IQueryHandler<IList<Product>, GetFeaturedProductsQuery>
    {
        private IProductRepository _productRepository;
        private ICacheService _cacheService;

        public GetFeaturedProductsQueryHandler(IProductRepository productRepository, ICacheService cacheService)
        {
            this._productRepository = productRepository;
            this._cacheService = cacheService;
        }

        public IList<Product> Handle(GetFeaturedProductsQuery query)
        {
            string key = CacheKeyHelper.Product.Featured();

            if(this._cacheService.Exists(key))
            {
                return this._cacheService.Get<IList<Product>>(key);
            }

            IList<Product> products = this._productRepository.GetFeaturedProducts();
            this._cacheService.Add(key, products);

            return products;
        }
    }
}
