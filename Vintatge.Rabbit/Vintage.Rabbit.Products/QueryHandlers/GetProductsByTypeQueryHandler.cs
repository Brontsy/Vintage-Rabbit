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
    public class GetProductsByTypeQuery
    {
        public ProductType Type { get; private set; }

        public GetProductsByTypeQuery(ProductType type)
        {
            this.Type = type;
        }
    }

    internal class GetProductsByTypeQueryHandler : IQueryHandler<IList<Product>, GetProductsByTypeQuery>
    {
        private ICacheService _cacheService;
        private IProductRepository _productRepository;

        public GetProductsByTypeQueryHandler(ICacheService cacheService, IProductRepository productRepository)
        {
            this._cacheService = cacheService;
            this._productRepository = productRepository;
        }

        public IList<Product> Handle(GetProductsByTypeQuery query)
        {
            string cacheKey = string.Format("Products-{0}", query.Type.ToString());

            if (this._cacheService.Exists(cacheKey))
            {
                return this._cacheService.Get<IList<Product>>(cacheKey);
            }

            IList<Product> products = this._productRepository.GetProductsByType(query.Type);

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
