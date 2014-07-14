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
using Vintage.Rabbit.Common.Entities;

namespace Vintage.Rabbit.Products.QueryHandlers
{
    public class GetProductsByTypeQuery
    {
        public ProductType Type { get; private set; }

        public int Page { get; private set; }

        public int ItemsPerPage { get; private set; }

        public GetProductsByTypeQuery(ProductType type, int page, int itemsPerPage)
        {
            this.Type = type;
            this.Page = page;
            this.ItemsPerPage = itemsPerPage;
        }
    }

    internal class GetProductsByTypeQueryHandler : IQueryHandler<PagedResult<Product>, GetProductsByTypeQuery>
    {
        private ICacheService _cacheService;
        private IProductRepository _productRepository;

        public GetProductsByTypeQueryHandler(ICacheService cacheService, IProductRepository productRepository)
        {
            this._cacheService = cacheService;
            this._productRepository = productRepository;
        }

        public PagedResult<Product> Handle(GetProductsByTypeQuery query)
        {
            string cacheKey = CacheKeyHelper.Product.ByType(query.Type);

            if (this._cacheService.Exists(cacheKey))
            {
                //return this._cacheService.Get<IList<Product>>(cacheKey);
            }

            PagedResult<Product> products = this._productRepository.GetProductsByType(query.Type, query.Page, query.ItemsPerPage);

            //this._cacheService.Add(cacheKey, products);
            foreach (Product product in products)
            {
                this._cacheService.Add(CacheKeyHelper.Product.ById(product.Id), product);
                this._cacheService.Add(CacheKeyHelper.Product.ByGuid(product.Guid), product);
            }

            return products;
        }
    }
}
