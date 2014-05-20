using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Web.Attributes.Validation;

namespace Vintage.Rabbit.Web.Models.Membership
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter a email address")]
        [Email(ErrorMessage = "Plesae enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [MinLength(6, ErrorMessage = "Your password must be greater then  characters")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}