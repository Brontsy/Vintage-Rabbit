using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.CommandHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Payment.CommandHandlers;
using Vintage.Rabbit.Payment.Entities;
using Vintage.Rabbit.Payment.QueryHandlers;
using Vintage.Rabbit.Payment.Services;
using Vintage.Rabbit.Web.Attributes;
using Vintage.Rabbit.Web.Models.Membership;
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

        [CheckoutRegister]
        public ActionResult Index(Member member, Order order)
        {
            if(order == null)
            {
                order = this._createOrderProvider.CreateOrder(member);
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
            return this.View("ShippingInformation", new ShippingInformationViewModel(order));
        }

        [HasOrder]
        [HttpPost]
        public ActionResult ShippingInformation(ShippingInformationViewModel viewModel, Order order, Member member)
        {
            if (this.ModelState.IsValid)
            {
                Address shippingAddress = this._addressProvider.SaveAddress(member, viewModel);
                this._commandDispatcher.Dispatch<AddShippingAddressCommand>(new AddShippingAddressCommand(order, shippingAddress));

                if (viewModel.BillingAddressIsTheSame)
                {
                    Address billingAddress = this._addressProvider.SaveAddress(member, viewModel);
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
            return this.View("BillingInformation", new BillingInformationViewModel());
        }

        [HttpPost]
        public ActionResult BillingInformation(BillingInformationViewModel viewModel, Order order, Member member)
        {
            if (this.ModelState.IsValid)
            {
                Address billingAddress = this._addressProvider.SaveAddress(member, viewModel);
                this._commandDispatcher.Dispatch<AddBillingAddressCommand>(new AddBillingAddressCommand(order, billingAddress));

                return this.RedirectToRoute(Routes.Checkout.PaymentInfo);
            }

            return this.View("BillingInformation    ", viewModel);
        }


        [HttpGet]
        public ActionResult PaymentInfo()
        {
            PaymentInformationViewModel viewModel = new PaymentInformationViewModel();

            return this.View("PaymentInfo", viewModel);
        }

        [HttpPost]
        public ActionResult CreditCard(PaymentInformationViewModel viewModel, Order order, Member member)
        {
            if (this.ModelState.IsValid)
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

            return this.View("PaymentInfo", viewModel);
        }

        public ActionResult Complete()
        {
            return this.View("Complete");
        }
	}
}