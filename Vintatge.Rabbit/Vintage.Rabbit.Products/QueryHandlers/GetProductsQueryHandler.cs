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
    public class GetProductsQuery
    {
        public int Page { get; private set; }

        public GetProductsQuery(int page = 1)
        {
            this.Page = page;
        }
    }

    internal class GetProductsQueryHandler : IQueryHandler<IList<Product>, GetProductsQuery>
    {
        private IProductRepository _productRepository;

        public GetProductsQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public IList<Product> Handle(GetProductsQuery query)
        {
            return this._productRepository.GetProducts(query.Page);
        }
    }
}
