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
    public class IsHireProductAvailableQuery
    {
        public int ProductId { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public IsHireProductAvailableQuery(int productId, DateTime startDate, DateTime endDate)
        {
            this.ProductId = productId;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }
    }

    internal class IsHireProductAvailableQueryHandler : IQueryHandler<bool, IsHireProductAvailableQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public IsHireProductAvailableQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public bool Handle(IsHireProductAvailableQuery query)
        {
            HireProduct product = this._queryDispatcher.Dispatch<HireProduct, GetHireProductQuery>(new GetHireProductQuery(query.ProductId));
            DateTime date = query.StartDate;

            bool available = true;

            while(date < query.EndDate)
            {
                if(product.UnavailableDates.Contains(date))
                {
                    available = false;
                }

                date = date.AddDays(1);
            }

            return available;
        }
    }
}
