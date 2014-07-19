using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Orders.Enums;

namespace Vintage.Rabbit.Admin.Web.Models.Orders
{
    public class OrdersPageViewModel
    {
        public IList<OrderViewModel> Orders { get; private set; }

        public OrderStatus SelectedStatus { get; private set; }

        public OrdersPageViewModel(IList<OrderViewModel> orders, OrderStatus status)
        {
            this.Orders = orders;
            this.SelectedStatus = status;
        }
    }
}