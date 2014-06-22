using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vintage.Rabbit.Web.Areas.Emails.Models
{
    public class AlertViewModel
    {
        public string Heading { get; private set; }

        public string Text { get; private set; }

        public AlertViewModel(string heading, string text)
        {
            this.Heading = heading;
            this.Text = text;
        }
    }
}