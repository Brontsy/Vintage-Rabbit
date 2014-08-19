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
using Vintage.Rabbit.Carts.CommandHandlers;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.CommandHandlers;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;
using Vintage.Rabbit.Orders.CommandHandlers;
using Vintage.Rabbit.Orders.Entities;
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
        public ActionResult Login(string returnUrl)
        {
            if(this.ControllerContext.IsChildAction)
            {
                return this.PartialView("Login", new LoginViewModel(returnUrl));
            }

            LoginRegisterViewModel viewModel = new LoginRegisterViewModel(returnUrl);

            return View("Index", viewModel);
        }

        [HttpGet]
        public ActionResult Register(string returnUrl)
        {
            if (this.ControllerContext.IsChildAction)
            {
                return this.PartialView("Register", new RegisterViewModel(returnUrl));
            }

            LoginRegisterViewModel viewModel = new LoginRegisterViewModel(returnUrl);

            return View("Index", viewModel);
        }


        [HttpPost]
        public ActionResult Login(LoginViewModel login, Cart cart, Order order)
        {
            if (this.ModelState.IsValid)
            {
                LoginResult result = this._loginProvider.Login(this.Request.GetOwinContext().Authentication, login.Email, login.Password, login.RememberMe);

                if(result.Successful)
                {
                    if(cart != null && cart.MemberId != result.Member.Guid)
                    {
                        // convert cart
                        this._commandDispatcher.Dispatch(new ChangeCartsMemberGuidCommand(cart, result.Member.Guid));
                    }

                    if (order != null && cart.MemberId != result.Member.Guid)
                    {
                        // convert order
                        this._commandDispatcher.Dispatch(new ChangeOrdersMemberGuidCommand(order, result.Member.Guid));
                    }

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

            return this.Login(login.ReturnUrl);
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel register, Member existingMemberRecord, Cart cart)
        {
            if(this.ModelState.IsValid)
            {
                RegisterCommand command = new RegisterCommand(existingMemberRecord.Guid, register.RegisterEmail, register.Password);
                this._commandDispatcher.Dispatch(command);

                Member member = this._queryDispatcher.Dispatch<Member, GetMemberByEmailQuery>(new GetMemberByEmailQuery(register.RegisterEmail));

                // TODO: Update carts memberId
                var result = this._loginProvider.Login(this.Request.GetOwinContext().Authentication, register.RegisterEmail, register.Password, false);

                if (result.Successful)
                {
                    if (cart != null && cart.MemberId != result.Member.Guid)
                    {
                        // convert cart
                        this._commandDispatcher.Dispatch(new ChangeCartsMemberGuidCommand(cart, result.Member.Guid));
                    }

                    if (string.IsNullOrEmpty(register.ReturnUrl))
                    {
                        return this.RedirectToRoute(Routes.Home);
                    }
                    else
                    {
                        return this.Redirect(register.ReturnUrl);
                    }
                }

            }

            return this.Register(register.ReturnUrl);
        }
        public ActionResult Logout()
        {
            this._loginProvider.Logout(this.Request.GetOwinContext().Authentication);

            return this.RedirectToRoute(Routes.Home);
        }
	}
}