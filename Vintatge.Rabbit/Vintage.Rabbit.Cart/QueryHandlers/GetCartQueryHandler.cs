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
    public class GetCartQuery
    {
        public Guid CartId { get; private set; }

        public GetCartQuery(Guid cartId)
        {
            this.CartId = cartId;
        }
    }

    internal class GetCartQueryHandler : IQueryHandler<Cart, GetCartQuery>
    {
        private ICartRepository _cartRepository;

        public GetCartQueryHandler(ICartRepository cartRepository)
        {
            this._cartRepository = cartRepository;
        }

        public Cart Handle(GetCartQuery query)
        {
            return this._cartRepository.GetCart(query.CartId);
        }
    }
}
