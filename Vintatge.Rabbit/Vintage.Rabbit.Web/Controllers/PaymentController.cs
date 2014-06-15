using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Membership.CommandHandlers;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;
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

        public ActionResult Index(Member member, Order order, Cart cart)
        {
            if(order == null)
            {
                order = this._createOrderProvider.CreateOrder(member, cart);
            }

            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                return this.RedirectToRoute(Routes.Checkout.BillingInformation);
            }

            return this.LoginRegister(null);
        }

        public ActionResult LoginRegister(GuestRegisterForm? showGuestOrRegister)
        {
            LoginRegisterViewModel viewModel = new LoginRegisterViewModel();
            viewModel.ShowGuestOrRegister = showGuestOrRegister;

            return this.View("LoginRegister", viewModel);
        }

        public ActionResult Guest(Member member)
        {
            this._commandDispatcher.Dispatch(new RegisterGuestCommand(member.Guid));

            return this.RedirectToRoute(Routes.Checkout.BillingInformation);
        }

        [HasOrder]
        [HttpGet]
        public ActionResult ShippingInformation(Order order)
        {
            if(order.ShippingAddressId.HasValue)
            {
                Address shippingAddress = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(order.ShippingAddressId.Value));
                return this.View("ShippingInformation", new AddressViewModel(shippingAddress));
            }

            return this.View("ShippingInformation", new AddressViewModel());
        }

        [HasOrder]
        [HttpPost]
        public ActionResult ShippingInformation(AddressViewModel viewModel, Order order, Member member)
        {
            if (this.ModelState.IsValid)
            {
                Address shippingAddress = this._addressProvider.SaveShippingAddress(member, viewModel);
                this._commandDispatcher.Dispatch<AddShippingAddressCommand>(new AddShippingAddressCommand(order, shippingAddress));

                if(order.Items.Any(o => o.Product.Type == ProductType.Hire))
                {
                    return this.RedirectToRoute(Routes.Checkout.Devliery);
                }

                return this.RedirectToRoute(Routes.Checkout.PaymentInfo);
            }

            return this.View("ShippingInformation", viewModel);
        }


        [HasOrder]
        [HttpGet]
        public ActionResult Delivery(Order order)
        {
            if (order.DeliveryAddressId.HasValue)
            {
                Address deliveryAddress = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(order.DeliveryAddressId.Value));
                return this.View("Delivery", new DeliveryAddressViewModel(deliveryAddress));
            }

            return this.View("Delivery", new DeliveryAddressViewModel());
        }

        [HasOrder]
        [HttpGet]
        public ActionResult PickupHiredProducts(Order order)
        {
            this._commandDispatcher.Dispatch<RemoveDeliveryAddressCommand>(new RemoveDeliveryAddressCommand(order));

            return this.RedirectToRoute(Routes.Checkout.PaymentInfo);
        }

        [HasOrder]
        [HttpPost]
        public ActionResult Delivery(DeliveryAddressViewModel viewModel, Order order, Member member)
        {
            if (this.ModelState.IsValid)
            {
                Address deliveryAddress = this._addressProvider.SaveDeliveryAddress(member, viewModel);
                this._commandDispatcher.Dispatch<AddDeliveryAddressCommand>(new AddDeliveryAddressCommand(order, deliveryAddress));

                return this.RedirectToRoute(Routes.Checkout.PaymentInfo);
            }

            return this.View("ShippingInformation", viewModel);
        }


        [HttpGet]
        public ActionResult BillingInformation(Order order, Member member)
        {
            if (order.BillingAddressId.HasValue)
            {
                Address billingAddress = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(order.BillingAddressId.Value));
                return this.View("BillingInformation", new BillingAddressViewModel(billingAddress));
            }

            return this.View("BillingInformation", new BillingAddressViewModel(member));
        }

        [HttpPost]
        public ActionResult BillingInformation(BillingAddressViewModel viewModel, Order order, Member member, bool shippingAddressIsTheSame)
        {
            if (this.ModelState.IsValid)
            {
                Address billingAddress = this._addressProvider.SaveBillingAddress(member, viewModel);
                this._commandDispatcher.Dispatch<AddBillingAddressCommand>(new AddBillingAddressCommand(order, billingAddress));

                if(shippingAddressIsTheSame)
                {
                    Address shippingAddress = this._addressProvider.SaveShippingAddress(member, viewModel);
                    this._commandDispatcher.Dispatch<AddShippingAddressCommand>(new AddShippingAddressCommand(order, shippingAddress));

                    if (order.Items.Any(o => o.Product.Type == ProductType.Hire))
                    {
                        return this.RedirectToRoute(Routes.Checkout.Devliery);
                    }

                    return this.RedirectToRoute(Routes.Checkout.PaymentInfo);
                }

                return this.RedirectToRoute(Routes.Checkout.ShippingInformation);
            }

            return this.View("BillingInformation", viewModel);
        }


        [HttpGet]
        public ActionResult PaymentInfo(Order order, Member member)
        {
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
                        return this.RedirectToRoute(Routes.Checkout.Complete, new { orderId = order.Guid });
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