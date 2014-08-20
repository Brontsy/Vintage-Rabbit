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

        public bool OrderContainsBuyItems { get; private set; }

        public BillingAddressViewModel() { }

        public BillingAddressViewModel(Member member, Order order) 
            : this(member, order, null)
        {
        }

        public BillingAddressViewModel(Member member, Order order, Address address)
            :base (address)
        {
            if (address != null && !string.IsNullOrEmpty(address.Email))
            {
                this.Email = address.Email;
            }
            else
            {
                this.Email = member.Email;
            }

            this.OrderContainsBuyItems = order.ContainsBuyProducts() && !order.ContainsHireProducts() && !order.ContainsTheme();
        }
    }
}