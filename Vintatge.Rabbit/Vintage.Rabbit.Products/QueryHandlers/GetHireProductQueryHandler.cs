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
    public class GetHireProductQuery
    {
        public int ProductId { get; private set; }

        public GetHireProductQuery(int productId)
        {
            this.ProductId = productId;
        }
    }

    internal class GetHireProductQueryHandler : IQueryHandler<HireProduct, GetHireProductQuery>
    {
        private IProductRepository _productRepository;

        public GetHireProductQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public HireProduct Handle(GetHireProductQuery query)
        {
            return this._productRepository.GetHireProduct(query.ProductId);
        }
    }
}
