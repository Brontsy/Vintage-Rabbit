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
    public class GetBuyProductQuery
    {
        public int ProductId { get; private set; }

        public GetBuyProductQuery(int productId)
        {
            this.ProductId = productId;
        }
    }

    internal class GetBuyProductQueryHandler : IQueryHandler<BuyProduct, GetBuyProductQuery>
    {
        private IProductRepository _productRepository;

        public GetBuyProductQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public BuyProduct Handle(GetBuyProductQuery query)
        {
            return this._productRepository.GetBuyProduct(query.ProductId);
        }
    }
}
