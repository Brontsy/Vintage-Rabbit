using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;
using Vintage.Rabbit.Orders.CommandHandlers;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Web.Providers
{
    public interface ICreateOrderProvider
    {
        Order CreateOrder(Member member, Cart cart);
    }

    public class CreateOrderProvider : ICreateOrderProvider
    {
        private ICommandDispatcher _commandDispatcher;

        public CreateOrderProvider(ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Order CreateOrder(Member member, Cart cart)
        {
            Order order = new Order(member.Guid);
            this._commandDispatcher.Dispatch(new AddCartItemsToOrderCommand(order, cart));

            HttpCookie myCookie = new HttpCookie("OrderGuid");

            myCookie.Value = order.Guid.ToString();
            myCookie.Expires = DateTime.Now.AddDays(3);

            HttpContext.Current.Response.Cookies.Add(myCookie);

            return order;
        }
    }
}