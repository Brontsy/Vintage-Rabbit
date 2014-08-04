using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Common.Constants;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Products.Helpers;

namespace Vintage.Rabbit.Web.Models.Payment
{
    public class PartyHireInformationViewModel : AddressViewModel
    {
        /// <summary>
        /// Is the party hire items being delivered by Vintage Rabbit
        /// </summary>
        [Display(Name = "I would like the items delivered and picked up ($30 each way)")]
        public bool IsDelivery { get; set; }

        [Required(ErrorMessage = "Plesae enter a contact phone number")]
        public string PhoneNumber { get; set; }

        public string DeliveryCost { get; set; }

        public DateTime? PartyDate { get; set; }

        public PartyHireInformationViewModel() { }

        public PartyHireInformationViewModel(Order order, Vintage.Rabbit.Parties.Entities.Party party)
            :this(null, order, party)
        {
        }

        public PartyHireInformationViewModel(Address address, Order order, Vintage.Rabbit.Parties.Entities.Party party)
            :base(address)
        {
            this.DeliveryCost = Constants.HireDeliveryCost.ToString("C0");

            if (order.Items.Any(o => o.Product.Type == ProductType.Hire))
            {
                this.PartyDate = (DateTime)order.Items.First(o => o.Product.Type == ProductType.Hire).Properties["PartyDate"];
            }
            else if (order.Items.Any(o => o.Product.Type == ProductType.Theme))
            {
                this.PartyDate = (DateTime)order.Items.First(o => o.Product.Type == ProductType.Theme).Properties["PartyDate"];
            }

            this.IsDelivery = order.Items.Any(o => o.Product.Type == ProductType.Delivery && o.Product.Title == "Pickup Hire Delivery") ||
                              order.Items.Any(o => o.Product.Type == ProductType.Delivery && o.Product.Title == "Dropoff Hire Delivery");

            if (address != null)
            {
                this.PhoneNumber = address.PhoneNumber;
            }
        }
    }
}