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

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy}")]
        public DateTime PartyDate { get; set; }

        //[Required(ErrorMessage = "Please enter childs name")]
        public string ChildsName { get; set; }

        //[Required(ErrorMessage = "Please enter the age of the child")]
        public string Age { get; set; }

        //[Required(ErrorMessage = "Please enter the time and date of the party")]
        public string PartyTime { get; set; }

        //[Required(ErrorMessage = "Please enter the address of the party")]
        public string PartyAddress { get; set; }

        public string RSVPDetails { get; set; }

        public bool OrderContainsCustomInvitations { get; set; }

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
            else
            {
                // TODO: HANDLE PROBLEM BECAUSE WE DO NOT HAVE A PARTY DATE
            }

            this.IsDelivery = order.Items.Any(o => o.Product.Type == Common.Enums.ProductType.Delivery && o.Product.Title == "Pickup Hire Delivery") ||
                              order.Items.Any(o => o.Product.Type == Common.Enums.ProductType.Delivery && o.Product.Title == "Dropoff Hire Delivery");

            if (address != null)
            {
                this.PhoneNumber = address.PhoneNumber;
            }

            if(party != null)
            {
                this.ChildsName = party.ChildsName;
                this.Age = party.Age;
                this.PartyTime = party.PartyTime;
                this.PartyAddress = party.PartyAddress;
                this.RSVPDetails = party.RSVPDetails;
            }
        }
    }
}