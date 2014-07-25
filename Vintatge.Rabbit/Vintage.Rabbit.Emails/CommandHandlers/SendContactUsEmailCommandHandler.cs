using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Vintage.Rabbit.Common.Http;
using Vintage.Rabbit.Common.Serialization;
using Vintage.Rabbit.Interfaces.CQRS;

namespace Vintage.Rabbit.Emails.CommandHandlers
{
    public class SendContactUsEmailCommand
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Comments { get; set; }

        public SendContactUsEmailCommand(string name, string email, string comments)
        {
            this.Name = name;
            this.Email = email;
            this.Comments = comments;
        }
    }

    public class SendContactUsEmailCommandHandler : ICommandHandler<SendContactUsEmailCommand>
    {
        private string _username;
        private string _password;
        private string _websiteUrl;

        private IHttpWebUtility _httpWebUtility;
        private ISerializer _serializer;

        public SendContactUsEmailCommandHandler(IHttpWebUtility httpWebUtility, ISerializer serializer)
        {
            this._username = System.Configuration.ConfigurationManager.AppSettings["SendGrid.Username"];
            this._password = System.Configuration.ConfigurationManager.AppSettings["SendGrid.Password"];
            this._websiteUrl = System.Configuration.ConfigurationManager.AppSettings["Website_Url"];

            this._httpWebUtility = httpWebUtility;
            this._serializer = serializer;
        }

        public void Handle(SendContactUsEmailCommand command)
        {
            var credentials = new NetworkCredential(this._username, this._password);

            SendGrid.ISendGrid myMessage = new SendGrid.SendGridMessage();

            myMessage.AddTo("brontsy@gmail.com");
            myMessage.From = new MailAddress("info@vintagerabbit.com.au", "Vintage Rabbit");
            myMessage.Subject = "Vintage Rabbit - Contact Us";

            string url = string.Format("{0}/email/contact-us", this._websiteUrl);
            string json = this._serializer.Serialize(new { command.Name, command.Email, command.Comments });

            var response = this._httpWebUtility.Post<string>(url, 5000, ContentType.Json, json, new Dictionary<HttpRequestHeader,string>());

            if (response.StatusCode == HttpStatusCode.OK)
            {
                myMessage.Html = response.Body;
                var web = new SendGrid.Web(credentials);

                web.Deliver(myMessage);
            }
        }
    }
}