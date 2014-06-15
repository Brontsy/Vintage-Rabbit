using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Web.Attributes.Validation;

namespace Vintage.Rabbit.Web.Models.Payment
{
    public class GuestViewModel
    {
        [Required(ErrorMessage = "Please enter a email address")]
        [Email(ErrorMessage = "Plesae enter a valid email address")]
        [Display(Name = "Please enter your email below.")]
        public string Email { get; set; }
    }
}