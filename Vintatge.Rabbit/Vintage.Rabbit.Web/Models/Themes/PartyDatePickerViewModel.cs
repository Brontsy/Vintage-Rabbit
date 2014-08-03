using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Web.Models.Products;

namespace Vintage.Rabbit.Web.Models.Themes
{
    public class PartyDatePickerViewModel
    {
        public string ThemeTitle { get; private set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? PartyDate { get; private set; }

        public PartyDatePickerViewModel() { }

        public PartyDatePickerViewModel(Theme theme)
        {
            this.ThemeTitle = theme.Title;
        }
    }
}