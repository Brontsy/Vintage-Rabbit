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
    public class CanAddProductToCartQuery
    {
        public Guid OwnerId { get; private set; }

        public Guid ProductGuid { get; private set; }

        public int Qty { get; private set; }

        public CanAddProductToCartQuery(Guid ownerId, Guid productGuid, int qty)
        {
            this.OwnerId = ownerId;
            this.ProductGuid = productGuid;
        }
    }

    internal class CanAddProductToCartQueryHandler : IQueryHandler<bool, CanAddProductToCartQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public CanAddProductToCartQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public bool Handle(CanAddProductToCartQuery query)
        {
            return this._queryDispatcher.Dispatch<int, GetInventoryCountCanAddToCartQuery>(new GetInventoryCountCanAddToCartQuery(query.OwnerId, query.ProductGuid)) >= query.Qty;
        }
    }
}
