using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Inventory.Enums;

namespace Vintage.Rabbit.Admin.Web.Models.Inventory
{
    public class InventoryItemViewModel
    {
        public Guid Guid { get; internal set; }

        public Guid ProductGuid { get; internal set; }

        public InventoryStatus Status { get; internal set; }

        public DateTime DateCreated { get; internal set; }

        internal IList<DateTime> DatesUnavailable { get; set; }

        public InventoryItemViewModel(InventoryItem inventory)
        {
            this.Guid = inventory.Guid;
            this.ProductGuid = inventory.ProductGuid;
            this.Status = inventory.Status;
            this.DateCreated = inventory.DateCreated;
            this.DatesUnavailable = inventory.DatesUnavailable;
        }
    }
}