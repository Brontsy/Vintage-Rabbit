using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Data;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Orders.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }

        public Guid MemberId { get; private set; }

        public IList<OrderItem> Items { get; private set; }

        public decimal Total
        {
            get { return this.Items.Sum(o => o.Product.Cost); }
        }

        public Order()
        {
            this.Id = Guid.NewGuid();
            this.Items = new List<OrderItem>();
        }

        public Order(Guid memberId) : this()
        {
            this.MemberId = memberId;
        }

        internal void AddProduct(Product product)
        {
            this.Items.Add(new OrderItem(product));
        }

        internal void RemoveProduct(Guid OrderItemId)
        {
            OrderItem OrderItem = this.Items.FirstOrDefault(o => o.Id == OrderItemId);

            if (OrderItem != null)
            {
                this.Items.Remove(OrderItem);
            }
        }

        internal void Clear()
        {
            this.Items = new List<OrderItem>();
        }
    }
}
