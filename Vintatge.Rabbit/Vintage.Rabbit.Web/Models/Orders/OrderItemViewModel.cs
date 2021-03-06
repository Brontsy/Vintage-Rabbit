﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Web.Models.Categories;
using Vintage.Rabbit.Interfaces.Products;
using Vintage.Rabbit.Themes.Entities;

namespace Vintage.Rabbit.Web.Models.Orders
{
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

            if (orderItem.Product is IProduct)
            {
                IProduct product = orderItem.Product as IProduct;

                if (product.Images.Any())
                {
                    this.Thumbnail = product.Images.First().SecureThumbnail;
                }
            }

            if (orderItem.Product is Theme)
            {
                this.Thumbnail = (orderItem.Product as Theme).Images.First().ThumbnailUrl;
            }

            if (orderItem.Product is LoyaltyCard)
            {
                var loyaltyCard = orderItem.Product as LoyaltyCard;
                if(loyaltyCard.LoyaltyCardType == Rabbit.Orders.Enums.LoyaltyCardType.Percentage)
                {
                    this.Cost = loyaltyCard.Discount.ToString("0") + "%";
                }
            }
        }
    }
}