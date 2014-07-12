using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vintage.Rabbit.Web.Models.Hire
{
    public class HireAvailabilityViewModel
    {
        public string Postcode { get; set; }

        public bool IsValidPostcode { get; set; }

        public HireAvailabilityViewModel(string postcode, bool isValidPostcode)
        {
            this.Postcode = postcode;
            this.IsValidPostcode = isValidPostcode;
        }
    }
}