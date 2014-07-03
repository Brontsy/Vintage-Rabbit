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
    public class GetProductsQuery
    {
        public int Page { get; private set; }

        public GetProductsQuery(int page = 1)
        {
            this.Page = page;
        }
    }

    internal class GetProductsQueryHandler : IQueryHandler<IList<Product>, GetProductsQuery>
    {
        private ICacheService _cacheService;
        private IProductRepository _productRepository;

        public GetProductsQueryHandler(ICacheService cacheService, IProductRepository productRepository)
        {
            this._cacheService = cacheService;
            this._productRepository = productRepository;
        }

        public IList<Product> Handle(GetProductsQuery query)
        {
            string cacheKey = CacheKeyHelper.Product.All();

            if (this._cacheService.Exists(cacheKey))
            {
                return this._cacheService.Get<IList<Product>>(cacheKey);
            }

            IList<Product> products = this._productRepository.GetProducts(query.Page);

            this._cacheService.Add(cacheKey, products);
            foreach (Product product in products)
            {
                this._cacheService.Add(CacheKeyHelper.Product.ById(product.Id), product);
                this._cacheService.Add(CacheKeyHelper.Product.ByGuid(product.Guid), product);
            }

            return products;
        }
    }
}
