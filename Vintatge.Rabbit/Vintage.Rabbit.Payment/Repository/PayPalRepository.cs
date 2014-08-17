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
using Vintage.Rabbit.Payment.Enums;

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
                var payments = connection.Query<dynamic>("Select * From VintageRabbit.PayPalPayment Where Guid = @Guid Order By Id Desc", new { Guid = guid });

                if (payments.Any())
                {
                    return this.ConvertToPayPalPayment(payments.First());
                }
            }

            return null;
        }

        public void SavePayPalPayment(PayPalPayment payment)
        {
            if (this.GetPayPalPayment(payment.Guid) == null)
            {
                // insert
                string sql = @"Insert Into VintageRabbit.PayPalPayment (Guid, OrderGuid, Status, PayPalResponse, PayPalId, Errors, DateCreated, DateLastModified) 
                Values (@Guid, @OrderGuid, @Status, @PayPalResponse, @PayPalId, @Errors, @DateCreated, @DateLastModified)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = payment.Guid,
                        OrderGuid = payment.OrderGuid,
                        Status = payment.Status.ToString(),
                        PayPalResponse = payment.PayPalResponse,
                        PayPalId = payment.PayPalId,
                        Errors = this._serializer.Serialize(payment.Errors),
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now

                    });
                }
            }
            else
            {
                //update
                string sql = @"Update VintageRabbit.PayPalPayment Set OrderGuid = @OrderGuid, Status = @Status, PayPalResponse = @PayPalResponse, PayPalId = @PayPalId, Errors = @Errors, DateLastModified = @DateLastModified Where Guid = @Guid";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = payment.Guid,
                        OrderGuid = payment.OrderGuid,
                        Status = payment.Status.ToString(),
                        PayPalResponse = payment.PayPalResponse,
                        PayPalId = payment.PayPalId,
                        Errors = payment.Errors.Any() ? this._serializer.Serialize(payment.Errors) : null,
                        DateLastModified = DateTime.Now

                    });
                }
            }
        }

        private PayPalPayment ConvertToPayPalPayment(dynamic payment)
        {
            PayPalPayment payPalPayment = new PayPalPayment()
            {
                Id = payment.Id,
                Guid = payment.Guid,
                OrderGuid = payment.OrderGuid,
                Status = (PayPalPaymentStatus)Enum.Parse(typeof(PayPalPaymentStatus), payment.Status),
                PayPalResponse = payment.PayPalResponse,
                PayPalId = payment.PayPalId,
                DateCreated = payment.DateCreated,
                DateLastModified = payment.DateLastModified
            };

            if(payment.Errors != null)
            {
                payPalPayment.Errors = this._serializer.Deserialize<IList<PayPalError>>(payment.Errors);
            }

            return payPalPayment;
        }
    }
}
