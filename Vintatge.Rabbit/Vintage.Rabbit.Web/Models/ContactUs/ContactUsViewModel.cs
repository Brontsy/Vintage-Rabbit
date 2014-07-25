using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Web.Attributes.Validation;

namespace Vintage.Rabbit.Web.Models.ContactUs
{
    public class ContactUsViewModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a email address")]
        [Email(ErrorMessage = "Plesae enter a valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Comments")]
        [Required(ErrorMessage = "Please enter your comments")]
        public string Comments { get; set; }
    }
}