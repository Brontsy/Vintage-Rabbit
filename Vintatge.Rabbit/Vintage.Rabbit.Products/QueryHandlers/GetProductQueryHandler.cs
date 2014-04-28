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
    public class GetProductQuery
    {
        public int ProductId { get; private set; }

        public GetProductQuery(int productId)
        {
            this.ProductId = productId;
        }
    }

    internal class GetProductQueryHandler : IQueryHandler<Product, GetProductQuery>
    {
        private IProductRepository _productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public Product Handle(GetProductQuery query)
        {
            return this._productRepository.GetProduct(query.ProductId);
        }
    }
}
