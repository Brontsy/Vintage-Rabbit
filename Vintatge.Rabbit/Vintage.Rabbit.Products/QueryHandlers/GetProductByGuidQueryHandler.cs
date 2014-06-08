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
    public class GetProductByGuidQuery
    {
        public Guid ProductGuid { get; private set; }

        public GetProductByGuidQuery(Guid productGuid)
        {
            this.ProductGuid = productGuid;
        }
    }

    internal class GetProductByGuidQueryHandler : IQueryHandler<Product, GetProductByGuidQuery>
    {
        private IProductRepository _productRepository;

        public GetProductByGuidQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public Product Handle(GetProductByGuidQuery query)
        {
            return this._productRepository.GetProductByGuid(query.ProductGuid);
        }
    }
}
