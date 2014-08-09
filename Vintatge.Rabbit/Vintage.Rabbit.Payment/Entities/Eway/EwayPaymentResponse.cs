using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Payment.Entities.Eway
{
    public class EwayPaymentResponse
    {
        public string AccessCode { get; set; }

        public string AuthorisationCode { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseMessage { get; set; }

        public string InvoiceNumber { get; set; }

        public int TotalAmount { get; set; }

        public string TransactionID { get; set; }

        public string TransactionStatus { get; set; }

        public string Errors { get; set; }
    }
}

// "AccessCode": "44DD7CgzOhVD1JiaBtmyeKFJFDEP6MORwP4Dfyc-c-eyu8tT4-
//ZXczJTWWE0hLX8gqMQyVv43VeF7wxt8XMP7pHsVSZL6EN_IuUvkUH8Gd8XdPIuhWxMSOgQmWZ_AjGptB1rc
//j8K0gajegLbkvjrSt0dqAQ==", 
// "AuthorisationCode": "000000",
// "ResponseCode": "58", 
// "ResponseMessage": "D4458", 
// "InvoiceNumber": "Inv 21540", 
// "InvoiceReference": "513456", 
// "TotalAmount": 100, 
// "TransactionID": 92979635, 
// "TransactionStatus": false, 
// "TokenCustomerID": null, 
// "BeagleScore": null, 
// "Options": [ 
//   {"Value": "Option1"}, 
//   {"Value": "Option2"}, 
//   {"Value": "Option3"} 
// ], 
// "Verification": { 
//   "CVN": 0, 
//   "Address": 0, 
//   "Email": 0, 
//   "Mobile": 0, 
//   "Phone": 0 
// }, 
// "BeagleVerification": { 
//   "Email": 0, 
//   "Phone": 0 
// }, 
// "Errors": null 