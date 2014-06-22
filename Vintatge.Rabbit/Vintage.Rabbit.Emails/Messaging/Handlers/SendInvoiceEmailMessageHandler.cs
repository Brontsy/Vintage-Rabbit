﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Http;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;


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
            var username = "azure_d9400eed0eab0750ed35123aa76a6c1c@azure.com";
            var pswd = "ip7UL1H6poM8ioA";

            var credentials = new NetworkCredential(username, pswd);

            SendGrid.ISendGrid myMessage = new SendGrid.SendGridMessage();

            // Create the email object first, then add the properties.
            //SendGrid myMessage = SendGrid.GetInstance();
            myMessage.AddTo("brontsy@gmail.com");
            myMessage.From = new MailAddress("test@vintagerabbit.com.ai", "Vintage Rabbit");
            myMessage.Subject = "Testing the SendGrid Library";
            myMessage.Html = "<p>TEst</p>";
            myMessage.Text = "Hello World!";


            string url = string.Format("http://dev.vintage-rabbit.com.au/email/invoice/{0}/{1}", message.Order.Guid, message.Order.Id);
            var response = this._httpWebUtility.Get<string>(url, 5000);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                myMessage.Html = response.Body;
                var web = new SendGrid.Web(credentials);

                web.Deliver(myMessage);
            }

        }
    }
}
