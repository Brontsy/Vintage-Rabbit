﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Parties.Enums;
using Vintage.Rabbit.Web.Models.Payment;

namespace Vintage.Rabbit.Web.Models.Party
{
    public class PartyViewModel
    {

        public Guid Guid { get; internal set; }

        public Guid OrderGuid { get; internal set; }

        public PartyStatus Status { get; internal set; }

        public DateTime PartyDate { get; internal set; }

        public DeliveryAddressViewModel DropoffAddress { get; internal set; }

        public DeliveryAddressViewModel PickupAddress { get; internal set; }

        public DateTime HireDate { get; internal set; }

        public DateTime ReturnDate { get; internal set; }

        public PartyViewModel(Vintage.Rabbit.Parties.Entities.Party party)
        {
            this.Guid = party.Guid;
            this.OrderGuid = party.OrderGuid;
            this.Status = party.Status;
            this.PartyDate = party.PartyDate;
            this.HireDate = party.HireDate;
            this.ReturnDate = party.ReturnDate;
        }
    }
}