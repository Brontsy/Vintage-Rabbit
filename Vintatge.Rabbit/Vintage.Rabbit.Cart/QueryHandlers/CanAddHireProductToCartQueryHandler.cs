using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Carts.Repository;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Themes.QueryHandlers;

namespace Vintage.Rabbit.Carts.QueryHandlers
{
    public class CanAddHireProductToCartQuery
    {
        public Guid OwnerId { get; private set; }

        public Guid ProductGuid { get; private set; }

        public DateTime PartyDate { get; private set; }

        public int Qty { get; private set; }

        public CanAddHireProductToCartQuery(Guid ownerId, Guid productGuid, DateTime partyDate, int qty = 1)
        {
            this.OwnerId = ownerId;
            this.ProductGuid = productGuid;
            this.PartyDate = partyDate;
        }
    }

    internal class CanAddHireProductToCartQueryHandler : IQueryHandler<bool, CanAddHireProductToCartQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public CanAddHireProductToCartQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public bool Handle(CanAddHireProductToCartQuery query)
        {
            return this._queryDispatcher.Dispatch<int, GetInventoryCountCanAddToCartQuery>(new GetInventoryCountCanAddToCartQuery(query.OwnerId, query.ProductGuid, query.PartyDate)) >= query.Qty;
        }
    }
}
