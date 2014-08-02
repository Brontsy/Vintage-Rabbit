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
    public class GetProductsByGuidsQuery
    {
        public IList<Guid> ProductGuids { get; private set; }

        public GetProductsByGuidsQuery(IList<Guid> productGuids)
        {
            this.ProductGuids = productGuids;
        }
    }

    internal class GetProductsByGuidsQueryHandler : IQueryHandler<IList<Product>, GetProductsByGuidsQuery>
    {
        private ICacheService _cacheService;
        private IProductRepository _productRepository;

        public GetProductsByGuidsQueryHandler(ICacheService cacheService, IProductRepository productRepository)
        {
            this._cacheService = cacheService;
            this._productRepository = productRepository;
        }

        public IList<Product> Handle(GetProductsByGuidsQuery query)
        {
            IList<Product> foundProducts = new List<Product>();
            IList<Guid> notFoundProducts = new List<Guid>();

            foreach(var id in query.ProductGuids)
            {
                string cacheKey = CacheKeyHelper.Product.ByGuid(id);

                if (this._cacheService.Exists(cacheKey))
                {
                    foundProducts.Add(this._cacheService.Get<Product>(cacheKey));
                }
                else
                {
                    notFoundProducts.Add(id);
                }
            }

            IList<Product> products = this._productRepository.GetProductsByGuid(notFoundProducts);

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
