
using PayPal;
using PayPal.Api.Payments;
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
        private string _payPalUrl;
        private string _payPalClientId;
        private string _payPalSecret;
        private string _websiteUrl;
        private bool _isSandbox;

        private IPaymentGateway _paymentGateway;
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;
        private IQueryDispatcher _queryDispatcher;
        private ILogger _logger;

        public PayPalService(IPaymentGateway paymentGateway, ICommandDispatcher commandDispatcher, IMessageService messageService, IQueryDispatcher queryDispatcher, ILogger logger)
        {
            this._payPalUrl = ConfigurationManager.AppSettings["PayPal_Url"];
            this._payPalClientId = ConfigurationManager.AppSettings["PayPal_ClientId"];
            this._payPalSecret = ConfigurationManager.AppSettings["PayPal_Secret"];
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

                string returnUrl = string.Format("{0}/checkout-{1}/paypal/success/{2}", this._websiteUrl, order.Guid, paypalPaymentGuid);
                string cancelUrl = string.Format("{0}/checkout-{1}/paypal/cancel/{2}", this._websiteUrl, order.Guid, paypalPaymentGuid);

                PayPal.Api.Payments.Payment payment = this.CreatePayment(order, returnUrl, cancelUrl);

                
                PayPalPayment payPalPayment = new PayPalPayment(paypalPaymentGuid, order.Guid, payment.id);

                this._commandDispatcher.Dispatch(new SavePayPalPaymentCommand(payPalPayment));
                this._messageService.AddMessage(new PayPalPaymentCreatedMessage(payPalPayment));

                return payment.links.First(o => o.rel == "approval_url").href;
            }
            catch (Exception exception)
            {
                this._logger.Error(exception, "Unable to get paypal express checkout url");
            }

            return null;
        }

        public PayPalPayment Success(IOrder order, Guid paypalGuid, string token, string payerId)
        {
            var payPalOrder = this._queryDispatcher.Dispatch<PayPalPayment, GetPayPalPaymentByGuidQuery>(new GetPayPalPaymentByGuidQuery(paypalGuid));
            try
            {
                var payment = PayPal.Api.Payments.Payment.Get(this.GetApiContext(), payPalOrder.PayPalId);

                if (payment.state == "created")
                {
                    var result = payment.Execute(this.GetApiContext(), new PaymentExecution() { payer_id = payment.payer.payer_info.payer_id });
                    if (result.state == "approved")
                    {
                        payPalOrder.Completed(payerId);
                        this._commandDispatcher.Dispatch(new SavePayPalPaymentCommand(payPalOrder));

                        if (payPalOrder.Status == Enums.PayPalPaymentStatus.Completed)
                        {
                            this._messageService.AddMessage(new PaymentCompleteMessage(order, Enums.PaymentMethod.PayPal));
                        }
                    }
                    else if (result.state == "failed")
                    {
                        payPalOrder.AddError(new PayPalError(string.Empty, "Sorry there was a problem processing your payment details. Please try again."));
                        this._messageService.AddMessage(new PayPalErrorMessage(payPalOrder));
                    }
                    else if (result.state == "cancelled")
                    {
                        this.Cancel(order, paypalGuid, token);
                    }
                }
            }
            catch (PayPal.Exception.PayPalException exception)
            {
                payPalOrder.AddError(new PayPalError(string.Empty, "Sorry there was a problem processing your payment details. Please try again."));
                this._messageService.AddMessage(new PayPalErrorMessage(payPalOrder));

                this._logger.Error(exception, "PayPal Payment: execute: " + payPalOrder.Guid);
            }
            catch (Exception exception)
            {
                payPalOrder.AddError(new PayPalError(string.Empty, "Sorry there was a problem processing your payment details. Please try again."));
                this._messageService.AddMessage(new PayPalErrorMessage(payPalOrder));

                this._logger.Error(exception, "PayPal Payment: execute: " + payPalOrder.Guid);
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

        public PayPal.Api.Payments.Payment CreatePayment(IOrder order, string returnUrl, string cancelUrl)
        {
            Amount amount = new Amount();
            amount.currency = "AUD";
            amount.total = order.Total.ToString();
            amount.details = new Details();

            RedirectUrls redirectUrls = new RedirectUrls();
            redirectUrls.return_url = returnUrl;
            redirectUrls.cancel_url = cancelUrl;

            Transaction transaction = new Transaction();
            transaction.amount = amount;
            transaction.item_list = new ItemList() { items = new List<Item>() };

            foreach (var orderItem in order.Items)
            {
                Item item = new Item()
                {
                    name = orderItem.Product.Title,
                    price = orderItem.Product.Cost.ToString(),
                    quantity = orderItem.Quantity.ToString(),
                    currency = "AUD"
                };

                transaction.item_list.items.Add(item);
            }

            List<Transaction> transactions = new List<Transaction>() { transaction };

            Payer payer = new Payer() { payment_method = "paypal" };

            PayPal.Api.Payments.Payment pyment = new PayPal.Api.Payments.Payment();
            pyment.intent = "sale";
            pyment.payer = payer;
            pyment.transactions = transactions;
            pyment.redirect_urls = redirectUrls;

            return pyment.Create(this.GetApiContext());
        }
        private APIContext GetApiContext()
        {
            Dictionary<string, string> payPalConfig = new Dictionary<string, string>();

            if (this._isSandbox)
            {
                payPalConfig.Add("mode", "sandbox");
            }

            OAuthTokenCredential tokenCredential = new OAuthTokenCredential(this._payPalClientId, this._payPalSecret, payPalConfig);
            string accessToken = tokenCredential.GetAccessToken();

            APIContext context = new APIContext(accessToken);
            context.Config = payPalConfig;
            return context;
        }
    }
}
