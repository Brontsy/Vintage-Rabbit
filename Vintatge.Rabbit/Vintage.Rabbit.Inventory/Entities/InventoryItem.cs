using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Inventory;
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

        public InventoryItem()
        {
            this.Guid = Guid.NewGuid();
            this.Status = InventoryStatus.Available;
            this.DatesUnavailable = new List<DateTime>();
        }

        internal void Sold()
        {
            this.Status = InventoryStatus.Sold;
        }

        internal void Hired(DateTime startDate, DateTime endDate)
        {
            DateTime currentDate = startDate.Date;
            while(currentDate <= endDate.Date)
            {
                this.DatesUnavailable.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }
        }

        public bool IsAvailable(DateTime startDate, DateTime endDate)
        {
            bool available = false;
            if (this.Status == InventoryStatus.Available)
            {
                available = true;
                DateTime currentDate = startDate.Date;
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
    }
}
