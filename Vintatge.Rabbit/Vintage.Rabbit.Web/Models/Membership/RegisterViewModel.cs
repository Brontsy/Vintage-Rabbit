using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Web.Attributes.Validation;

namespace Vintage.Rabbit.Web.Models.Membership
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter a email address")]
        [Email(ErrorMessage = "Plesae enter a valid email address")]
        public string RegisterEmail { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [MinLength(6, ErrorMessage = "Your password must be greater then 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [Compare("Password", ErrorMessage = "The password and confirmation do not match.")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }

        public RegisterViewModel(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
        }
        public RegisterViewModel() { }
    }
}