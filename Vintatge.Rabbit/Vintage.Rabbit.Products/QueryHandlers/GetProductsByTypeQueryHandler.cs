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
        private IProductRepository _productRepository;

        public GetProductsByTypeQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public IList<Product> Handle(GetProductsByTypeQuery query)
        {
            return this._productRepository.GetProductsByType(query.Type);
        }
    }
}
