using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Web.Attributes.Validation;

namespace Vintage.Rabbit.Web.Models.Payment
{
    public class BillingAddressViewModel : AddressViewModel
    {
        [Required(ErrorMessage = "Please enter a email address")]
        [Email(ErrorMessage = "Plesae enter a valid email address")]
        [Display(Name = "Please enter your email below.")]
        public string Email { get; set; }

        public BillingAddressViewModel() { }

        public BillingAddressViewModel(Member member)
        {
            if (member != null)
            {
                this.Email = member.Email;
            }
        }

        public BillingAddressViewModel(Address address)
            :base (address)
        {
            if (address != null)
            {
                this.Email = address.Email;
            }
        }
    }
}