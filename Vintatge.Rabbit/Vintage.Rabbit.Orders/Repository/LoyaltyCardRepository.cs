using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Orders.Messaging.Messages;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Common.Serialization;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Interfaces.Products;

namespace Vintage.Rabbit.Orders.Repository
{
    internal interface ILoyaltyCardRepository
    {
        LoyaltyCard GetLoyaltyCard(string number);

        void SaveLoyaltyCard(LoyaltyCard loyaltyCard);
    }

    internal class LoyaltyCardRepository : ILoyaltyCardRepository
    {
        private ISerializer _serializer;
        private string _connectionString;
        public LoyaltyCardRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public LoyaltyCard GetLoyaltyCard(string number)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var cards = connection.Query<LoyaltyCard>("Select * From VintageRabbit.LoyaltyCards Where Number = @Number", new { Number = number });

                if (cards.Any())
                {
                    return cards.First();
                }
            }

            return null;
        }

        public LoyaltyCard GetLoyaltyCardByGuid(Guid guid)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var cards = connection.Query<LoyaltyCard>("Select * From VintageRabbit.LoyaltyCards Where Guid = @Guid", new { Guid = guid });

                if (cards.Any())
                {
                    return cards.First();
                }
            }

            return null;
        }


        public void SaveLoyaltyCard(LoyaltyCard loyaltyCard)
        {

            if (this.GetLoyaltyCardByGuid(loyaltyCard.Guid) == null)
            {
                // insert
                string sql = "Insert Into VintageRabbit.LoyaltyCards (Guid, Number, Discount, Status, DateCreated, DateConsumed) Values (@Guid, @Number, @Discount, @Status, @DateCreated, @DateConsumed)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = loyaltyCard.Guid,
                        Number = loyaltyCard.Number,
                        Discount = loyaltyCard.Discount,
                        Status = loyaltyCard.Status.ToString(),
                        DateCreated = DateTime.Now,
                        DateConsumed = loyaltyCard.DateConsumed

                    });
                }
            }
            else
            {
                //update
                string sql = @"Update VintageRabbit.LoyaltyCards Set Number = @Number, Discount = @Discount, Status = @Status, DateConsumed = @DateConsumed Where Guid = @Guid";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = loyaltyCard.Guid,
                        Number = loyaltyCard.Number,
                        Discount = loyaltyCard.Discount,
                        Status = loyaltyCard.Status.ToString(),
                        DateCreated = DateTime.Now,
                        DateConsumed = loyaltyCard.DateConsumed
                    });
                }
            }
        }
    }
}
