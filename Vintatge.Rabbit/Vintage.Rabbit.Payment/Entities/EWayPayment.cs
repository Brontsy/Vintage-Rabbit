using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Payment.Entities.Eway;
using Vintage.Rabbit.Payment.Enums;

namespace Vintage.Rabbit.Payment.Entities
{
    public class EWayPayment
    {
        public Guid Guid { get; internal set; }

        public Guid OrderGuid { get; internal set; }

        public string AccessCode { get; internal set; }

        public string InvoiceNumber { get; internal set; }

        public string AuthorisationCode { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseMessage { get; set; }

        public string TransactionID { get; set; }

        public string TransactionStatus { get; set; }

        public EWayPayment() { }

        public EWayPayment(IOrder order, EwayPaymentResponse response)
        {
            this.Guid = Guid.NewGuid();
            this.OrderGuid = order.Guid;

            this.AccessCode = response.AccessCode;
            this.InvoiceNumber = response.InvoiceNumber;
            this.AuthorisationCode = response.AuthorisationCode;
            this.ResponseCode = response.ResponseCode;
            this.ResponseMessage = response.ResponseMessage;
            this.TransactionID = response.TransactionID;
            this.TransactionStatus = response.TransactionStatus;
        }
    }
}
