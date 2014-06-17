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

        public GetProductsByCategoryQueryHandler(ICacheService cacheService, IProductRepository productRepository)
        {
            this._cacheService = cacheService;
            this._productRepository = productRepository;
        }

        public IList<Product> Handle(GetProductsByCategoryQuery query)
        {
            string cacheKey = string.Format("Products-{0}-{1}", query.Category.Name, query.ProductType.ToString());

            if(this._cacheService.Exists(cacheKey))
            {
                return this._cacheService.Get<IList<Product>>(cacheKey);
            }

            IList<Product> products = this._productRepository.GetProductsByCategory(query.Category, query.ProductType);

            this._cacheService.Add(cacheKey, products);
            foreach(Product product in products)
            {
                this._cacheService.Add(CacheKeyHelper.Product.ById(product.Id), product);
                this._cacheService.Add(CacheKeyHelper.Product.ByGuid(product.Guid), product);
            }

            return products;
        }
    }
}
