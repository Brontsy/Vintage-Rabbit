using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Web.Models.Payment
{
    public class DeliveryAddressViewModel : AddressViewModel
    {
        public bool IsPickup { get; set; }

        public bool IsDropoff { get; set; }

        [Required(ErrorMessage = "Plesae enter a contact phone number")]
        public string PhoneNumber { get; set; }

        public DeliveryAddressViewModel()
        {
        }

        public DeliveryAddressViewModel(Address address)
            :base (address)
        {
            this.PhoneNumber = address.PhoneNumber;
        }
    }
}