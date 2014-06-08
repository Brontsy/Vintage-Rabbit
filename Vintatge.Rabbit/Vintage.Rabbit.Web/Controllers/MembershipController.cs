using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.CommandHandlers;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;
using Vintage.Rabbit.Web.Models.Membership;
using Vintage.Rabbit.Web.Providers;

namespace Vintage.Rabbit.Web.Controllers
{
    public class MembershipController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ILoginProvider _loginProvider;
        private ICommandDispatcher _commandDispatcher;

        public MembershipController(IQueryDispatcher queryDispatcher, ILoginProvider loginProvider, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._loginProvider = loginProvider;
            this._commandDispatcher = commandDispatcher;
        }

        public ActionResult PageHeader(Member member)
        {
            bool isLoggedIn = this.HttpContext.User.Identity.IsAuthenticated;
            string email = null;

            if(isLoggedIn)
            {
                email = member.Email;
            }

            PageHeaderViewModel viewModel = new PageHeaderViewModel(isLoggedIn, email);
            return this.PartialView("PageHeader", viewModel);
        }

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
            if (this.ModelState.IsValid)
            {
                LoginResult result = this._loginProvider.Login(this.Request.GetOwinContext().Authentication, login.Email, login.Password, login.RememberMe);

                if(result.Successful)
                {
                    if (string.IsNullOrEmpty(login.ReturnUrl))
                    {
                        return this.RedirectToRoute(Routes.Home);
                    }
                    else
                    {
                        return this.Redirect(login.ReturnUrl);
                    }
                }
                else
                {
                    this.ModelState.AddModelError("Email", "Invalid username or email. Please try again");
                }
            }

            return this.Login();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel register)
        {
            if(this.ModelState.IsValid)
            {
                RegisterCommand command = new RegisterCommand(register.RegisterEmail, register.Password);
                this._commandDispatcher.Dispatch(command);

                Member member = this._queryDispatcher.Dispatch<Member, GetMemberByEmailQuery>(new GetMemberByEmailQuery(register.RegisterEmail));

                this._loginProvider.Login(this.Request.GetOwinContext().Authentication, register.RegisterEmail, register.Password, false);

                if (string.IsNullOrEmpty(register.ReturnUrl))
                {
                    return this.RedirectToRoute(Routes.Home);
                }
                else
                {
                    return this.Redirect(register.ReturnUrl);
                }

            }

            return this.Register();
        }
        public ActionResult Logout()
        {
            this._loginProvider.Logout(this.Request.GetOwinContext().Authentication);

            return this.RedirectToRoute(Routes.Home);
        }
	}
}