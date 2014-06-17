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
    public class GetProductByIdQuery
    {
        public int ProductId { get; private set; }

        public GetProductByIdQuery(int productId)
        {
            this.ProductId = productId;
        }
    }

    internal class GetProductByIdQueryHandler : IQueryHandler<Product, GetProductByIdQuery>
    {
        private ICacheService _cacheService;
        private IProductRepository _productRepository;

        public GetProductByIdQueryHandler(ICacheService cacheService, IProductRepository productRepository)
        {
            this._cacheService = cacheService;
            this._productRepository = productRepository;
        }

        public Product Handle(GetProductByIdQuery query)
        {
            string cacheKey = CacheKeyHelper.Product.ById(query.ProductId);

            if (this._cacheService.Exists(cacheKey))
            {
                return this._cacheService.Get<Product>(cacheKey);
            }

            Product product = this._productRepository.GetProductById(query.ProductId);

            this._cacheService.Add(CacheKeyHelper.Product.ById(product.Id), product);
            this._cacheService.Add(CacheKeyHelper.Product.ByGuid(product.Guid), product);

            return product;
        }
    }
}
