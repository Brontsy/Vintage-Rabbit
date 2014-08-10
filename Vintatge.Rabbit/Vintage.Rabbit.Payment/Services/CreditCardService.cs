using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Http;
using Vintage.Rabbit.Common.Serialization;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Logging;
using Vintage.Rabbit.Payment.CommandHandlers;
using Vintage.Rabbit.Payment.Entities;
using Vintage.Rabbit.Payment.Entities.Eway;
using Vintage.Rabbit.Payment.Enums;
using Vintage.Rabbit.Payment.Messaging.Messages;

namespace Vintage.Rabbit.Payment.Services
{
    public interface ICreditCardService
    {
        AccessCodeResponse GetEwayAccessCode(IOrder order);

        PaymentResult CompletePayment(IOrder order, string accessCode);
    }

    internal class CreditCardService : ICreditCardService
    {
        private IPaymentGateway _paymentGateway;
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;
        private IHttpWebUtility _httpWebUtility;
        private ISerializer _serializer;
        private ILogger _logger;

        private string _ewayUrl;
        private string _authorisation;
        private string _websiteUrl;

        public CreditCardService(IPaymentGateway paymentGateway, ICommandDispatcher commandDispatcher, IMessageService messageService, IHttpWebUtility httpWebUtility, ISerializer serializer, ILogger logger)
        {
            this._paymentGateway = paymentGateway;
            this._commandDispatcher = commandDispatcher;
            this._messageService = messageService;
            this._httpWebUtility = httpWebUtility;
            this._serializer = serializer;
            this._logger = logger;

            this._ewayUrl = ConfigurationManager.AppSettings["Eway_Url"];
            this._authorisation = ConfigurationManager.AppSettings["Eway_Auth"];
            this._websiteUrl = ConfigurationManager.AppSettings["Website_Url"];
        }

        public AccessCodeResponse GetEwayAccessCode(IOrder order)
        {
            string url = string.Format("{0}/AccessCodes", this._ewayUrl);
            AccessCodeRequest request = new AccessCodeRequest(order);
            request.RedirectUrl = string.Format("{0}/checkout-{1}/credit-card/complete", this._websiteUrl, order.Guid);

            var headers = new Dictionary<HttpRequestHeader,string>();
            headers.Add(HttpRequestHeader.Authorization, this._authorisation);

            var response = this._httpWebUtility.Post<AccessCodeResponse>(url, 3000, ContentType.Json, _serializer.Serialize(request), headers);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                return response.Response;
            }
            else
            {
                this._logger.Log(LogType.Error, "unable to get access code: " + response.StatusCode.ToString());
            }

            return null;
        }

        public PaymentResult CompletePayment(IOrder order, string accessCode)
        {
            string url = string.Format("{0}/AccessCode/{1}", this._ewayUrl, accessCode);
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.Authorization, this._authorisation);

            var response = this._httpWebUtility.Get<EwayPaymentResponse>(url, 3000, headers);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                this._commandDispatcher.Dispatch(new EwayPaymentProcessedCommand(order, response.Response));

                if (response.Response.ResponseCode == "00")
                {
                    this._messageService.AddMessage(new PaymentCompleteMessage(order, PaymentMethod.CreditCard));

                    return PaymentResult.Success();
                }

                return PaymentResult.Error(this.GetError(response.Response));
            }
            else
            {
                this._logger.Log(LogType.Error, "unable to complete payment: " + response.StatusCode.ToString());
            }

            return PaymentResult.Error("Sorry there was a problem processing your payment details. Please try again.");
        }

        public string GetError(EwayPaymentResponse response)
        {
            switch(response.ResponseCode)
            {

                case "01"://	Refer to Issuer	Fail
                case "02"://		Refer to Issuer, special	Fail
                    return "It looks like there is a problem with your credit card. Please contact your bank or use a different credit card.";

                case "04"://		Pick Up Card	Fail
                case "07"://		Pick Up Card, Special	Fail
                case "41"://		Lost Card	Fail
                    return "Sorry we cannot process your payment with that credit, please try a different card.";

                case "05"://		Do Not Honour	Fail
                    return "Your transaction has been declined. Please contact your bank or use a different credit card.";

                 case "14"://		Invalid Card Number	Fail
                                    return "The credit card number you have provided is invalid. Please try again.";

                 case "15"://		No Issuer	Fail
                                    return "Sorry there was a problem processing your payment details. Please try again";

                 case "51"://		Insufficient Funds	Fail
                     return "Your transaction has not completed due to insufficient funds.";

                 case "33"://		Expired Card, Capture	Fail
                 case "54"://		Expired Card	Fail
                     return "It looks like your credit card has expired. Please try a different card.";

                default:
                    return "Sorry there was a problem processing your payment details. Please try again.";
            }
        }
    }
}
