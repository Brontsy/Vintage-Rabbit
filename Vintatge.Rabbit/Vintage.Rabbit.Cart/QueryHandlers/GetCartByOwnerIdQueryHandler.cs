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

namespace Vintage.Rabbit.Carts.QueryHandlers
{
    public class GetCartByOwnerIdQuery
    {
        public Guid OwnerId { get; private set; }

        public GetCartByOwnerIdQuery(Guid ownerId)
        {
            this.OwnerId = ownerId;
        }
    }

    internal class GetCartByOwnerIdQueryHandler : IQueryHandler<Cart, GetCartByOwnerIdQuery>
    {
        private ICartRepository _cartRepository;
        private ICacheService _cacheService;

        public GetCartByOwnerIdQueryHandler(ICacheService cacheService, ICartRepository cartRepository)
        {
            this._cartRepository = cartRepository;
            this._cacheService = cacheService;
        }

        public Cart Handle(GetCartByOwnerIdQuery query)
        {
            string cacheKey = CacheKeyHelper.Cart.ByOwnerId(query.OwnerId);

            if(this._cacheService.Exists(cacheKey))
            {
                return this._cacheService.Get<Cart>(cacheKey);
            }

            Cart cart = this._cartRepository.GetCartByOwnerId(query.OwnerId);

            if (cart == null)
            {
                cart = new Cart(query.OwnerId);
            }
            else
            {
                this._cacheService.Add(cacheKey, cart);
            }

            return cart;
        }
    }
}
