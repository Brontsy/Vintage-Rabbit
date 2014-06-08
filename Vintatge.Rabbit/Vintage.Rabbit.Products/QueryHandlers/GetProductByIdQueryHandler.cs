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
    public class GetProductByIdQuery
    {
        public int ProductId { get; private set; }

        public GetProductByIdQuery(int productId)
        {
            this.ProductId = productId;
        }
    }

    internal class GetProductByIdQueryHandler : IQueryHandler<Product, GetProductByIdQuery>
    {
        private IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public Product Handle(GetProductByIdQuery query)
        {
            return this._productRepository.GetProductById(query.ProductId);
        }
    }
}
