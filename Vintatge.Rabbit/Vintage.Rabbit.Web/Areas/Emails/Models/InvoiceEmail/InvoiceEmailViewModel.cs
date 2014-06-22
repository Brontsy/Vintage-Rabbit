using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Web.Models.Products;

namespace Vintage.Rabbit.Web.Areas.Emails.Models.InvoiceEmail
{
    public class InvoiceEmailViewModel
    {
        public string Total { get; private set; }

        public IList<OrderItemViewModel> OrderItems { get; private set; }

        public InvoiceEmailViewModel(Order order)
        {
            this.Total = order.Total.ToString("C");
            this.OrderItems = new List<OrderItemViewModel>();
        }
    }

    public class OrderItemViewModel
    {
        public string Title { get; private set; }

        public int Quantity { get; private set; }

        public string Total { get; private set; }

        public string Thumbnail { get; private set; }

        public string Description { get; private set; }

        public Guid ProductGuid { get; private set; }

        public OrderItemViewModel(IOrderItem orderItem, Product product)
        {
            this.Title = orderItem.Product.Title;
            this.Quantity = orderItem.Quantity;
            this.Total = orderItem.Total.ToString("C");
            this.Thumbnail = product.Images.First().Thumbnail;
            this.ProductGuid = product.Guid;
            this.Description = (product.Description.Length > 120 ? product.Description.Substring(0, 120) : product.Description);
        }
    }
}