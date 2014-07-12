using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Common.Constants;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Web.Models.Payment
{
    public class DeliveryAddressViewModel : AddressViewModel
    {
        [Display(Name = "Vintage Rabbit picks up the items ($30)")]
        public bool IsPickup { get; set; }

        [Display(Name = "Vintage Rabbit delivers the items ($30)")]
        public bool IsDropoff { get; set; }

        [Required(ErrorMessage = "Plesae enter a contact phone number")]
        public string PhoneNumber { get; set; }

        public string DeliveryCost { get; set; }

        public DeliveryAddressViewModel()
        {
            this.DeliveryCost = Constants.HireDeliveryCost.ToString("C0");
        }

        public DeliveryAddressViewModel(Address address, Order order)
            : base(address)
        {
            this.PhoneNumber = address.PhoneNumber;
            this.DeliveryCost = Constants.HireDeliveryCost.ToString("C0");

            this.IsPickup = order.Items.Any(o => o.Product.Type == Common.Enums.ProductType.Delivery && o.Product.Title == "Pickup Hire Delivery");
            this.IsDropoff = order.Items.Any(o => o.Product.Type == Common.Enums.ProductType.Delivery && o.Product.Title == "Dropoff Hire Delivery");
        }
    }
}