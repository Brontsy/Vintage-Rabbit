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
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Themes.QueryHandlers;
using Vintage.Rabbit.Web.Models.Hire;
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

        public ActionResult UpdateQty(Member member, Guid cartItemId, int qty, HireDatesViewModel hireDates)
        {
            qty = (qty <= 1 ? 1 : qty);
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Guid));
            
            this._commandDispatcher.Dispatch(new UpdateQuantityCommand(cart, cartItemId, qty, hireDates.PartyDate));

            return null;
        }


        public ActionResult Add(int productId, Member member, Order order, int qty = 1)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));

            this._commandDispatcher.Dispatch(new AddBuyProductToCartCommand(member.Guid, qty, product));

            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Guid));

            return this.Json(cart, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddHireProduct(int productId, Member member, Order order, DateTime partyDate, int qty = 1)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));

            this._commandDispatcher.Dispatch(new AddHireProductToCartCommand(member.Guid, qty, product, partyDate));

            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Guid));

            return this.Json(cart, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddTheme(string name, Guid themeGuid, Member member, HireDatesViewModel dates)
        {
            if (dates.PartyDate.HasValue)
            {
                Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(themeGuid));
                this._commandDispatcher.Dispatch(new AddThemeToCartCommand(member.Guid, theme, dates.PartyDate.Value));
            }

            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Guid));

            return this.Json(cart, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(Guid cartItemId, Member member)
        {
            this._commandDispatcher.Dispatch(new RemoveCartItemCommand(member.Guid, cartItemId));

            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Guid));

            return this.Json(cart, JsonRequestBehavior.AllowGet);
        }
	}
}