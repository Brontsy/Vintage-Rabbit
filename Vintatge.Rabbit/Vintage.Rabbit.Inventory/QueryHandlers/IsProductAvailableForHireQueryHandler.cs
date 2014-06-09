using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Inventory.Entities;

namespace Vintage.Rabbit.Inventory.QueryHandlers
{
    public class IsProductAvailableForHireQuery
    {
        public Guid ProductGuid { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public IsProductAvailableForHireQuery(Guid productGuid, DateTime startDate, DateTime endDate)
        {
            this.ProductGuid = productGuid;
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
            IList<InventoryItem> inventory = this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(query.ProductGuid));

            return inventory.Any(o => o.IsAvailable(query.StartDate, query.EndDate));
        }
    }
}
