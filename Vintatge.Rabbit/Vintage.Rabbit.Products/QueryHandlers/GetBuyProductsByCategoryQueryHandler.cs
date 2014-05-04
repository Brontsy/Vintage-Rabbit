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
    public class GetBuyProductsByCategoryQuery
    {
        public Category Category { get; private set; }

        public GetBuyProductsByCategoryQuery(Category category)
        {
            this.Category = category;
        }
    }

    internal class GetBuyProductsByCategoryQueryHandler : IQueryHandler<IList<BuyProduct>, GetBuyProductsByCategoryQuery>
    {
        private IProductRepository _productRepository;

        public GetBuyProductsByCategoryQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public IList<BuyProduct> Handle(GetBuyProductsByCategoryQuery query)
        {
            return this._productRepository.GetBuyProductsByCategory(query.Category);
        }
    }
}
