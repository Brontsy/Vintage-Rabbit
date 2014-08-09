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
    public class EWayPayment : CreditCardPayment
    {

        public string AccessCode { get; internal set; }

        public string InvoiceNumber { get; internal set; }

        public string AuthorisationCode { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseMessage { get; set; }

        public string TransactionID { get; set; }

        public string TransactionStatus { get; set; }

        public EWayPayment(IOrder order, AccessCodeResponse accessCodeResponse)
            :base(order)
        {
            this.AccessCode = accessCodeResponse.AccessCode;
            this.InvoiceNumber = accessCodeResponse.Payment.InvoiceNumber;
        }

        public EWayPayment(IOrder order, EwayPaymentResponse response)
            :base(order)
        {
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
