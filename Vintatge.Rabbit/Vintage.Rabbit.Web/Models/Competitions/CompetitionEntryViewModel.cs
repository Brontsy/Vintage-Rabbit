using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Models.Competitions
{
    public class CompetitionEntryViewModel
    {
        [Display(Name = "Name*")]
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please choose the day you were born")]
        public int DOBDay { get; set; }

        [Required(ErrorMessage = "Please choose the month you were born")]
        public int DOBMonth { get; set; }

        [Required(ErrorMessage = "Please choose the year you were born")]
        public int DOBYear { get; set; }

        
        [Display(Name = "Date of Birth*")]
        [Required(ErrorMessage = "Please enter a date of birth")]
        public DateTime? DateOfBirth
        {
            get
            {
                try
                {
                    return new DateTime(this.DOBYear, this.DOBMonth, this.DOBDay);
                }
                catch (Exception e) { }

                return null;
            }
        }

        [Display(Name = "Email*")]
        [Required(ErrorMessage = "Please enter your email")]
        public string Email { get; set; }

        [Display(Name = "Phone number*")]
        [Required(ErrorMessage = "Please enter your phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "What was your favourite childhood birthday memory?")]
        [Required(ErrorMessage = "Please answer the question to enter the competition")]
        public string EntryText { get; set; }

        public IList<SelectListItem> Days
        {
            get
            {
                IList<SelectListItem> days = new List<SelectListItem>();
                for (int i = 1; i <= 31; i++)
                {
                    days.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }

                return days;
            }
        }

        public IList<SelectListItem> Months
        {
            get
            {
                IList<SelectListItem> months = new List<SelectListItem>();
                for (int i = 1; i <= 12; i++)
                {
                    DateTime date = new DateTime(2014, i, 1);
                    months.Add(new SelectListItem() { Text = date.ToString("MMMM"), Value = i.ToString() });
                }

                return months;
            }
        }

        public IList<SelectListItem> Years
        {
            get
            {
                IList<SelectListItem> years = new List<SelectListItem>();
                for (int i = 2014; i > 1900; i--)
                {
                    years.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }

                return years;
            }
        }
    }
}