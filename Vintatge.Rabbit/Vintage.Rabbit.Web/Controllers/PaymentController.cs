using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.CommandHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.QueryHandlers;
using Vintage.Rabbit.Payment.CommandHandlers;
using Vintage.Rabbit.Payment.Entities;
using Vintage.Rabbit.Payment.QueryHandlers;
using Vintage.Rabbit.Payment.Services;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Web.Attributes;
using Vintage.Rabbit.Web.Models.Membership;
using Vintage.Rabbit.Web.Models.Orders;
using Vintage.Rabbit.Web.Models.Payment;
using Vintage.Rabbit.Web.Providers;

namespace Vintage.Rabbit.Web.Controllers
{
    public class PaymentController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;
        private ICreateOrderProvider _createOrderProvider;
        private IAddressProvider _addressProvider;
        private ICreditCardService _creditCardService;

        public PaymentController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, ICreateOrderProvider createOrderProvider, IAddressProvider addressProvider, ICreditCardService creditCardService)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
            this._createOrderProvider = createOrderProvider;
            this._addressProvider = addressProvider;
            this._creditCardService = creditCardService;
        }

        public ActionResult Index(Member member, Order order)
        {
            if(order == null)
            {
                order = this._createOrderProvider.CreateOrder(member);
            }

            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                return new RedirectToRouteResult(Routes.Checkout.ShippingInformation, new System.Web.Routing.RouteValueDictionary());
            }

            return this.LoginRegister();
        }

        public ActionResult LoginRegister()
        {
            LoginRegisterViewModel viewModel = new LoginRegisterViewModel();

            return this.View("LoginRegister", viewModel);
        }

        public ActionResult Guest()
        {
            return this.RedirectToRoute(Routes.Checkout.ShippingInformation);
        }

        [HasOrder]
        [HttpGet]
        public ActionResult ShippingInformation(Order order)
        {
            return this.View("ShippingInformation", new AddressViewModel(order.ShippingAddress));
        }

        [HasOrder]
        [HttpPost]
        public ActionResult ShippingInformation(AddressViewModel viewModel, Order order, Member member, bool billingAddressIsTheSame)
        {
            if (this.ModelState.IsValid)
            {
                Address shippingAddress = this._addressProvider.SaveShippingAddress(member, viewModel);
                this._commandDispatcher.Dispatch<AddShippingAddressCommand>(new AddShippingAddressCommand(order, shippingAddress));

                if (billingAddressIsTheSame)
                {
                    Address billingAddress = this._addressProvider.SaveBillingAddress(member, viewModel);
                    this._commandDispatcher.Dispatch<AddBillingAddressCommand>(new AddBillingAddressCommand(order, billingAddress));

                    return this.RedirectToRoute(Routes.Checkout.PaymentInfo);
                }

                return this.RedirectToRoute(Routes.Checkout.BillingInformation);
            }

            return this.View("ShippingInformation", viewModel);
        }


        [HttpGet]
        public ActionResult BillingInformation()
        {
            return this.View("BillingInformation", new AddressViewModel());
        }

        [HttpPost]
        public ActionResult BillingInformation(AddressViewModel viewModel, Order order, Member member)
        {
            if (this.ModelState.IsValid)
            {
                Address billingAddress = this._addressProvider.SaveBillingAddress(member, viewModel);
                this._commandDispatcher.Dispatch<AddBillingAddressCommand>(new AddBillingAddressCommand(order, billingAddress));

                return this.RedirectToRoute(Routes.Checkout.PaymentInfo);
            }

            return this.View("BillingInformation    ", viewModel);
        }


        [HttpGet]
        public ActionResult PaymentInfo(Order order, Member member)
        {
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Guid));

            this._commandDispatcher.Dispatch(new AddCartItemsToOrderCommand(order, cart));

            PaymentInformationViewModel viewModel = new PaymentInformationViewModel();

            return this.View("PaymentInfo", viewModel);
        }

        [HttpPost]
        public ActionResult CreditCard(PaymentInformationViewModel viewModel, Order order, Member member)
        {
            if (this.ModelState.IsValid)
            {
                IList<IOrderItem> unavailableOrderItems = this._queryDispatcher.Dispatch<IList<IOrderItem>, GetUnavailableOrderItemsQuery>(new GetUnavailableOrderItemsQuery(order));
                if (unavailableOrderItems.Count == 0)
                {
                    PaymentResult result = this._creditCardService.PayForOrder(order, viewModel.Name, viewModel.CreditCardNumber, viewModel.ExpiryMonth, viewModel.ExpiryYear, viewModel.CCV);

                    if (result.Successful)
                    {
                        return this.RedirectToRoute(Routes.Checkout.Complete, new { orderId = order.Id });
                    }
                    else
                    {
                        this.ModelState.AddModelError("CreditCardNumber", result.ErrorMessage);
                    }
                }
            }

            return this.View("PaymentInfo", viewModel);
        }

        public ActionResult Complete()
        {
            return this.View("Complete");
        }

        public ActionResult CheckOrderAvailability(Order order)
        {
            IList<IOrderItem> unavailableOrderItems = this._queryDispatcher.Dispatch<IList<IOrderItem>, GetUnavailableOrderItemsQuery>(new GetUnavailableOrderItemsQuery(order));

            IList<OrderItemViewModel> orderItems = unavailableOrderItems.Select(o => new OrderItemViewModel(o)).ToList();

            return this.PartialView("OrderAvailability", orderItems);
        }

        public ActionResult OrderSummary(Order order)
        {
            return this.PartialView("OrderSummary", new OrderViewModel(order));
        }
	}
}