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
        }

        public bool IsAvailable(DateTime startDate, DateTime endDate)
        {
            DateTime currentDate = startDate.Date;
            while(currentDate <= endDate.Date)
            {
                if(this.DatesUnavailable.Contains(currentDate))
                {
                    return false;
                }

                currentDate = currentDate.AddDays(1);
            }

            return true;
        }
    }
}
