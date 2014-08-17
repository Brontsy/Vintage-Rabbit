using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Payment.Enums;

namespace Vintage.Rabbit.Payment.Entities
{
    public class PayPalPayment
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid OrderGuid { get; set; }

        public PayPalPaymentStatus Status { get; set; }

        public string PayPalId { get; set; }

        public string PayPalPayerId { get; set; }

        public string PayPalResponse { get; set; }

        public IList<PayPalError> Errors { get; set; }
        
        public DateTime DateCreated { get; set; }

        public DateTime DateLastModified { get; set; }

        public PayPalPayment()
        {
            this.Errors = new List<PayPalError>();
        }

        public PayPalPayment(Guid guid, Guid orderGuid, string payPalId)
        {
            this.Guid = guid;
            this.OrderGuid = orderGuid;
            this.PayPalId = payPalId;
            this.DateCreated = DateTime.Now;
            this.Status = PayPalPaymentStatus.Initialised;
            this.Errors = new List<PayPalError>();
        }

        public void Completed(string payerId)
        {
            this.PayPalPayerId = payerId;
            this.Status = PayPalPaymentStatus.Completed;
        }

        public void Cancelled()
        {
            this.Status = PayPalPaymentStatus.Cancelled;
        }

        public void AddError(PayPalError error)
        {
            this.Errors.Add(error);

            this.Status = PayPalPaymentStatus.Error;
        }
    }

    public class PayPalError
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public PayPalError(string errorCode, string errorMessage)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
        }
    }
}
