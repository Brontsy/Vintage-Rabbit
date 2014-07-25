
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Http;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;


namespace Vintage.Rabbit.Emails.Messaging.Handlers
{
    internal class SendInvoiceEmailMessageHandler : IMessageHandler<IOrderPaidMessage>
    {
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;
        private IQueryDispatcher _queryDispatcher;
        private IHttpWebUtility _httpWebUtility;

        public SendInvoiceEmailMessageHandler(IMessageService messageService, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IHttpWebUtility httpWebUtility)
        {
            this._commandDispatcher = commandDispatcher;
            this._messageService = messageService;
            this._queryDispatcher = queryDispatcher;
            this._httpWebUtility = httpWebUtility;
        }

        public void Handle(IOrderPaidMessage message)
        {
            var username = ConfigurationManager.AppSettings["SendGrid.Username"].ToString();
            var pswd = ConfigurationManager.AppSettings["SendGrid.Password"].ToString();

            var credentials = new NetworkCredential(username, pswd);

            SendGrid.ISendGrid myMessage = new SendGrid.SendGridMessage();

            if (message.Order.BillingAddressId.HasValue)
            {
                Address billingAddress = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(message.Order.BillingAddressId.Value));
                if(billingAddress != null && !string.IsNullOrEmpty(billingAddress.Email))
                {
                    // Create the email object first, then add the properties.
                    //SendGrid myMessage = SendGrid.GetInstance();
                    myMessage.AddTo(billingAddress.Email);
                    myMessage.From = new MailAddress("invoices@vintagerabbit.com.au", "Vintage Rabbit");
                    myMessage.Subject = "Vintage Rabbit - Invoice";

                    string url = string.Format("http://vintagerabbit.azurewebsites.net/email/invoice/{0}/{1}", message.Order.Guid, message.Order.Id);
                    var response = this._httpWebUtility.Get<string>(url, 5000);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        myMessage.Html = response.Body;
                        var web = new SendGrid.Web(credentials);

                        web.Deliver(myMessage);
                    }
                }
            }
        }
    }
}
