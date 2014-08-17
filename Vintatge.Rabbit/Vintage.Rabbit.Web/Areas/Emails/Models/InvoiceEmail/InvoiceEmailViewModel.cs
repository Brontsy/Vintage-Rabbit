using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Interfaces.Products;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Web.Models.Products;

namespace Vintage.Rabbit.Web.Areas.Emails.Models.InvoiceEmail
{
    public class InvoiceEmailViewModel
    {
        public string Total { get; private set; }

        public IList<OrderItemViewModel> OrderItems { get; private set; }

        public string PaymentMethod { get; private set; }

        public InvoiceEmailViewModel(Order order)
        {
            this.Total = order.Total.ToString("C");
            this.OrderItems = order.Items.Select(o => new OrderItemViewModel(o)).ToList();
            this.PaymentMethod = (order.PaymentMethod == Payment.Enums.PaymentMethod.PayPal ? "PayPal" : "credit card");
        }
    }

    public class OrderItemViewModel
    {

        public string Id { get; private set; }

        public string Title { get; private set; }

        public string Key { get; private set; }

        public int Quantity { get; private set; }

        public string Cost { get; private set; }

        public string Total { get; private set; }

        public Guid ProductGuid { get; private set; }

        public int ProductId { get; private set; }

        public bool IsHire { get; private set; }

        public bool IsBuy { get; private set; }

        public bool IsTheme { get; private set; }

        public bool IsDelivery { get; private set; }

        public bool IsDiscount { get; private set; }

        public ProductType Type { get; private set; }

        public string Thumbnail { get; private set; }

        public string Description { get; private set; }

        public OrderItemViewModel(IOrderItem orderItem)
        {
            this.Id = orderItem.Guid.ToString();
            this.Title = orderItem.Product.Title;
            this.Cost = orderItem.Product.Cost.ToString("C2");
            this.Total = orderItem.Total.ToString("C2");
            this.Quantity = orderItem.Quantity;
            this.Key = orderItem.Product.Title.ToUrl();
            this.ProductGuid = orderItem.Product.Guid;
            this.IsHire = orderItem.Product.Type == ProductType.Hire;
            this.IsBuy = orderItem.Product.Type == ProductType.Buy;
            this.IsDelivery = orderItem.Product.Type == ProductType.Delivery;
            this.IsDiscount = orderItem.Product.Type == ProductType.Discount;
            this.IsTheme = orderItem.Product.Type == ProductType.Theme;
            this.Type = orderItem.Product.Type;

            if (orderItem.Product is Product)
            {
                Product product = orderItem.Product as Product;
                this.Description = product.Description;
                if (product.Images.Any())
                {
                    this.Thumbnail = product.Images.First().SecureThumbnail;
                }
            }

            if (orderItem.Product is Theme)
            {
                this.Thumbnail = (orderItem.Product as Theme).Images.First().ThumbnailUrl;
                this.Description = (orderItem.Product as Theme).Description;
            }
        }
    }
}