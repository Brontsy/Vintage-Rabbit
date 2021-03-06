﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Inventory.Enums;

namespace Vintage.Rabbit.Inventory.Entities
{
    public class InventoryItem : IInventoryItem
    {
        public int  Id { get; internal set; }

        public Guid Guid { get; internal set; }

        public Guid ProductGuid { get; internal set; }

        public InventoryStatus Status { get; internal set; }

        public DateTime DateCreated { get; internal set; }

        public IList<DateTime> DatesUnavailable { get; set; }

        public DateTime? DateSold { get; set; }

        public Guid? OrderItemGuid { get; set; }

        public InventoryItem()
        {
            this.Guid = Guid.NewGuid();
            this.Status = InventoryStatus.Available;
            this.DatesUnavailable = new List<DateTime>();
        }

        internal void Sold(IOrderItem orderItem)
        {
            this.Status = InventoryStatus.Sold;
            this.DateSold = DateTime.Now;
            this.OrderItemGuid = orderItem.Guid;
        }

        internal void Hired(DateTime partyDate)
        {
            DateTime currentDate = this.GetHireStartDate(partyDate).Date;
            DateTime endDate = this.GetHireEndDate(partyDate);
            while(currentDate <= endDate.Date)
            {
                this.DatesUnavailable.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }
        }

        public bool IsAvailable(DateTime partyDate)
        {
            bool available = false;
            if (this.Status == InventoryStatus.Available)
            {
                available = true;

                DateTime endDate = this.GetHireEndDate(partyDate);
                DateTime currentDate = this.GetHireStartDate(partyDate).Date;

                while (currentDate <= endDate.Date)
                {
                    if (this.DatesUnavailable.Contains(currentDate))
                    {
                        available = false;
                    }

                    currentDate = currentDate.AddDays(1);
                }
            }

            return available;
        }

        public bool IsAvailable()
        {
            return this.Status == InventoryStatus.Available;
        }

        public void Deleted()
        {
            this.Status = InventoryStatus.Deleted;
        }
        private DateTime GetHireStartDate(DateTime partyDate)
        {
            DateTime date = partyDate;
            while (date.DayOfWeek != DayOfWeek.Friday)
            {
                date = date.AddDays(-1);
            }

            return date;
        }

        private DateTime GetHireEndDate(DateTime partyDate)
        {
            DateTime date = partyDate;
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(1);
            }

            return date;
        }
    }
}
