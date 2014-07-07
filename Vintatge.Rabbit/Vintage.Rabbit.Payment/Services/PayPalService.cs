
using Microsoft.TeamFoundation.Client;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Http;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Payment.CommandHandlers;
using Vintage.Rabbit.Payment.Entities;
using Vintage.Rabbit.Payment.Messaging.Messages;
using Vintage.Rabbit.Payment.QueryHandlers;
using Vintage.Rabbit.Payment.Repository;

namespace Vintage.Rabbit.Payment.Services
{
    public interface IPayPalService
    {
        string Checkout(IOrder order);

        void Success(IOrder order, Guid paypalGuid, string token);

        void Cancel(IOrder order, Guid paypalGuid, string token);
    }

    internal class PayPalService : IPayPalService
    {
        private string _restApiUrl;
        private string _username;
        private string _password;
        private string _key;
        private string _websiteUrl;

        private IPaymentGateway _paymentGateway;
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;
        private IQueryDispatcher _queryDispatcher;

        public PayPalService(IPaymentGateway paymentGateway, ICommandDispatcher commandDispatcher, IMessageService messageService, IQueryDispatcher queryDispatcher)
        {
            this._restApiUrl = ConfigurationManager.AppSettings["PayPal_RestApiUrl"];
            this._username = ConfigurationManager.AppSettings["PayPayl_Username"];
            this._password = ConfigurationManager.AppSettings["PayPayl_Password"];
            this._key = ConfigurationManager.AppSettings["PayPayl_Key"];
            this._websiteUrl = ConfigurationManager.AppSettings["Website_Url"];

            this._paymentGateway = paymentGateway;
            this._commandDispatcher = commandDispatcher;
            this._messageService = messageService;
            this._queryDispatcher = queryDispatcher;
        }

        public void OAuth()
        {
            Dictionary<string, string> sdkConfig = new Dictionary<string, string>();
            sdkConfig.Add("mode", "sandbox");
            //string accessToken = new OAuthTokenCredential("AQkquBDf1zctJOWGKWUEtKXm6qVhueUEMvXO_-MCI4DQQ4-LWvkDLIN2fGsd", "EL1tVxAjhT7cJimnz5-Nsx9k2reTKSVfErNQF-CmrwJgxRtylkGTKlU4RvrX", sdkConfig).GetAccessToken();
        }

        public string Checkout(IOrder order)
        {
            Guid paypalPaymentGuid = Guid.NewGuid();

            List<PaymentDetailsItemType> paymentItems = new List<PaymentDetailsItemType>();

            foreach(var orderItem in order.Items)
            {
                PaymentDetailsItemType paymentItem = new PaymentDetailsItemType();
                paymentItem.Amount = new BasicAmountType(CurrencyCodeType.AUD, orderItem.Product.Cost.ToString());
                paymentItem.Quantity = orderItem.Quantity;
                paymentItem.ItemCategory = ItemCategoryType.PHYSICAL;
                paymentItem.Name = orderItem.Product.Title;

                paymentItems.Add(paymentItem);
            }


            PaymentDetailsType paymentDetail = new PaymentDetailsType();
            paymentDetail.PaymentDetailsItem = paymentItems;

            paymentDetail.PaymentAction = (PaymentActionCodeType)EnumUtils.GetValue("Sale", typeof(PaymentActionCodeType));
            paymentDetail.OrderTotal = new BasicAmountType(CurrencyCodeType.AUD, (order.Total).ToString()); 
            List<PaymentDetailsType> paymentDetails = new List<PaymentDetailsType>();
            paymentDetails.Add(paymentDetail);

            SetExpressCheckoutRequestDetailsType ecDetails = new SetExpressCheckoutRequestDetailsType();
            ecDetails.ReturnURL = string.Format("{0}/checkout-{1}/paypal/success/{2}", this._websiteUrl, order.Guid, paypalPaymentGuid);
            ecDetails.CancelURL = string.Format("{0}/checkout-{1}/paypal/cancel/{2}", this._websiteUrl, order.Guid, paypalPaymentGuid);
            ecDetails.PaymentDetails = paymentDetails;

            SetExpressCheckoutRequestType request = new SetExpressCheckoutRequestType();
            request.Version = "104.0";
            request.SetExpressCheckoutRequestDetails = ecDetails;

            SetExpressCheckoutReq wrapper = new SetExpressCheckoutReq();
            wrapper.SetExpressCheckoutRequest = request;
            Dictionary<string, string> sdkConfig = new Dictionary<string, string>();
            sdkConfig.Add("mode", "sandbox");
            sdkConfig.Add("account1.apiUsername", this._username);
            sdkConfig.Add("account1.apiPassword", this._password);
            sdkConfig.Add("account1.apiSignature", this._key);
            PayPalAPIInterfaceServiceService service = new  PayPalAPIInterfaceServiceService(sdkConfig); 
            SetExpressCheckoutResponseType setECResponse = service.SetExpressCheckout(wrapper);

            PayPalPayment payment = new PayPalPayment(paypalPaymentGuid, order.Guid, setECResponse.Token, (setECResponse.Ack.HasValue ? setECResponse.Ack.Value.ToString() : null), setECResponse.CorrelationID);

            this._commandDispatcher.Dispatch(new SavePayPalPaymentCommand(payment));

            return string.Format("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token={0}", setECResponse.Token);
        }

        public void Success(IOrder order, Guid paypalGuid, string token)
        {
            var payPalOrder = this._queryDispatcher.Dispatch<PayPalPayment, GetPayPalPaymentByGuidQuery>(new GetPayPalPaymentByGuidQuery(paypalGuid));
            if (payPalOrder.Token == token && payPalOrder.OrderGuid == order.Guid)
            {
                payPalOrder.Completed();
                this._commandDispatcher.Dispatch(new SavePayPalPaymentCommand(payPalOrder));

                this._messageService.AddMessage(new PaymentCompleteMessage(order));
            }
        }

        public void Cancel(IOrder order, Guid paypalGuid, string token)
        {
            var payPalOrder = this._queryDispatcher.Dispatch<PayPalPayment, GetPayPalPaymentByGuidQuery>(new GetPayPalPaymentByGuidQuery(paypalGuid));
            if (payPalOrder.Token == token && payPalOrder.OrderGuid == order.Guid)
            {
                payPalOrder.Cancelled();
                this._commandDispatcher.Dispatch(new SavePayPalPaymentCommand(payPalOrder));
            }
        }
    }
}
