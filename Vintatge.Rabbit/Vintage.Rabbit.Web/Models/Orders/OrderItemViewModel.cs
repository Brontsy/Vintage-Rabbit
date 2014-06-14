using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Web.Models.Categories;

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

        public IList<CategoryViewModel> Categories { get; private set; }

        public OrderItemViewModel(IOrderItem orderItem)
        {
            this.Id = orderItem.Id.ToString();
            this.Title = orderItem.Product.Title;
            this.Cost = orderItem.Product.Cost.ToString("C2");
            this.Total = orderItem.Total.ToString("C2");
            this.Quantity = orderItem.Quantity;
            this.Key = orderItem.Product.Title.ToUrl();
            this.ProductGuid = orderItem.Product.Guid;
            this.ProductId = orderItem.Product.Id;
            this.IsHire = orderItem.Product.Type == ProductType.Hire;
            //this.Categories = orderItem.Product.Categories.Select(o => new CategoryViewModel(o)).ToList();
        }
    }
}