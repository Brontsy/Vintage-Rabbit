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
    public class IsProductAvailableForHireQuery
    {
        public int ProductId { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public IsProductAvailableForHireQuery(int productId, DateTime startDate, DateTime endDate)
        {
            this.ProductId = productId;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }
    }

    internal class IsProductAvailableForHireQueryHandler : IQueryHandler<bool, IsProductAvailableForHireQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public IsProductAvailableForHireQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public bool Handle(IsProductAvailableForHireQuery query)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(query.ProductId));

            return product.IsAvailableForHire(query.StartDate, query.EndDate);
        }
    }
}
