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
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Id));

            return this.PartialView("PageHeader", new CartViewModel(cart, isOpen));
        }

        public ActionResult Checkout(Member member)
        {
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Id));

            return this.PartialView("Checkout", new CartViewModel(cart, true));
        }


        public ActionResult Add(int productId, Member member, int qty = 1)
        {
            BuyProduct product = this._queryDispatcher.Dispatch<BuyProduct, GetBuyProductQuery>(new GetBuyProductQuery(productId));

            this._commandDispatcher.Dispatch(new AddBuyProductToCartCommand(member.Id, qty, product));

            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Id));

            return this.Json(cart, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddHireProduct(int productId, Member member, DateTime startDate, DateTime endDate)
        {
            HireProduct product = this._queryDispatcher.Dispatch<HireProduct, GetHireProductQuery>(new GetHireProductQuery(productId));

            this._commandDispatcher.Dispatch(new AddHireProductToCartCommand(member.Id, product, startDate, endDate));

            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Id));

            return this.Json(cart, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(Guid cartItemId, Member member)
        {
            this._commandDispatcher.Dispatch(new RemoveCartItemCommand(member.Id, cartItemId));

            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Id));

            return this.Json(cart, JsonRequestBehavior.AllowGet);
        }
	}
}