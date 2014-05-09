using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vintage.Rabbit.Web.Models.Membership
{
    public class LoginRegisterViewModel
    {
        public LoginViewModel Login { get; private set; }

        public RegisterViewModel Register { get; private set; }

        public LoginRegisterViewModel()
        {
            this.Login = new LoginViewModel();
            this.Register = new RegisterViewModel();
        }
    }
}