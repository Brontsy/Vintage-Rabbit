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

        public string Token { get; set; }

        public string Ack { get; set; }
        
        public string CorrelationID { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateLastModified { get; set; }

        public PayPalPayment() { }

        public PayPalPayment(Guid guid, Guid orderGuid, string token, string ack, string correlationID)
        {
            this.Guid = guid;
            this.OrderGuid = orderGuid;
            this.Token = token;
            this.Ack = ack;
            this.CorrelationID = correlationID;
            this.DateCreated = DateTime.Now;
            this.Status = PayPalPaymentStatus.Initialised;
        }

        public void Completed()
        {
            this.Status = PayPalPaymentStatus.Completed;
        }

        public void Cancelled()
        {
            this.Status = PayPalPaymentStatus.Cancelled;
        }
    }
}
