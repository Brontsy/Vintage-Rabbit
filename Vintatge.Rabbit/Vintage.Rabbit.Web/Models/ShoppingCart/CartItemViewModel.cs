using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Web.Models.Categories;

namespace Vintage.Rabbit.Web.Models.ShoppingCart
{
    public class CartItemViewModel
    {
        public string Id { get; private set; }

        public string Title { get; private set; }

        public string Key { get; private set; }

        public int Quantity { get; private set; }

        public string Cost { get; private set; }

        public string Total { get; private set; }

        public int ProductId { get; private set; }

        public Guid ProductGuid { get; private set; }

        public bool IsHire { get; private set; }

        public bool IsBuy { get; private set; }

        public bool IsDelivery { get; private set; }

        public IList<CategoryViewModel> Categories { get; private set; }

        public CartItemViewModel(CartItem item)
        {
            this.Id = item.Id.ToString();
            this.Title = item.Product.Title;
            this.Cost = item.Product.Cost.ToString("C2");
            this.Total = item.Total.ToString("C2");
            this.Quantity = item.Quantity;
            this.Key = item.Product.Title.ToUrl();
            this.ProductId = item.Product.Id;
            this.ProductGuid = item.Product.Guid;
            this.IsHire = item.Product.Type == ProductType.Hire;
            this.IsBuy = item.Product.Type == ProductType.Buy;
            this.IsDelivery = item.Product.Type == ProductType.Delivery;
            this.Categories = item.Product.Categories.Select(o => new CategoryViewModel(o)).ToList();
        }
    }
}