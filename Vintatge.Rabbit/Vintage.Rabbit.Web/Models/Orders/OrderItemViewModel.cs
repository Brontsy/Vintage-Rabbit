using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Web.Models.Orders
{
    public class OrderItemViewModel
    {
        public string Title { get; private set; }

        public OrderItemViewModel(IOrderItem orderItem)
        {
            this.Title = orderItem.Product.Title;
        }
    }
}