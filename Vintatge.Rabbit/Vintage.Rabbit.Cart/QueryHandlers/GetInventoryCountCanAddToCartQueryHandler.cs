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
using Vintage.Rabbit.Inventory.Entities;

namespace Vintage.Rabbit.Carts.QueryHandlers
{
    public class GetInventoryCountCanAddToCartQuery
    {
        public Guid OwnerId { get; private set; }

        public Guid ProductGuid { get; private set; }

        public DateTime? PartyDate { get; private set; }

        public GetInventoryCountCanAddToCartQuery(Guid ownerId, Guid productGuid, DateTime? partyDate = null)
        {
            this.OwnerId = ownerId;
            this.ProductGuid = productGuid;
            this.PartyDate = partyDate;
        }
    }

    internal class GetInventoryCountCanAddToCartQueryHandler : IQueryHandler<int, GetInventoryCountCanAddToCartQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public GetInventoryCountCanAddToCartQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public int Handle(GetInventoryCountCanAddToCartQuery query)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByGuidQuery>(new GetProductByGuidQuery(query.ProductGuid));
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(query.OwnerId));

            int quantityInCart = 0;
            int totalInventoryAvailable = 0;

            if (cart.Items.Any(o => o.Product.Guid == query.ProductGuid))
            {
                quantityInCart = cart.Items.Where(o => o.Product.Guid == query.ProductGuid).Sum(o => o.Quantity);
            }

            IList<InventoryItem> inventory = this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(query.ProductGuid));

            if(product.Type == ProductType.Buy)
            {
                totalInventoryAvailable = inventory.Count(o => o.IsAvailable());
            }
            else if (product.Type == ProductType.Hire && query.PartyDate.HasValue)
            {
                totalInventoryAvailable = inventory.Count(o => o.IsAvailable(query.PartyDate.Value));
            }

            if(cart.Items.Any(o => o.Product.Type == ProductType.Theme))
            {
                foreach (var themeCartItem in cart.Items.Where(o => o.Product.Type == ProductType.Theme))
                {
                    Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(themeCartItem.Product.Guid));
                    var products = theme.GetProductGuids();

                    if(products.Keys.Any(o => o == query.ProductGuid))
                    {
                        quantityInCart = quantityInCart + products[query.ProductGuid];
                    }
                }
            }

            return totalInventoryAvailable - quantityInCart;
        }
    }
}
