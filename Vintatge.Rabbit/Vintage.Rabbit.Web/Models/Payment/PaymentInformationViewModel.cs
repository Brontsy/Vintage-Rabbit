using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Payment.Enums;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Web.Attributes.Validation;

namespace Vintage.Rabbit.Web.Models.Payment
{
    public class PaymentInformationViewModel
    {
        [Display(Name = "Name of card")]
        [Required(ErrorMessage = "Please enter the name on your credit card")]
        public string Name { get; set; }

        [Display(Name = "Credit card number")]
        [Required(ErrorMessage = "Please enter your credit card number")]
        public string CreditCardNumber { get; set; }

        [Display(Name = "Expiry Month")]
        [Required(ErrorMessage = "Please enter your credit cards expiry month")]
        [ValidExpiry("ExpiryMonth", "ExpiryYear", ErrorMessage = " ")]
        public int ExpiryMonth { get; set; }

        [Display(Name = "Expiry Year")]
        [Required(ErrorMessage = "Please enter your credit cards expiry year")]
        [ValidExpiry("ExpiryMonth", "ExpiryYear", ErrorMessage = "Please enter a valid expiry date")]
        public int ExpiryYear { get; set; }

        [Display(Name = "CCV")]
        [Required(ErrorMessage = "Please enter your ccv number")]
        public string CCV { get; set; }

        public IList<SelectListItem> ExpiryMonths { get; set; }

        public IList<SelectListItem> ExpiryYears { get; set; }

        public string EwayUrl { get; set; }

        public string EwayAccessCode { get; set; }

        [Display(Name = "Credit card number")]
        [Required(ErrorMessage = "Please enter your credit card number")]
        [CreditCardNumber(ErrorMessage = "Please enter a valid credit card number")]
        public string EWAY_CARDNUMBER { get; set; }

        [Display(Name = "Expiry Month")]
        [Required(ErrorMessage = "Please enter your credit cards expiry month")]
        [ValidExpiry("EWAY_CARDEXPIRYMONTH", "EWAY_CARDEXPIRYYEAR", ErrorMessage = "Please enter a valid expiry date")]
        public string EWAY_CARDEXPIRYMONTH { get; set; }

        [Display(Name = "Expiry Year")]
        [Required(ErrorMessage = "Please enter your credit cards expiry year")]
        [ValidExpiry("EWAY_CARDEXPIRYMONTH", "EWAY_CARDEXPIRYYEAR", ErrorMessage = "Please enter a valid expiry date")]
        public string EWAY_CARDEXPIRYYEAR { get; set; }

        [Display(Name = "CCV")]
        [Required(ErrorMessage = "Please enter your ccv number")]
        [CCV(ErrorMessage = "Please enter a valid CCV number")]
        public string EWAY_CARDCVN { get; set; }

        [Display(Name = "Name of card")]
        [Required(ErrorMessage = "Please enter the name on your credit card")]
        public string EWAY_CARDNAME { get; set; }

        public string Error { get; set; }

        public PaymentMethod? PaymentMethod { get; set; }

        public PaymentInformationViewModel()
        {
            IList<int> months = new List<int>() { 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12 };
            this.ExpiryMonths = months.Select(o => new SelectListItem() { Value = o.ToString(), Text = o.ToString() }).ToList();

            this.ExpiryYears = new List<SelectListItem>();

            int year = DateTime.Now.Year;
            while(year < DateTime.Now.AddYears(5).Year)
            {
                this.ExpiryYears.Add(new SelectListItem() { Value = year.ToString(), Text = year.ToString() });
                year++;
            }
        }
    }
}