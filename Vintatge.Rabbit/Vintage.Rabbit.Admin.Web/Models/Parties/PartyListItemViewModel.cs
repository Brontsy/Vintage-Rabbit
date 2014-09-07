using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Admin.Web.Models.Membership;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Parties.Enums;

namespace Vintage.Rabbit.Admin.Web.Models.Parties
{
    public class PartyListItemViewModel
    {
        public Guid Guid { get; internal set; }

        public Guid OrderGuid { get; internal set; }

        public DateTime PartyDate { get; internal set; }

        public DateTime HireDate { get; internal set; }

        public DateTime ReturnDate { get; internal set; }

        public bool IsDelivery { get; internal set; }

        public bool IsPickup { get; internal set; }

        public string Name { get; internal set; }

        public DeliveryAddressViewModel DropoffAddress { get; internal set; }

        public DeliveryAddressViewModel PickupAddress { get; internal set; }

        public PartyListItemViewModel(Party party, string name)
        {
            this.Guid = party.Guid;
            this.OrderGuid = party.OrderGuid;
            this.PartyDate = party.PartyDate;
            this.HireDate = party.HireDate;
            this.ReturnDate = party.ReturnDate;
            this.Name = name;
            this.IsDelivery = party.DropoffAddress.HasValue;
            this.IsPickup = party.PickupAddress.HasValue;
        }
    }
}