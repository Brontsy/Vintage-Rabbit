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
    public class GetProductsByCategoryQuery
    {
        public Category Category { get; private set; }

        public GetProductsByCategoryQuery(Category category)
        {
            this.Category = category;
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
            return this._productRepository.GetProductsByCategory(query.Category);
        }
    }
}
