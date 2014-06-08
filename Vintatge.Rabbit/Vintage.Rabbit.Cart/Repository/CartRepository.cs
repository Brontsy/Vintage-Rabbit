using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.Messaging.Messages;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Serialization;
using System.Configuration;
using System.Data.SqlClient;

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
        private string _connectionString;

        public CartRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public Cart GetCart(Guid cartid)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var serialised = connection.Query<dynamic>("Select * From VintageRabbit.Carts Where Id = @Id", new { Id = cartid.ToString() });

                if(serialised.Any())
                {
                    if(!string.IsNullOrEmpty(serialised.First().Json))
                    {
                        return this._serializer.Deserialize<Cart>(serialised.First().Json);
                    }
                }
            }

            return null;
        }

        public Cart GetCartByOwnerId(Guid memberId)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var serialised = connection.Query<string>("Select Json From VintageRabbit.Carts Where MemberId = @MemberId", new { MemberId = memberId.ToString() });

                if(serialised.Any())
                {
                    if(!string.IsNullOrEmpty(serialised.First()))
                    {
                        return this._serializer.Deserialize<Cart>(serialised.First());
                    }
                }
            }

            return null;
        }

        public void Handle(SaveCartMessage message)
        {
            string serialized = this._serializer.Serialize(message.Cart);

            if(this.GetCart(message.Cart.Id) == null)
            {
                // insert
                string sql = "Insert Into VintageRabbit.Carts (Id, MemberId, Json, DateCreated, DateLastModified) Values (@Id, @MemberId, @Json, @DateCreated, @DateLastModified)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Id = message.Cart.Id,
                        MemberId = message.Cart.MemberId,
                        Json = serialized,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now
                     
                    });
                }
            }
            else
            {
                //update
                string sql = @"Update VintageRabbit.Carts Set MemberId = @MemberId, Json = @Json, DateLastModified = @DateLastModified Where Id = @Id";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Id = message.Cart.Id,
                        MemberId = message.Cart.MemberId,
                        Json = serialized,
                        DateLastModified = DateTime.Now

                    });
                }
            }
        }
    }
}
