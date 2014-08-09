using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Inventory.QueryHandlers;

namespace Vintage.Rabbit.Themes.QueryHandlers
{
    public class IsThemeAvailableForHireQuery
    {
        public string ThemeName { get; private set; }

        public Guid? ThemeGuid { get; private set; }

        public DateTime PartyDate { get; private set; }

        public IsThemeAvailableForHireQuery(string themeName, DateTime partyDate)
        {
            this.ThemeName = themeName;
            this.PartyDate = partyDate;
        }

        public IsThemeAvailableForHireQuery(Guid themeGuid, DateTime partyDate)
        {
            this.ThemeGuid = themeGuid;
            this.PartyDate = partyDate;
        }
    }

    internal class IsThemeAvailableForHireQueryHandler : IQueryHandler<bool, IsThemeAvailableForHireQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public IsThemeAvailableForHireQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public bool Handle(IsThemeAvailableForHireQuery query)
        {
            Theme theme = null;

            if (query.ThemeGuid.HasValue)
            {
                theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(query.ThemeGuid.Value));
            }
            else
            {
                theme = this._queryDispatcher.Dispatch<IList<Theme>, GetThemesQuery>(new GetThemesQuery()).First(o => o.Title.ToUrl() == query.ThemeName);
            }

            Dictionary<Guid, int> products = this.GetProductGuids(theme);

            foreach (var productGuid in products)
            {
                if(!this._queryDispatcher.Dispatch<bool, IsProductAvailableForHireQuery>(new IsProductAvailableForHireQuery(productGuid.Key, productGuid.Value, query.PartyDate)))
                {
                    return false;
                }
            }

            return true;
        }


        private Dictionary<Guid, int> GetProductGuids(Theme theme)
        {
            Dictionary<Guid, int> products = new Dictionary<Guid, int>();

            foreach (var image in theme.Images)
            {
                foreach (var product in image.Products)
                {
                    if (!products.ContainsKey(product.Guid))
                    {
                        products.Add(product.ProductGuid, product.Qty);
                    }
                }
            }

            return products;
        }
    }
}
