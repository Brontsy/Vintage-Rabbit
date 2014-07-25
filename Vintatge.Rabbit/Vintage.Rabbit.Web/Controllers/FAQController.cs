using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Emails.CommandHandlers;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Web.Models.ContactUs;

namespace Vintage.Rabbit.Web.Controllers
{
    public class FAQController : Controller
    {
        private ICommandDispatcher _commandDispatcher;

        public FAQController(ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HowItWorks()
        {
            return View("Index");
        }

        public ActionResult TermsAndConditions()
        {
            return View("TermsAndConditions");
        }

        public ActionResult PrivacyPolicy()
        {
            return View("PrivacyPolicy");
        }

        [HttpGet]
        public ActionResult ContactUs(bool sent = false)
        {
            ViewBag.EmailSent = sent;

            return View("ContactUs", new ContactUsViewModel());
        }

        [HttpPost]
        public ActionResult ContactUs(ContactUsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                this._commandDispatcher.Dispatch(new SendContactUsEmailCommand(viewModel.Name, viewModel.Email, viewModel.Comments));
                return this.RedirectToRoute(Routes.ContactUs, new { sent = true });
            }

            return View("ContactUs", viewModel);
        }

        public ActionResult AboutUs()
        {
            return View("AboutUs");
        }
	}
}