using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vintage.Rabbit.Web.Models.Membership
{
    public enum GuestRegisterForm
    {
        Guest,
        Register
    }

    public class LoginRegisterViewModel
    {
        public LoginViewModel Login { get; private set; }

        public RegisterViewModel Register { get; private set; }

        public GuestRegisterForm? ShowGuestOrRegister { get; set; }

        public LoginRegisterViewModel(string returnUrl = null)
        {
            this.Login = new LoginViewModel(returnUrl);
            this.Register = new RegisterViewModel(returnUrl);
        }
    }
}