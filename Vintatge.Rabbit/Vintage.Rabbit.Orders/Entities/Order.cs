using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Data;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.Enums;
using Vintage.Rabbit.Payment.Enums;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Orders.Entities
{
    public class Order : IOrder
    {
        public int Id { get; internal set; }

        public Guid Guid { get; internal set; }

        public Guid MemberGuid { get; internal set; }

        public PaymentMethod PaymentMethod { get; internal set; }

        public Guid? ShippingAddressId { get; internal set; }

        public Guid? BillingAddressId { get; internal set; }

        public Guid? DeliveryAddressId { get; internal set; }

        public IList<IOrderItem> Items { get; internal set; }

        public OrderStatus Status { get; internal set; }

        public DateTime? DatePaid { get; internal set; }

        public DateTime DateCreated { get; internal set; }

        public decimal Total
        {
            get { return this.Items.Sum(o => o.Total); }
        }

        public Order()
        {
            this.Guid = Guid.NewGuid();
            this.Items = new List<IOrderItem>();
            this.Status = OrderStatus.Unpaid;
            this.DateCreated = DateTime.Now;
        }

        public Order(Guid memberId) : this()
        {
            this.MemberGuid = memberId;
        }

        internal void AddProduct(CartItem cartItem)
        {
            this.Items.Add(new OrderItem(cartItem));
        }

        internal void AddDelivery(Delivery delivery)
        {
            this.Items.Add(new OrderItem(delivery));
        }

        internal void AddloyaltyCard(LoyaltyCard loyaltyCard)
        {
            if(this.Items.Any(o => o.Product.Type == Common.Enums.ProductType.Discount))
            {
                this.Items.Remove(this.Items.First(o => o.Product.Type == Common.Enums.ProductType.Discount));
            }

            this.Items.Add(new OrderItem(loyaltyCard));
        }

        internal void RemoveProduct(Guid OrderItemId)
        {
            IOrderItem OrderItem = this.Items.FirstOrDefault(o => o.Guid == OrderItemId);

            if (OrderItem != null)
            {
                this.Items.Remove(OrderItem);
            }
        }

        internal void Clear()
        {
            this.Items = new List<IOrderItem>();
        }

        internal void AddShippingAddress(Address address)
        {
            this.ShippingAddressId = address.Guid;
        }

        internal void AddBillingAddress(Address address)
        {
            this.BillingAddressId = address.Guid;
        }

        internal void AddDeliveryAddress(Address address)
        {
            this.DeliveryAddressId = address.Guid;
        }

        internal void RemoveDeliveryAddress()
        {
            this.DeliveryAddressId = null;
        }

        internal void Paid(PaymentMethod paymentMethod)
        {
            this.Status = this.ContainsHireProducts() ? OrderStatus.Complete : OrderStatus.AwaitingShipment;
            this.DatePaid = DateTime.Now;
            this.PaymentMethod = paymentMethod;
        }

        internal void Error()
        {
            this.Status = OrderStatus.Error;
        }

        public bool ContainsBuyProducts()
        {
            return this.Items.Any(o => o.Product.Type == Common.Enums.ProductType.Buy);
        }

        public bool ContainsHireProducts()
        {
            return this.Items.Any(o => o.Product.Type == Common.Enums.ProductType.Hire);
        }
    }
}
