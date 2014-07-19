using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Membership
{
    public class BillingAddressViewModel : AddressViewModel
    {
        public string Email { get; set; }

        public BillingAddressViewModel() { }

        public BillingAddressViewModel(Address address)
            :base (address)
        {
            this.Email = address.Email;
        }
    }
}