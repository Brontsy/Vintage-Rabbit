using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.Enums;

namespace Vintage.Rabbit.Admin.Web.Models.LoyaltyCards
{
    public class LoyaltyCardViewModel
    {
        public Guid Guid { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public decimal? Discount { get; set; }

        [Required]
        public LoyaltyCardType? LoyaltyCardType { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateConsumed { get; set; }

        public LoyaltyCardStatus Status { get; set; }

        public Guid? OrderGuid { get; set; }

        [Required]
        public string Title { get; set; }

        public IList<SelectListItem> LoyaltyCardTypes
        {
            get
            {
                return new List<SelectListItem>() { new SelectListItem() { Text = "DollarAmount", Value = "DollarAmount" }, new SelectListItem() { Text = "Percentage", Value = "Percentage" } };
            }
        }

        public LoyaltyCardViewModel()
        {
            this.Guid = Guid.NewGuid();
        }

        public LoyaltyCardViewModel(LoyaltyCard loyaltyCard)
        {
            this.Guid = loyaltyCard.Guid;
            this.Number = loyaltyCard.Number;
            this.Discount = loyaltyCard.Discount;
            this.LoyaltyCardType = loyaltyCard.LoyaltyCardType;
            this.DateCreated = loyaltyCard.DateCreated;
            this.DateConsumed = loyaltyCard.DateConsumed;
            this.Status = loyaltyCard.Status;
            this.Title = loyaltyCard.Title;
            this.OrderGuid = loyaltyCard.OrderGuid;
        }
    }
}