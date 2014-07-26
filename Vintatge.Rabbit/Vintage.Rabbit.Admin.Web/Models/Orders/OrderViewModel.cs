using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Admin.Web.Models.Membership;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.Enums;

namespace Vintage.Rabbit.Admin.Web.Models.Orders
{
    public class OrderViewModel
    {
        public int Id { get; private set; }

        public Guid Guid { get; private set; }

        public MemberViewModel Member { get; private set; }

        public IList<OrderItemViewModel> OrderItems { get; private set; }

        public OrderStatus Status { get; private set; }

        public DateTime? DatePaid { get; private set; }

        public DateTime DateCreated { get; private set; }

        public BillingAddressViewModel BillingAddress { get; private set; }

        public AddressViewModel ShippingAddress { get; private set; }

        public DeliveryAddressViewModel DeliveryAddress { get; private set; }

        public string Total { get; private set; }
        
        public bool IsPickup { get; set; }

        public bool IsDropoff { get; set; }


        public OrderViewModel(Order order, Member member)
        {
            this.OrderItems = order.Items.Select(o => new OrderItemViewModel(o)).ToList();
            this.Total = order.Total.ToString("C2");
            this.Id = order.Id;
            this.Guid = order.Guid;
            this.Status = order.Status;
            this.DatePaid = order.DatePaid;
            this.DateCreated = order.DateCreated;
            this.Member = new MemberViewModel(member);

            if (order.BillingAddressId.HasValue && member.BillingAddresses.Any(o => o.Guid == order.BillingAddressId.Value))
            {
                var address = member.BillingAddresses.First(o => o.Guid == order.BillingAddressId.Value);
                this.BillingAddress = new BillingAddressViewModel(address);
            }

            if (order.ShippingAddressId.HasValue && member.ShippingAddresses.Any(o => o.Guid == order.ShippingAddressId.Value))
            {
                var address = member.ShippingAddresses.First(o => o.Guid == order.ShippingAddressId.Value);
                this.ShippingAddress = new AddressViewModel(address);
            }

            if (order.DeliveryAddressId.HasValue && member.DeliveryAddresses.Any(o => o.Guid == order.DeliveryAddressId.Value))
            {
                var address = member.DeliveryAddresses.First(o => o.Guid == order.DeliveryAddressId.Value);
                this.DeliveryAddress = new DeliveryAddressViewModel(address);
            }

            this.IsPickup = order.Items.Any(o => o.Product.Type == Common.Enums.ProductType.Delivery && o.Product.Title == "Pickup Hire Delivery");
            this.IsDropoff = order.Items.Any(o => o.Product.Type == Common.Enums.ProductType.Delivery && o.Product.Title == "Dropoff Hire Delivery");
        }
    }
}