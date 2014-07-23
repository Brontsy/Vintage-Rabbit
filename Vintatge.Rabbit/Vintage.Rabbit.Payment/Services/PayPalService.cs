
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
using Vintage.Rabbit.Logging;
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

        PayPalPayment Success(IOrder order, Guid paypalGuid, string token, string payerId);

        void Cancel(IOrder order, Guid paypalGuid, string token);
    }

    internal class PayPalService : IPayPalService
    {
        private string _username;
        private string _password;
        private string _key;
        private string _payPalUrl;
        private string _websiteUrl;
        private bool _isSandbox;

        private IPaymentGateway _paymentGateway;
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;
        private IQueryDispatcher _queryDispatcher;
        private ILogger _logger;

        public PayPalService(IPaymentGateway paymentGateway, ICommandDispatcher commandDispatcher, IMessageService messageService, IQueryDispatcher queryDispatcher, ILogger logger)
        {
            this._username = ConfigurationManager.AppSettings["PayPayl_Username"];
            this._password = ConfigurationManager.AppSettings["PayPayl_Password"];
            this._key = ConfigurationManager.AppSettings["PayPayl_Key"];
            this._payPalUrl = ConfigurationManager.AppSettings["PayPayl_Url"];
            this._websiteUrl = ConfigurationManager.AppSettings["Website_Url"];

            if (ConfigurationManager.AppSettings["PayPal_IsSandbox"] != null)
            {
                this._isSandbox = bool.Parse(ConfigurationManager.AppSettings["PayPal_IsSandbox"]);
            }

            this._paymentGateway = paymentGateway;
            this._commandDispatcher = commandDispatcher;
            this._messageService = messageService;
            this._queryDispatcher = queryDispatcher;
            this._logger = logger;
        }

        public string Checkout(IOrder order)
        {
            try
            {
                Guid paypalPaymentGuid = Guid.NewGuid();

                List<PaymentDetailsItemType> paymentItems = new List<PaymentDetailsItemType>();

                foreach (var orderItem in order.Items)
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

                paymentDetail.PaymentAction = PaymentActionCodeType.SALE;
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

                PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService(this.GetSdkConfig());
                SetExpressCheckoutResponseType setECResponse = service.SetExpressCheckout(wrapper);

                PayPalPayment payment = new PayPalPayment(paypalPaymentGuid, order.Guid, setECResponse.Token, (setECResponse.Ack.HasValue ? setECResponse.Ack.Value.ToString() : null), setECResponse.CorrelationID);

                this._commandDispatcher.Dispatch(new SavePayPalPaymentCommand(payment));
                this._messageService.AddMessage(new PayPalPaymentCreatedMessage(payment));

                return string.Format("{0}?cmd=_express-checkout&token={1}", this._payPalUrl, setECResponse.Token);
            }
            catch(Exception exception)
            {
                this._logger.Error(exception, "Unable to get paypal express checkout url");
            }

            return null;
        }

        public PayPalPayment Success(IOrder order, Guid paypalGuid, string token, string payerId)
        {
            var payPalOrder = this._queryDispatcher.Dispatch<PayPalPayment, GetPayPalPaymentByGuidQuery>(new GetPayPalPaymentByGuidQuery(paypalGuid));
            if (payPalOrder.Token == token && payPalOrder.OrderGuid == order.Guid)
            {
                GetExpressCheckoutDetailsResponseType type = this.GetExpressCheckoutDetails(token);

                if(type.Errors != null && type.Errors.Any())
                {
                    // errors
                    foreach(var error in type.Errors)
                    {
                        payPalOrder.AddError(new PayPalError(error.ErrorCode, error.LongMessage));
                    }
                }
                else
                {
                    payPalOrder = this.CommitPayment(type, payPalOrder, order);
                }

                this._commandDispatcher.Dispatch(new SavePayPalPaymentCommand(payPalOrder));

                if (payPalOrder.Status == Enums.PayPalPaymentStatus.Completed)
                {
                    this._messageService.AddMessage(new PaymentCompleteMessage(order, Enums.PaymentMethod.PayPal));
                }
                else
                {
                    this._messageService.AddMessage(new PayPalErrorMessage(payPalOrder));
                }
            }

            return payPalOrder;
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

        private GetExpressCheckoutDetailsResponseType GetExpressCheckoutDetails(string token)
        {
            GetExpressCheckoutDetailsReq req = new GetExpressCheckoutDetailsReq()
            {
                GetExpressCheckoutDetailsRequest = new GetExpressCheckoutDetailsRequestType()
                {
                    Version = "104.0",
                    Token = token
                }
            };

            var service = new PayPalAPIInterfaceServiceService(this.GetSdkConfig());
            // query PayPal for transaction details
            return service.GetExpressCheckoutDetails(req);
        }

        private PayPalPayment CommitPayment(GetExpressCheckoutDetailsResponseType response, PayPalPayment payPalOrder, IOrder order)
        {
            var service1 = new PayPalAPIInterfaceServiceService(this.GetSdkConfig());

            var total = order.Total.ToString();
            // get transaction details
            //prepare for commiting transaction
            var payReq = new DoExpressCheckoutPaymentReq()
            {
                DoExpressCheckoutPaymentRequest = new DoExpressCheckoutPaymentRequestType()
                {
                    Version = "104.0",
                    DoExpressCheckoutPaymentRequestDetails = new DoExpressCheckoutPaymentRequestDetailsType
                    {
                        Token = response.GetExpressCheckoutDetailsResponseDetails.Token,
                        PaymentAction = PaymentActionCodeType.SALE,
                        PayerID = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerID,
                        PaymentDetails = new List<PaymentDetailsType>
                        {
                            new PaymentDetailsType
                            {
                                OrderTotal = new BasicAmountType
                                {
                                    currencyID = CurrencyCodeType.AUD,
                                    value = total
                                }
                            }
                        }
                    },
                }
            };

            // commit transaction and display results to user
            DoExpressCheckoutPaymentResponseType doResponse = service1.DoExpressCheckoutPayment(payReq);

            if (doResponse.Errors != null && doResponse.Errors.Count > 0)
            {
                // errors
                foreach (var error in doResponse.Errors)
                {
                    payPalOrder.AddError(new PayPalError(error.ErrorCode, error.LongMessage));
                }
            }
            else
            {
                payPalOrder.TransactionId = doResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo[0].TransactionID;
                payPalOrder.Completed();
            }

            return payPalOrder;
        }

        private Dictionary<string, string> GetSdkConfig()
        {
            Dictionary<string, string> sdkConfig = new Dictionary<string, string>();
            
            sdkConfig.Add("account1.apiUsername", this._username);
            sdkConfig.Add("account1.apiPassword", this._password);
            sdkConfig.Add("account1.apiSignature", this._key);

            if(this._isSandbox)
            {
                sdkConfig.Add("mode", "sandbox");
            }

            return sdkConfig;
        }
    }
}
