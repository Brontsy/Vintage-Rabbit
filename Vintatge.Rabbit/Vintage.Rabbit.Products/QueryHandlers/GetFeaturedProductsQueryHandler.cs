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
    public class GetFeaturedProductsQuery
    {
    }

    internal class GetFeaturedProductsQueryHandler : IQueryHandler<IList<Product>, GetFeaturedProductsQuery>
    {
        private IProductRepository _productRepository;

        public GetFeaturedProductsQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public IList<Product> Handle(GetFeaturedProductsQuery query)
        {
            return this._productRepository.GetFeaturedProducts();
        }
    }
}
