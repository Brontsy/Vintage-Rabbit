using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vintage.Rabbit.Web.Models.Payment
{
    public class BillingInformationViewModel
    {
        public string Address { get; set; }

        public string SuburbCity { get; set; }

        public string Postcode { get; set; }

        public string State { get; set; }

        public string CompanyName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public BillingInformationViewModel()
        {

        }
    }
}