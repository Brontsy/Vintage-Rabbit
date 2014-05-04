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
    public class GetBuyProductsQuery
    {
        public int Page { get; private set; }

        public GetBuyProductsQuery(int? page)
        {
            this.Page = page ?? 1;
        }
    }

    internal class GetBuyProductsQueryHandler : IQueryHandler<IList<BuyProduct>, GetBuyProductsQuery>
    {
        private IProductRepository _productRepository;

        public GetBuyProductsQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public IList<BuyProduct> Handle(GetBuyProductsQuery query)
        {
            return this._productRepository.GetBuyProducts(query.Page);
        }
    }
}
