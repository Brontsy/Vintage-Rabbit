using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Web.Models.Membership;

namespace Vintage.Rabbit.Web.Controllers
{
    public class MembershipController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            if(this.ControllerContext.IsChildAction)
            {
                return this.PartialView("Login", new LoginViewModel());
            }

            LoginRegisterViewModel viewModel = new LoginRegisterViewModel();

            return View("Index", viewModel);
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (this.ControllerContext.IsChildAction)
            {
                return this.PartialView("Register", new RegisterViewModel());
            }

            LoginRegisterViewModel viewModel = new LoginRegisterViewModel();

            return View("Index", viewModel);
        }


        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            return null;
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel login)
        {
            return null;
        }
        public ActionResult Logout()
        {
            return null;
        }
	}
}