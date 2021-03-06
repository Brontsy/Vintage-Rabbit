﻿using System;
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
using Vintage.Rabbit.Parties.CommandHandlers;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Parties.QueryHandlers;
using Vintage.Rabbit.Payment.CommandHandlers;
using Vintage.Rabbit.Payment.Entities;
using Vintage.Rabbit.Payment.Enums;
using Vintage.Rabbit.Payment.QueryHandlers;
using Vintage.Rabbit.Payment.Services;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.Helpers;
using Vintage.Rabbit.Web.Attributes;
using Vintage.Rabbit.Web.Models.Hire;
using Vintage.Rabbit.Web.Models.Membership;
using Vintage.Rabbit.Web.Models.Orders;
using Vintage.Rabbit.Web.Models.Payment;
using Vintage.Rabbit.Web.Providers;

namespace Vintage.Rabbit.Web.Controllers
{
    [RequireHttps]
    public class PaymentController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;
        private IAddressProvider _addressProvider;
        private ICreditCardService _creditCardService;
        private IPayPalService _paypalService;

        public PaymentController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IAddressProvider addressProvider, ICreditCardService creditCardService, IPayPalService paypalService)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
            this._addressProvider = addressProvider;
            this._creditCardService = creditCardService;
            this._paypalService = paypalService;
        }

        public ActionResult Index(Member member, Order order, Cart cart, HireDatesViewModel hireDates)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                Guid orderGuid = Guid.NewGuid();
                if (order == null)
                {
                    this._commandDispatcher.Dispatch(new AddCartItemsToOrderCommand(orderGuid, member, cart, hireDates.PartyDate));
                }
                else
                {
                    orderGuid = order.Guid;
                }

                return this.RedirectToRoute(Routes.Checkout.CustomisedInvitations, new { orderGuid = orderGuid });
            }

            return this.LoginRegister(null);
        }

        public ActionResult LoginRegister(GuestRegisterForm? showGuestOrRegister)
        {
            LoginRegisterViewModel viewModel = new LoginRegisterViewModel();
            viewModel.ShowGuestOrRegister = showGuestOrRegister;

            return this.View("LoginRegister", viewModel);
        }

        public ActionResult Guest(Member member, Order order, Cart cart, HireDatesViewModel hireDates)
        {
            this._commandDispatcher.Dispatch(new RegisterGuestCommand(member.Guid));

            Guid orderGuid = Guid.NewGuid();
            if (order == null)
            {
                this._commandDispatcher.Dispatch(new AddCartItemsToOrderCommand(orderGuid, member, cart, hireDates.PartyDate));
            }
            else
            {
                orderGuid = order.Guid;
            }

            return this.RedirectToRoute(Routes.Checkout.CustomisedInvitations, new { orderGuid = orderGuid });
        }

        [OrderIsValid]
        public ActionResult CustomisedInvitations(Order order, Member member)
        {
            if (order.Items.Any(o => ProductHelper.IsCustomisableInvitation(o.Product)))
            {
                Party party = this._queryDispatcher.Dispatch<Party, GetPartyByOrderGuidQuery>(new GetPartyByOrderGuidQuery(order.Guid));

                InvitationViewModel viewModel = new InvitationViewModel(order, party);

                return this.View("CustomisedInvitations", viewModel);
            }

            return this.RedirectToRoute(Routes.Checkout.PartyHireInformation);
        }

        [OrderIsValid]
        [HttpPost]
        public ActionResult CustomisedInvitations(InvitationViewModel viewModel, Order order, Member member)
        {
            if (this.ModelState.IsValid)
            {
                AddInvitationDetailsCommand command = new AddInvitationDetailsCommand(order, viewModel.PartyDate.Value, viewModel.ChildsName, viewModel.Age, viewModel.PartyTime, viewModel.PartyAddress, viewModel.RSVPDetails, member);
                this._commandDispatcher.Dispatch(command);

                return this.RedirectToRoute(Routes.Checkout.PartyHireInformation);
            }

            return this.CustomisedInvitations(order, member);
        }

        [OrderIsValid]
        public ActionResult PartyHireInformation(Order order, Member member)
        {
            if (order.ContainsHireProducts() || order.ContainsTheme())
            {
                Party party = this._queryDispatcher.Dispatch<Party, GetPartyByOrderGuidQuery>(new GetPartyByOrderGuidQuery(order.Guid));

                PartyHireInformationViewModel viewModel = new PartyHireInformationViewModel(null, order, party);

                if (order.DeliveryAddressId.HasValue)
                {
                    Address address = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(order.DeliveryAddressId.Value));
                    viewModel = new PartyHireInformationViewModel(address, order, party);
                }
                else if (member.DeliveryAddresses.Any())
                {
                    Address address = member.DeliveryAddresses.OrderByDescending(o => o.DateCreated).First();
                    viewModel = new PartyHireInformationViewModel(address, order, party);
                }

                return this.View("PartyHireInformation", viewModel);
            }

            return this.RedirectToRoute(Routes.Checkout.BillingInformation);
        }

        [OrderIsValid]
        [HttpPost]
        public ActionResult PartyHireInformation(PartyHireInformationViewModel viewModel, Order order, Member member, bool isBillingDetailsTheSame = false)
        {
            if (viewModel.IsDelivery && !this.ModelState.IsValid)
            {
                return this.PartyHireInformation(order, member);
            }

            this._commandDispatcher.Dispatch(new CreatePartyCommand(order, viewModel.PartyDate.Value, member));

            if (viewModel.IsDelivery)
            {
                Address deliveryAddress = this._addressProvider.SaveDeliveryAddress(member, viewModel, viewModel.PhoneNumber);
                this._commandDispatcher.Dispatch(new AddDeliveryAddressCommand(order, deliveryAddress, true, true));
                this._commandDispatcher.Dispatch(new AddPartyAddressCommand(order, deliveryAddress, member));


                if (isBillingDetailsTheSame)
                {
                    Address billingAddress = this._addressProvider.SaveBillingAddress(member, viewModel, null);
                    this._commandDispatcher.Dispatch<AddBillingAddressCommand>(new AddBillingAddressCommand(order, billingAddress));
                }
            }
            else
            {
                this._commandDispatcher.Dispatch(new RemoveDeliveryAddressCommand(order));
            }

            return this.RedirectToRoute(Routes.Checkout.BillingInformation);
        }

        [OrderIsValid]
        [HttpGet]
        public ActionResult ShippingInformation(Order order, Member member)
        {
            var viewModel = new AddressViewModel();

            if (order.ShippingAddressId.HasValue)
            {
                Address address = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(order.ShippingAddressId.Value));
                viewModel = new AddressViewModel(address);
            }
            else if (member.ShippingAddresses.Any())
            {
                Address address = member.ShippingAddresses.OrderByDescending(o => o.DateCreated).First();
                viewModel = new AddressViewModel(address);
            }

            return this.View("ShippingInformation", viewModel);
        }

        [OrderIsValid]
        [HttpPost]
        public ActionResult ShippingInformation(AddressViewModel viewModel, Order order, Member member)
        {
            if (this.ModelState.IsValid)
            {
                Address shippingAddress = this._addressProvider.SaveShippingAddress(member, viewModel);
                this._commandDispatcher.Dispatch<AddShippingAddressCommand>(new AddShippingAddressCommand(order, shippingAddress));

                return this.RedirectToRoute(Routes.Checkout.PaymentInfo);
            }

            return this.View("ShippingInformation", viewModel);
        }


        [HttpGet]
        [OrderIsValid]
        public ActionResult BillingInformation(Order order, Member member)
        {
            var viewModel = new BillingAddressViewModel(member, order);

            if (order.BillingAddressId.HasValue)
            {
                Address address = this._queryDispatcher.Dispatch<Address, GetAddressByGuidQuery>(new GetAddressByGuidQuery(order.BillingAddressId.Value));
                viewModel = new BillingAddressViewModel(member, order, address);
            }
            else if (member.BillingAddresses.Any())
            {
                Address address = member.BillingAddresses.OrderByDescending(o => o.DateCreated).First();
                viewModel = new BillingAddressViewModel(member, order, address);
            }

            return this.View("BillingInformation", viewModel);
        }

        [HttpPost]
        [OrderIsValid]
        public ActionResult BillingInformation(BillingAddressViewModel viewModel, Order order, Member member, bool? shippingAddressIsTheSame)
        {
            if (this.ModelState.IsValid)
            {
                Address billingAddress = this._addressProvider.SaveBillingAddress(member, viewModel, viewModel.Email);
                this._commandDispatcher.Dispatch<AddBillingAddressCommand>(new AddBillingAddressCommand(order, billingAddress));
            
                if ((shippingAddressIsTheSame.HasValue && shippingAddressIsTheSame.Value))
                {
                    viewModel.Guid = Guid.NewGuid();
                    Address shippingAddress = this._addressProvider.SaveShippingAddress(member, viewModel);
                    this._commandDispatcher.Dispatch<AddShippingAddressCommand>(new AddShippingAddressCommand(order, shippingAddress));

                    return this.RedirectToRoute(Routes.Checkout.PaymentInfo);
                }
                else
                {
                    if (order.ContainsBuyProducts() && !order.ContainsHireProducts() && !order.ContainsTheme())
                    {
                        return this.RedirectToRoute(Routes.Checkout.ShippingInformation, new { guid = string.Empty });
                    }

                    return this.RedirectToRoute(Routes.Checkout.PaymentInfo);
                }
            }

            return this.View("BillingInformation", viewModel);
        }


        [HttpGet]
        [OrderIsValid]
        public ActionResult PaymentInfo(Order order, Member member, PaymentMethod? paymentMethod = null)
        {
            PaymentInformationViewModel viewModel = new PaymentInformationViewModel();
            viewModel.PaymentMethod = paymentMethod;
            var ewayAccess = this._creditCardService.GetEwayAccessCode(order);

            if(ewayAccess != null)
            {
                viewModel.EwayUrl = ewayAccess.FormActionUrl;
                viewModel.EwayAccessCode = ewayAccess.AccessCode;
            }

            if(order.Status == Orders.Enums.OrderStatus.Error)
            {
                viewModel.Error = "Sorry we are unable to process your payment. Please try again";
            }

            return this.View("PaymentInfo", viewModel);
        }

        [OrderIsValid]
        public ActionResult CreditCardComplete(Order order, Member member, string AccessCode)
        {
            var result = this._creditCardService.CompletePayment(order, AccessCode);

            if(result.Successful)
            {
                return this.RedirectToRoute(Routes.Checkout.Complete);
            }
            else
            {
                this.ModelState.AddModelError("Error", result.ErrorMessage);
            }

            return this.PaymentInfo(order, member, PaymentMethod.CreditCard);
        }

        public ActionResult Complete(Order order)
        {
            OrderViewModel viewModel = new OrderViewModel(order);

            return this.View("Complete", viewModel);
        }

        [OrderIsValid]
        public ActionResult PayPal(Order order)
        {
            string url = this._paypalService.Checkout(order);

            return this.Redirect(url);
        }

        [OrderIsValid]
        public ActionResult PayPalSuccess(Order order, Member member, Guid paypalPaymentGuid, string token, string PayerID)
        {
            PayPalPayment payment = this._paypalService.Success(order, paypalPaymentGuid, token, PayerID);

            if(payment.Status == Payment.Enums.PayPalPaymentStatus.Completed)
            {
                return this.RedirectToRoute(Routes.Checkout.Complete);
            }
            else if(payment.Status == PayPalPaymentStatus.Error)
            {
                this.ModelState.AddModelError("Error", payment.Errors.First().ErrorMessage);
            }

            return this.PaymentInfo(order, member, PaymentMethod.PayPal);
        }

        [OrderIsValid]
        public ActionResult PayPalCancel(Order order, Guid paypalPaymentGuid, string token)
        {
            this._paypalService.Cancel(order, paypalPaymentGuid, token);

            return this.RedirectToRoute(Routes.Checkout.PaymentInfo);
        }

        public ActionResult CheckOrderAvailability(Order order, HireDatesViewModel hireDates)
        {
            IList<IOrderItem> unavailableOrderItems = this._queryDispatcher.Dispatch<IList<IOrderItem>, GetUnavailableOrderItemsQuery>(new GetUnavailableOrderItemsQuery(order, hireDates.PartyDate));

            IList<OrderItemViewModel> orderItems = unavailableOrderItems.Select(o => new OrderItemViewModel(o)).ToList();

            return this.PartialView("OrderAvailability", orderItems);
        }

        public ActionResult OrderSummary(Order order)
        {
            return this.PartialView("OrderSummary", new OrderViewModel(order));
        }
	}
}
