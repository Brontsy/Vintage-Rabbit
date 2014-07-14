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
using Vintage.Rabbit.Common.Entities;

namespace Vintage.Rabbit.Products.QueryHandlers
{
    public class GetProductsQuery
    {
        public int Page { get; private set; }

        public int ItemsPerPage { get; private set; }

        public GetProductsQuery(int page, int itemsPerPage)
        {
            this.Page = page;
            this.ItemsPerPage = itemsPerPage;
        }
    }

    internal class GetProductsQueryHandler : IQueryHandler<PagedResult<Product>, GetProductsQuery>
    {
        private ICacheService _cacheService;
        private IProductRepository _productRepository;

        public GetProductsQueryHandler(ICacheService cacheService, IProductRepository productRepository)
        {
            this._cacheService = cacheService;
            this._productRepository = productRepository;
        }

        public PagedResult<Product> Handle(GetProductsQuery query)
        {
            string cacheKey = CacheKeyHelper.Product.All();

            if (this._cacheService.Exists(cacheKey))
            {
                //return this._cacheService.Get<IList<Product>>(cacheKey);
            }

            PagedResult<Product> products = this._productRepository.GetProducts(query.Page, query.ItemsPerPage);

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
