using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Web.Models.Payment
{
    public class AddressViewModel
    {
        [Display(Name = "Address")]
        [Required(ErrorMessage = "Please enter your address")]
        public string Address { get; set; }

        [Display(Name = "Suburb / City")]
        [Required(ErrorMessage = "Please enter your suburb / city")]
        public string SuburbCity { get; set; }

        [Display(Name = "Postcode")]
        [Required(ErrorMessage = "Please enter your postcode")]
        public int? Postcode { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "Please select your state")]
        public string State { get; set; }

        [Display(Name = "Company name")]
        public string CompanyName { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        public AddressViewModel()
        {
        }

        public AddressViewModel(Address address)
        {
            if (address != null)
            {
                this.FirstName = address.FirstName;
                this.LastName = address.LastName;
                this.CompanyName = address.CompanyName;
                this.Address = address.Line1;
                this.SuburbCity = address.SuburbCity;
                this.Postcode = address.Postcode;
                this.State = address.State;
            }
        }
    }
}