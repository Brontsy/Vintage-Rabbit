﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Web.Attributes;
using Vintage.Rabbit.Web.Models.Membership;
using Vintage.Rabbit.Web.Models.Payment;

namespace Vintage.Rabbit.Web.Controllers
{
    public class PaymentController : Controller
    {
        [CheckoutRegister]
        public ActionResult Index()
        {
            return View("Index");
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

        [HttpGet]
        public ActionResult ShippingInformation()
        {
            return this.View("ShippingInformation", new ShippingInformationViewModel());
        }

        [HttpPost]
        public ActionResult ShippingInformation(ShippingInformationViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                if (viewModel.BillingAddressIsTheSame)
                {
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
        public ActionResult BillingInformation(BillingInformationViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                return this.RedirectToRoute(Routes.Checkout.PaymentInfo);
            }

            return this.View("BillingInformation    ", viewModel);
        }


        [HttpGet]
        public ActionResult PaymentInfo()
        {
            return this.View("PaymentInfo");
        }

        [HttpPost]
        public ActionResult PaymentInfo(PaymentInfoViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                return this.RedirectToRoute(Routes.Checkout.Confirm);
            }

            return this.View("PaymentInfo", viewModel);
        }

        public ActionResult Confirm()
        {
            return this.View("Confirm");
        }

        public ActionResult Confirmed()
        {
            return this.RedirectToRoute(Routes.Checkout.Complete);
        }

        public ActionResult Complete()
        {
            return this.View("PaymentInfo");
        }
	}
}