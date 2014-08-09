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
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Themes.QueryHandlers;

namespace Vintage.Rabbit.Carts.QueryHandlers
{
    public class GetUnavailableCartItemsQuery
    {
        public Cart Cart { get; private set; }

        public DateTime PartyDate { get; private set; }

        public GetUnavailableCartItemsQuery(Cart cart, DateTime partyDate)
        {
            this.Cart = cart;
            this.PartyDate = partyDate;
        }
    }

    internal class GetUnavailableCartItemsQueryHandler : IQueryHandler<IList<CartItem>, GetUnavailableCartItemsQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public GetUnavailableCartItemsQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public IList<CartItem> Handle(GetUnavailableCartItemsQuery query)
        {
            IList<CartItem> unavailableCartItems = new List<CartItem>();

            foreach (var cartItem in query.Cart.Items)
            {
                if (cartItem.Product.Type == Common.Enums.ProductType.Hire)
                {
                    if (!this._queryDispatcher.Dispatch<bool, IsProductAvailableForHireQuery>(new IsProductAvailableForHireQuery(cartItem.Product.Guid, cartItem.Quantity, query.PartyDate)))
                    {
                        unavailableCartItems.Add(cartItem);
                    }
                }
                else if (cartItem.Product.Type == Common.Enums.ProductType.Theme)
                {
                    if (!this._queryDispatcher.Dispatch<bool, IsThemeAvailableForHireQuery>(new IsThemeAvailableForHireQuery(cartItem.Product.Guid, query.PartyDate)))
                    {
                        unavailableCartItems.Add(cartItem);
                    }
                }
            }

            return unavailableCartItems;
        }
    }
}
