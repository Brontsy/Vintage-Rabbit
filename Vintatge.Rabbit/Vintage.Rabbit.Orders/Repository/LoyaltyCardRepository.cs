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
        IList<LoyaltyCard> GetLoyaltyCards();

        LoyaltyCard GetLoyaltyCard(string number);

        LoyaltyCard GetLoyaltyCardByGuid(Guid guid);

        void DeleteLoyaltyCard(Guid guid);

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


        public IList<LoyaltyCard> GetLoyaltyCards()
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                return connection.Query<LoyaltyCard>("Select * From VintageRabbit.LoyaltyCards").ToList();
            }
        }


        public void SaveLoyaltyCard(LoyaltyCard loyaltyCard)
        {

            if (this.GetLoyaltyCardByGuid(loyaltyCard.Guid) == null)
            {
                // insert
                string sql = "Insert Into VintageRabbit.LoyaltyCards (Guid, Number, Title, Discount, Status, DateCreated, DateConsumed, OrderGuid, LoyaltyCardType, Cost) Values (@Guid, @Number, @Title, @Discount, @Status, @DateCreated, @DateConsumed, @OrderGuid, @LoyaltyCardType, @Cost)";
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = loyaltyCard.Guid,
                        Number = loyaltyCard.Number,
                        Title = loyaltyCard.Title,
                        Discount = loyaltyCard.Discount,
                        Cost = loyaltyCard.Cost,
                        Status = loyaltyCard.Status.ToString(),
                        DateCreated = DateTime.Now,
                        DateConsumed = loyaltyCard.DateConsumed,
                        OrderGuid = loyaltyCard.OrderGuid,
                        LoyaltyCardType = loyaltyCard.LoyaltyCardType

                    });
                }
            }
            else
            {
                //update
                string sql = @"Update VintageRabbit.LoyaltyCards Set Number = @Number, Title = @Title, Discount = @Discount, Status = @Status, DateConsumed = @DateConsumed, OrderGuid = @OrderGuid, LoyaltyCardType= @LoyaltyCardType, Cost = @Cost Where Guid = @Guid";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = loyaltyCard.Guid,
                        Number = loyaltyCard.Number,
                        Title = loyaltyCard.Title,
                        Discount = loyaltyCard.Discount,
                        Cost = loyaltyCard.Cost,
                        Status = loyaltyCard.Status.ToString(),
                        DateCreated = DateTime.Now,
                        DateConsumed = loyaltyCard.DateConsumed,
                        OrderGuid = loyaltyCard.OrderGuid,
                        LoyaltyCardType = loyaltyCard.LoyaltyCardType
                    });
                }
            }
        }

        public void DeleteLoyaltyCard(Guid guid)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Execute("Delete From VintageRabbit.LoyaltyCards Where Guid = @Guid", new { Guid = guid });
            }
        }
    }
}
