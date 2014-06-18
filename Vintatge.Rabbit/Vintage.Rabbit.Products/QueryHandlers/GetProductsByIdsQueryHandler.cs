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
        private IProductRepository _productRepository;

        public GetProductsByIdsQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public IList<Product> Handle(GetProductsByIdsQuery query)
        {
            return this._productRepository.GetProducts(1).Where(o => query.ProductIds.Contains(o.Id)).ToList();
        }
    }
}
