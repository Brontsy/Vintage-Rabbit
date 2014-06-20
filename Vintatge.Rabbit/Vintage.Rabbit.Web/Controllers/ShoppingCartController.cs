using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Carts.CommandHandlers;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Web.Models.ShoppingCart;

namespace Vintage.Rabbit.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public ShoppingCartController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher; 
            this._commandDispatcher = commandDispatcher;
        }

        public ActionResult PageHeader(Member member, bool isOpen = false)
        {
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Guid));

            return this.PartialView("PageHeader", new CartViewModel(cart, isOpen));
        }

        public ActionResult Checkout(Member member)
        {
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Guid));

            return this.PartialView("Checkout", new CartViewModel(cart, true));
        }


        public ActionResult Add(int productId, Member member, Order order, int qty = 1)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));

            this._commandDispatcher.Dispatch(new AddBuyProductToCartCommand(member.Guid, qty, product));

            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Guid));

            this.ClearOrders(order);

            return this.Json(cart, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddHireProduct(int productId, Member member, Order order, DateTime partyDate, int qty = 1)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));

            this._commandDispatcher.Dispatch(new AddHireProductToCartCommand(member.Guid, qty, product, this.GetHireStartDate(partyDate), this.GetHireEndDate(partyDate)));

            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Guid));

            this.ClearOrders(order);

            return this.Json(cart, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(Guid cartItemId, Member member)
        {
            this._commandDispatcher.Dispatch(new RemoveCartItemCommand(member.Guid, cartItemId));

            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Guid));

            return this.Json(cart, JsonRequestBehavior.AllowGet);
        }

        private void ClearOrders(Order order)
        {
            if(this.Response.Cookies["OrderGuid"] != null)
            {
                HttpCookie myCookie = new HttpCookie("OrderGuid");
                myCookie.Expires = DateTime.Now.AddDays(-1);
                this.Response.Cookies.Add(myCookie);
            }
        }

        private DateTime GetHireStartDate(DateTime partyDate)
        {
            DateTime date = partyDate;
            while (date.DayOfWeek != DayOfWeek.Friday)
            {
                date = date.AddDays(-1);
            }

            return date;
        }
        private DateTime GetHireEndDate(DateTime partyDate)
        {
            DateTime date = partyDate;
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(1);
            }

            return date;
        }
	}
}