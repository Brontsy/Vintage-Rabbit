using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Interfaces.Data;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.Enums;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Orders.Entities
{
    public class Order : IOrder
    {
        public Guid Id { get; private set; }

        public Guid MemberId { get; private set; }

        public Address ShippingAddress { get; private set; }

        public Address BillingAddress { get; private set; }

        public IList<IOrderItem> Items { get; private set; }

        public OrderStatus Status { get; private set; }

        public DateTime DatePaid { get; private set; }

        public DateTime DateCreated { get; private set; }

        public OrderWorkflowStatus? WorkflowStatus { get; private set; }

        public decimal Total
        {
            get { return this.Items.Sum(o => o.Total); }
        }

        public Order()
        {
            this.Id = Guid.NewGuid();
            this.Items = new List<IOrderItem>();
            this.Status = OrderStatus.Initialised;
            this.WorkflowStatus = OrderWorkflowStatus.AwaitingPayment;
            this.DateCreated = DateTime.Now;
        }

        public Order(Guid memberId) : this()
        {
            this.MemberId = memberId;
        }

        internal void AddProduct(CartItem cartItem)
        {
            this.Items.Add(new OrderItem(cartItem));
        }

        internal void AddDelivery(Delivery delivery)
        {
            this.Items.Add(new OrderItem(delivery));
        }

        internal void RemoveProduct(Guid OrderItemId)
        {
            IOrderItem OrderItem = this.Items.FirstOrDefault(o => o.Id == OrderItemId);

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
            this.ShippingAddress = address;
        }

        internal void AddBillingAddress(Address address)
        {
            this.BillingAddress = address;
        }

        internal void Paid()
        {
            this.Status = OrderStatus.Paid;
            this.WorkflowStatus = OrderWorkflowStatus.AwaitingShipment;
            this.DatePaid = DateTime.Now;
        }
    }
}
