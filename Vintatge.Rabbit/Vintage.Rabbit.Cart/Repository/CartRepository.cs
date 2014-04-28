using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.Messaging.Messages;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Serialization;

namespace Vintage.Rabbit.Carts.Repository
{
    internal interface ICartRepository
    {
        Cart GetCart(Guid cartid);

        Cart GetCartByOwnerId(Guid ownerId);
    }

    internal class CartRepository : ICartRepository, IMessageHandler<SaveCartMessage>
    {
        private ISerializer _serializer;

        public CartRepository(ISerializer serializer)
        {
            this._serializer = serializer;
        }

        public Cart GetCart(Guid cartid)
        {
            string sql = "Select * From Query.Cart Where Id = @CartId";

            string serialized = string.Empty;

            return this._serializer.Deserialize<Cart>(serialized);
        }

        public Cart GetCartByOwnerId(Guid ownerId)
        {
            string sql = "Select * From Query.Cart Where OwnerId = @OwnerId";

            string serialized = string.Empty;

            return this._serializer.Deserialize<Cart>(serialized);
        }

        public void Handle(SaveCartMessage message)
        {
            string serialized = this._serializer.Serialize(message);

            string sql = "Insert Into Query.Cart";
        }
    }
}
