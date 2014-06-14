using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Web.Models.Orders
{
    public class OrderViewModel
    {
        public IList<OrderItemViewModel> OrderItems { get; private set; }

        public string Total { get; private set; }
        public OrderViewModel(Order order)
        {
            this.OrderItems = order.Items.Select(o => new OrderItemViewModel(o)).ToList();
            this.Total = order.Total.ToString("C2");
        }
    }
}