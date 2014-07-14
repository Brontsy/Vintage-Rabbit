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
    public class GetProductsByCategoryQuery
    {
        public Category Category { get; private set; }

        public ProductType ProductType { get; private set; }

        public int Page { get; private set; }

        public int ItemsPerPage { get; private set; }

        public GetProductsByCategoryQuery(Category category, ProductType productType, int page, int itemsPerPage)
        {
            this.Category = category;
            this.ProductType = productType;
            this.Page = page;
            this.ItemsPerPage = itemsPerPage;
        }
    }

    internal class GetProductsByCategoryQueryHandler : IQueryHandler<PagedResult<Product>, GetProductsByCategoryQuery>
    {
        private ICacheService _cacheService;
        private IProductRepository _productRepository;
        private IQueryDispatcher _queryDispatcher;

        public GetProductsByCategoryQueryHandler(ICacheService cacheService, IQueryDispatcher queryDispatcher, IProductRepository productRepository)
        {
            this._cacheService = cacheService;
            this._productRepository = productRepository;
            this._queryDispatcher = queryDispatcher;
        }

        public PagedResult<Product> Handle(GetProductsByCategoryQuery query)
        {
            return this._productRepository.GetProductsByCategory(query.ProductType, query.Category, query.Page, query.ItemsPerPage);
        }
    }
}
