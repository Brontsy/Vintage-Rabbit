using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Parties.Enums;

namespace Vintage.Rabbit.Parties.Entities
{
    public class Party
    {
        public int Id { get; internal set; }

        public Guid Guid { get; internal set; }

        public Guid OrderGuid { get; internal set; }

        public PartyStatus Status { get; internal set; }

        public DateTime PartyDate { get; internal set; }

        public Guid? DropoffAddress { get; internal set; }

        public Guid? PickupAddress { get; internal set; }

        public DateTime DateCreated { get; internal set; }

        public DateTime DateLastModified { get; internal set; }

        public DateTime HireDate
        {
            get
            {
                DateTime date = this.PartyDate;
                while (date.DayOfWeek != DayOfWeek.Friday)
                {
                    date = date.AddDays(-1);
                }

                return date;
            }
        }

        public DateTime ReturnDate
        {
            get
            {
                DateTime date = this.PartyDate;
                while (date.DayOfWeek != DayOfWeek.Monday)
                {
                    date = date.AddDays(1);
                }

                return date;
            }
        }

        public Party() { }

        public Party(IOrder order)
        {
            this.Guid = Guid.NewGuid();
            this.OrderGuid = order.Guid;
            this.Status = PartyStatus.New;
            this.PartyDate = (DateTime)order.Items.First(o => o.Product.Type == ProductType.Hire).Properties["PartyDate"];

            if(order.Items.Any(o => o.Product.Type == ProductType.Delivery && o.Product.Title == "Dropoff Hire Delivery"))
            {
                this.DropoffAddress = order.DeliveryAddressId;
            }

            if(order.Items.Any(o => o.Product.Type == ProductType.Delivery && o.Product.Title == "Pickup Hire Delivery"))
            {
                this.PickupAddress = order.DeliveryAddressId;
            }
        }
    }
}
