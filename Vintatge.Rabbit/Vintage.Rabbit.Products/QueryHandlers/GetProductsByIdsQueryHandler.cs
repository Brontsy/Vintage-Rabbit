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
    public class GetProductsByIdsQuery
    {
        public IList<int> ProductIds { get; private set; }

        public GetProductsByIdsQuery(IList<int> productIds)
        {
            this.ProductIds = productIds;
        }
    }

    internal class GetProductsByIdsQueryHandler : IQueryHandler<IList<Product>, GetProductsByIdsQuery>
    {
        private ICacheService _cacheService;
        private IProductRepository _productRepository;

        public GetProductsByIdsQueryHandler(ICacheService cacheService, IProductRepository productRepository)
        {
            this._cacheService = cacheService;
            this._productRepository = productRepository;
        }

        public IList<Product> Handle(GetProductsByIdsQuery query)
        {
            IList<Product> foundProducts = new List<Product>();
            IList<int> notFoundProducts = new List<int>();

            foreach(var id in query.ProductIds)
            {
                string cacheKey = CacheKeyHelper.Product.ById(id);

                if (this._cacheService.Exists(cacheKey))
                {
                    foundProducts.Add(this._cacheService.Get<Product>(cacheKey));
                }
                else
                {
                    notFoundProducts.Add(id);
                }
            }

            IList<Product> products = this._productRepository.GetProductsById(notFoundProducts);

            foreach (Product product in products)
            {
                this._cacheService.Add(CacheKeyHelper.Product.ById(product.Id), product);
                this._cacheService.Add(CacheKeyHelper.Product.ByGuid(product.Guid), product);

                foundProducts.Add(product);
            }

            return foundProducts;
        }
    }
}
