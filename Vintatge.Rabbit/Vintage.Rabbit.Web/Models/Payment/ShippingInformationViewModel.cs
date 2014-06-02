using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Web.Models.Payment
{
    public class ShippingInformationViewModel
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

        [Display(Name = "Is your billing address the same as your shipping address?")]
        public bool BillingAddressIsTheSame { get; set; }

        public ShippingInformationViewModel()
        {
            this.BillingAddressIsTheSame = true;
        }

        public ShippingInformationViewModel(Order order)
        {
            if (order != null && order.ShippingAddress != null)
            {
                this.FirstName = order.ShippingAddress.FirstName;
                this.LastName = order.ShippingAddress.LastName;
                this.CompanyName = order.ShippingAddress.CompanyName;
                this.Address = order.ShippingAddress.Line1;
                this.SuburbCity = order.ShippingAddress.SuburbCity;
                this.Postcode = order.ShippingAddress.Postcode;
                this.State = order.ShippingAddress.State;
            }

            this.BillingAddressIsTheSame = true;
        }
    }
}