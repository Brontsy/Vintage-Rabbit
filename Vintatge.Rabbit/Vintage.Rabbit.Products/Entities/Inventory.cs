using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Products.Enums;

namespace Vintage.Rabbit.Products.Entities
{
    public class Inventory
    {
        public Guid Id { get; internal set; }

        public Guid ProductGuid { get; internal set; }

        public InventoryStatus Status { get; internal set; }

        internal IList<DateTime> DatesUnavailable { get; set; }

        public Inventory()
        {
            this.Id = Guid.NewGuid();
            this.Status = InventoryStatus.Available;
            this.DatesUnavailable = new List<DateTime>();
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
    }
}
