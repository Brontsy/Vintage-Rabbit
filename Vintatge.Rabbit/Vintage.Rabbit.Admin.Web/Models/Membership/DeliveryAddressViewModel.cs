using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Common.Constants;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Membership
{
    public class DeliveryAddressViewModel : AddressViewModel
    {
        [Required(ErrorMessage = "Plesae enter a contact phone number")]
        public string PhoneNumber { get; set; }


        public DeliveryAddressViewModel()
        {

        }

        public DeliveryAddressViewModel(Address address)
            : base(address)
        {
            this.PhoneNumber = address.PhoneNumber;
        }
    }
}