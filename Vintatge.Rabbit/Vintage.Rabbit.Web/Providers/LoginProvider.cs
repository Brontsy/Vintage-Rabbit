using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;

namespace Vintage.Rabbit.Web.Providers
{
    public interface ILoginProvider
    {
        LoginResult Login(IAuthenticationManager authenticationManager, string email, string password, bool rememberMe);

        void Logout(IAuthenticationManager authenticationManager);

    }

    public class LoginResult {

        public bool Successful { get; private set; }

        private LoginResult() { }
        
        public static LoginResult Success()
        {
            return new LoginResult() { Successful = true };
        }
        public static LoginResult InvalidUsernamePassword()
        {
            return new LoginResult() { Successful = false };
        }
    }

    public class LoginProvider : ILoginProvider
    {
        private IQueryDispatcher _queryDispatcher;

        public LoginProvider(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public LoginResult Login(IAuthenticationManager authenticationManager, string email, string password, bool rememberMe)
        {
            bool isValid = this._queryDispatcher.Dispatch<bool, ValidateLoginQuery>(new ValidateLoginQuery(email, password));

            if (isValid)
            {
                Member member = this._queryDispatcher.Dispatch<Member, GetMemberByEmailQuery>(new GetMemberByEmailQuery(email));

                var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, member.Id.ToString()), }, DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);

                // if you want roles, just add as many as you want here (for loop maybe?)
                identity.AddClaim(new Claim(ClaimTypes.Role, "guest"));
                // tell OWIN the identity provider, optional
                // identity.AddClaim(new Claim(IdentityProvider, "Simplest Auth"));

                authenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = rememberMe
                }, identity);

                return LoginResult.Success();
            }

            return LoginResult.InvalidUsernamePassword();
        }

        public void Logout(IAuthenticationManager authenticationManager)
        {
            authenticationManager.SignOut();
        }
    }
}