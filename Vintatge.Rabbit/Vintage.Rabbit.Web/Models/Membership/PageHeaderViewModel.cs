using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vintage.Rabbit.Web.Models.Membership
{
    public class PageHeaderViewModel
    {
        public bool IsLoggedIn { get; private set; }

        public string Email { get; private set; }

        public PageHeaderViewModel(bool IsLoggedIn, string email = null)
        {
            this.IsLoggedIn = IsLoggedIn;
            this.Email = email;
        }
    }
}