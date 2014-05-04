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
    public class GetHireProductsQuery
    {
        public GetHireProductsQuery()
        {
        }
    }

    internal class GetHireProductsQueryHandler : IQueryHandler<IList<HireProduct>, GetHireProductsQuery>
    {
        private IProductRepository _productRepository;

        public GetHireProductsQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public IList<HireProduct> Handle(GetHireProductsQuery query)
        {
            return this._productRepository.GetHireProducts();
        }
    }
}
