using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Admin.Web.Providers;
using Vintage.Rabbit.Admin.Web.Models.Membership;

namespace Vintage.Rabbit.Admin.Web.Controllers
{
    [Authorize]
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel login)
        {
            if (this.ModelState.IsValid)
            {
                LoginResult result = this._loginProvider.Login(this.HttpContext.GetOwinContext().Authentication, login.Email, login.Password, login.RememberMe);

                if (result.Successful)
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

            return this.Login(login.ReturnUrl);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            this._loginProvider.Logout(this.HttpContext.GetOwinContext().Authentication);

            return RedirectToRoute(Routes.Membership.Login);
        }
    }
}