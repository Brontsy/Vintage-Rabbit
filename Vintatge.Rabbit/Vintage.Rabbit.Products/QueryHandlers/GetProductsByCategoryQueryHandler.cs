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
        private IProductRepository _productRepository;

        public GetProductsByCategoryQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public IList<Product> Handle(GetProductsByCategoryQuery query)
        {
            return this._productRepository.GetProductsByCategory(query.Category, query.ProductType);
        }
    }
}
