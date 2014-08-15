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
    public class CanAddThemeToCartQuery
    {
        public Guid OwnerId { get; private set; }

        public Guid ThemeGuid { get; private set; }

        public DateTime PartyDate { get; private set; }

        public CanAddThemeToCartQuery(Guid ownerId, Guid themeGuid, DateTime partyDate)
        {
            this.OwnerId = ownerId;
            this.ThemeGuid = themeGuid;
            this.PartyDate = partyDate;
        }
    }

    internal class CanAddThemeToCartQueryHandler : IQueryHandler<bool, CanAddThemeToCartQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public CanAddThemeToCartQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public bool Handle(CanAddThemeToCartQuery query)
        {
            Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(query.ThemeGuid));

            Dictionary<Guid, int> products = theme.GetProductGuids();

            foreach (var productGuid in products)
            {
                if (!this._queryDispatcher.Dispatch<bool, CanAddHireProductToCartQuery>(new CanAddHireProductToCartQuery(query.OwnerId, productGuid.Key, query.PartyDate, productGuid.Value)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
