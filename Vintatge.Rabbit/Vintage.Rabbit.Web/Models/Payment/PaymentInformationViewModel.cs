using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public int ExpiryMonth { get; set; }

        [Display(Name = "Expiry Year")]
        [Required(ErrorMessage = "Please enter your credit cards expiry year")]
        public int ExpiryYear { get; set; }

        [Display(Name = "CCV")]
        [Required(ErrorMessage = "Please enter your ccv number")]
        public string CCV { get; set; }

        public IList<SelectListItem> ExpiryMonths { get; set; }

        public IList<SelectListItem> ExpiryYears { get; set; }

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