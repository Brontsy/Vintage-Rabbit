using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Serialization;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Payment.Entities;

namespace Vintage.Rabbit.Payment.Repository
{
    internal interface IEWayPaymentRepository
    {
        void SaveEwayPayment(EWayPayment ewayPayment);

        EWayPayment GetEwayPaymentByAccessCode(string accessCode);
    }

    internal class EWayPaymentRepository : IEWayPaymentRepository
    {
        private ISerializer _serializer;
        private string _connectionString;

        public EWayPaymentRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public void SaveEwayPayment(EWayPayment ewayPayment)
        {
            if (this.GetEwayPaymentByAccessCode(ewayPayment.AccessCode) == null)
            {
                string sql = @"Insert Into VintageRabbit.EWayPayments (Guid, OrderGuid, AccessCode, InvoiceNumber, AuthorisationCode, ResponseCode, ResponseMessage, TransactionID, TransactionStatus, DateCreated, DateLastModified) 
                                Values (@Guid, @OrderGuid, @AccessCode, @InvoiceNumber, @AuthorisationCode, @ResponseCode, @ResponseMessage, @TransactionID, @TransactionStatus, @DateCreated, @DateLastModified)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = ewayPayment.Guid,
                        OrderGuid = ewayPayment.OrderGuid,
                        AccessCode = ewayPayment.AccessCode,
                        InvoiceNumber = ewayPayment.InvoiceNumber,
                        AuthorisationCode = ewayPayment.AuthorisationCode,
                        ResponseCode = ewayPayment.ResponseCode,
                        ResponseMessage = ewayPayment.ResponseMessage,
                        TransactionID = ewayPayment.TransactionID,
                        TransactionStatus = ewayPayment.TransactionStatus,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now
                    });
                }
            }
            else
            {
                string sql = @"Update VintageRabbit.EWayPayments Set OrderGuid = @OrderGuid, AccessCode = @AccessCode, InvoiceNumber = @InvoiceNumber, AuthorisationCode = @AuthorisationCode, 
                                ResponseCode = @ResponseCode, ResponseMessage = @ResponseMessage, TransactionID = @TransactionID, TransactionStatus = @TransactionStatus ,
                                DateLastModified = @DateLastModified Where Guid = @Guid";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = ewayPayment.Guid,
                        OrderGuid = ewayPayment.OrderGuid,
                        AccessCode = ewayPayment.AccessCode,
                        InvoiceNumber = ewayPayment.InvoiceNumber,
                        AuthorisationCode = ewayPayment.AuthorisationCode,
                        ResponseCode = ewayPayment.ResponseCode,
                        ResponseMessage = ewayPayment.ResponseMessage,
                        TransactionID = ewayPayment.TransactionID,
                        TransactionStatus = ewayPayment.TransactionStatus,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now
                    });
                }
            }
        }

        public EWayPayment GetEwayPaymentByAccessCode(string accessCode)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var ewayPayments = connection.Query<EWayPayment>("Select * From VintageRabbit.EWayPayments Where AccessCode = @AccessCode", new { AccessCode = accessCode });

                if (ewayPayments.Any())
                {
                    return ewayPayments.First();
                }
            }

            return null;
        }
    }
}
