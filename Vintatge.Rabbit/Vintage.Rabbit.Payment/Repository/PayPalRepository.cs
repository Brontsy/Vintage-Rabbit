using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Serialization;
using Vintage.Rabbit.Payment.Entities;

namespace Vintage.Rabbit.Payment.Repository
{
    internal interface IPayPalRepository
    {
        PayPalPayment GetPayPalPayment(Guid guid);

        void SavePayPalPayment(PayPalPayment payment);
    }

    internal class PayPalRepository : IPayPalRepository
    {
        private ISerializer _serializer;
        private string _connectionString;

        public PayPalRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public PayPalPayment GetPayPalPayment(Guid guid)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var payments = connection.Query<PayPalPayment>("Select * From VintageRabbit.PayPalPayment Where Guid = @Guid", new { Guid = guid });

                if (payments.Any())
                {
                    return payments.First();
                }
            }

            return null;
        }

        public void SavePayPalPayment(PayPalPayment payment)
        {
            if (this.GetPayPalPayment(payment.Guid) == null)
            {
                // insert
                string sql = "Insert Into VintageRabbit.PayPalPayment (Guid, OrderGuid, Status, Token, Ack, CorrelationID, DateCreated, DateLastModified) Values (@Guid, @OrderGuid, @Status, @Token, @Ack, @CorrelationID, @DateCreated, @DateLastModified)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = payment.Guid,
                        OrderGuid = payment.OrderGuid,
                        Status = payment.Status.ToString(),
                        Token = payment.Token,
                        Ack = payment.Ack,
                        CorrelationID = payment.CorrelationID,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now

                    });
                }
            }
            else
            {
                //update
                string sql = @"Update VintageRabbit.PayPalPayment Set OrderGuid = @OrderGuid, Status = @Status, Token = @Token, Ack = @Ack, CorrelationID = @CorrelationID, DateLastModified = @DateLastModified Where Guid = @Guid";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = payment.Guid,
                        OrderGuid = payment.OrderGuid,
                        Status = payment.Status.ToString(),
                        Token = payment.Token,
                        Ack = payment.Ack,
                        CorrelationID = payment.CorrelationID,
                        DateLastModified = DateTime.Now

                    });
                }
            }
        }
    }
}
